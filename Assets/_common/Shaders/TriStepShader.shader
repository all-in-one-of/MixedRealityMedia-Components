Shader "Unlit/Tri Step Shader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_AlphaTex ("Alpha Texture", 2D) = "white" {}
		_WebcamTex ("Webcam Texture", 2D) = "white" {}
		_WebcamMask ("Webcam Mask", 2D) = "white" {}
		_LightTex ("Light Texture", 2D) = "white" {}
		_TargetColor ("Target Color", Color) = (0, 1, 0, 1)
		_SpillRemoval ("Spill Removal", Range(0, 2)) = 0.18
		_Tolerance ("Uppper Cut Off", Range(0, 5)) = 0.1
		_Threshold ("Lower Cut Off", Range(0, 5)) = 0.4
		[Toggle] _AlphaBlur("Alpha Blur Lookup", Float) = 0
		[Toggle] _DebugAlpha ("Debug Alpha Out", Float) = 0
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

			float4 _MainTex_TexelSize;
			sampler2D _AlphaTex;
			sampler2D _WebcamTex;
			sampler2D _WebcamMask;
			sampler2D _LightTex;
			fixed4 _TargetColor;
			int _AlphaBlur;
			int _DebugAlpha;

			fixed4 mixCol(fixed4 front, fixed4 median) {
				fixed alphaRef = median.a; // that's what's left of alpha
				fixed mixAlpha = alphaRef * (1 - front.a); // that's the alpha left after mixing front and back
				return front * (1 - mixAlpha) + median * mixAlpha;
			}

			fixed ChromaMin(float2 uv, fixed a) {
				float4 duv = _MainTex_TexelSize.xyxy * float4(-0.5, -0.5, 0.5, 0.5);
				fixed4 lowerleft = fixed4(tex2D(_WebcamTex, uv + duv.xy).rgb, a);
				fixed4 lowerright = fixed4(tex2D(_WebcamTex, uv + duv.zy).rgb, a);
				fixed4 upperleft = fixed4(tex2D(_WebcamTex, uv + duv.xw).rgb, a);
				fixed4 upperright = fixed4(tex2D(_WebcamTex, uv + duv.zw).rgb, a);
				float alpha = chromaKey(lowerleft, _TargetColor).w;
				alpha = min(alpha, chromaKey(lowerright, _TargetColor).w);
				alpha = min(alpha, chromaKey(upperleft, _TargetColor).w);
				alpha = min(alpha, chromaKey(upperright, _TargetColor).w);
				return alpha;
			}

			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
				fixed4 alpha = tex2D(_AlphaTex, i.uv);
				col.a = alpha.a;
				fixed4 webCol = tex2D(_WebcamTex, i.uv);
				fixed4 webMask = tex2D(_WebcamMask, i.uv);
				fixed4 light = tex2D(_LightTex, i.uv);

				// return 1 - webMask.aaaa;
				webCol.a = 1 - webMask.a;
				float alp;
				if(_AlphaBlur) {
					alp = ChromaMin(i.uv, 1 - webMask.a);
				} else {
					alp = chromaKey(webCol, _TargetColor).w;
				}
				webCol.a = alp;
				if(_DebugAlpha) {
					return webCol.aaaa;
				}
				col = mixCol(col, webCol * light);
				return col;
				if(webCol.a == 1) {
					return webCol;
				}
				return col;
				col = mixCol(col, webCol); // chromaKey(webCol, _TargetColor));
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col * tex2D(_LightTex, i.uv);
			}
			ENDCG
		}
	}
}