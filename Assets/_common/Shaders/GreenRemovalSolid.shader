Shader "Unlit/GreenRemovalSolid"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_TargetColor ("Target Color", Color) = (0, 1, 0, 1)
		_SpillRemoval ("Spill Removal", Range(0, 2)) = 0.2
		_Tolerance ("Tolerance", Range(0, 5)) = 0.2
		_Threshold ("Threshold", Range(0, 5)) = 0.2
		_Lerp ("Lerp", Range(0, 1)) = 0.5
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
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;

			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);

				return o;
			}
			
			float4 _MainTex_TexelSize;
			float4 _TargetColor;
			float _ColorTimer;
			int _AlphaBlur;
			int _Debug;

			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
				if(_AlphaBlur) {
					col.a = ChromaMin(i.uv, _MainTex_TexelSize, _MainTex, _TargetColor);
				} else {
					col.a = chromaKey(col, _TargetColor).w;
				}

				if(_Debug) {
					return col.aaaa;
				}
				col = fixed4(spillRemoval(col.rgb, _TargetColor.rgb), col.a);

				float time = pow(_Time.y, _ColorTimer);
				fixed4 tCol = fixed4(
					sin(time),
					cos(time),
					sin(time) + cos(time * 0.7),
					1
				);
				tCol *= _ColorTimer;
				return col * col.a + tCol * (1 - col.a);
				return col;
			}
			ENDCG
		}
	}
}
