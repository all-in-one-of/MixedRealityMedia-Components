Shader "Unlit/Tri Step Shader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_AlphaTex ("Alpha Texture", 2D) = "white" {}
		_WebcamTex ("Webcam Texture", 2D) = "white" {}
		_WebcamMask ("Webcam Mask", 2D) = "white" {}
		_TargetColor ("Target Color", Color) = (0, 1, 0, 1)
		_SpillRemoval ("Spill Removal", Range(0, 2)) = 0.18
		_Tolerance ("Tolerance", Range(0, 2)) = 0.1
		_Threshold ("Threshold", Range(0, 2)) = 0.4
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
			
			sampler2D _AlphaTex;
			sampler2D _WebcamTex;
			sampler2D _WebcamMask;
			fixed4 _TargetColor;

			fixed4 mixCol(fixed4 front, fixed4 median) {
				fixed alphaRef = median.a; // that's what's left of alpha
				fixed mixAlpha = alphaRef * (1 - front.a); // that's the alpha left after mixing front and back
				return front * (1 - mixAlpha) + median * mixAlpha;
			}

			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
				fixed4 alpha = tex2D(_AlphaTex, i.uv);
				col.a = alpha.a;
				fixed4 webCol = tex2D(_WebcamTex, i.uv);
				fixed4 webMask = tex2D(_WebcamMask, i.uv);
				// return 1 - webMask.aaaa;
				webCol.a = 1 - webMask.a;
				webCol = chromaKey(webCol, _TargetColor);
				col = mixCol(col, webCol);
				return col;
				if(webCol.a == 1) {
					return webCol;
				}
				return col;
				col = mixCol(col, webCol); // chromaKey(webCol, _TargetColor));
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}
	}
}