Shader "Unlit/GreenRemovalSolid"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_TargetColor ("Target Color", Color) = (0, 1, 0, 1)
		_SpillRemoval ("Spill Removal", Range(0, 2)) = 0.2
		_Tolerance ("Tolerance", Range(0, 1)) = 0.2
		_Threshold ("Threshold", Range(0, 5)) = 0.2
		_ColorTimer ("Color Rotation Timer", Range(0, 10)) = 1
		_YCgCoMult ("YCgCo Multiplier", Range(0, 200)) = 50
		[Toggle] _YCgCo("YCgCo Method", Float) = 0
		[Toggle] _Debug("Debug Out", Float) = 0
		[Toggle] _AlphaBlur("Alpha Blur Lookup", Float) = 0
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"
			#include "GreenScreen.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			float4 _TargetColor;
			float _ColorTimer;
			float4 _MainTex_TexelSize;
			int _AlphaBlur;
			int _Debug;
			
			fixed ChromaMin(float2 uv) {
				float4 duv = _MainTex_TexelSize.xyxy * float4(-0.5, -0.5, 0.5, 0.5);
				float alpha = chromaKey(tex2D(_MainTex, uv + duv.xy), _TargetColor).w;
				alpha = min(alpha, chromaKey(tex2D(_MainTex, uv + duv.zy), _TargetColor).w);
				alpha = min(alpha, chromaKey(tex2D(_MainTex, uv + duv.xw), _TargetColor).w);
				alpha = min(alpha, chromaKey(tex2D(_MainTex, uv + duv.zw), _TargetColor).w);
				return alpha;
			}

			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				float alpha;
				if(_AlphaBlur) {
					alpha = ChromaMin(i.uv);
				} else {
					alpha = chromaKey(col, _TargetColor).w;
				}

				if(_Debug) {
					return fixed4(alpha, alpha, alpha, 1) * 100;
				}
				col = fixed4(spillRemoval(col.rgb, _TargetColor.rgb), 1);
				// col = chromaKey(col, _TargetColor);
				if(alpha < 0.01) {
					float time = pow(_Time.y, _ColorTimer);
					col = fixed4(
						sin(time),
						cos(time),
						sin(time) + cos(time * 0.7),
						1
					);
					col *= _ColorTimer;
				}
				return col;
			}
			ENDCG
		}
	}
}
