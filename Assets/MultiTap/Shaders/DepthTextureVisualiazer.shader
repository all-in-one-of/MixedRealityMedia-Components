Shader "Unlit/DepthTextureVisualiazer"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Saturate ("Saturation Multiplier", Range(0.5, 15)) = 1
		[Toggle] _Invert ("Invert Depth Texture", Float) = 0
		[Toggle] _OpenGL ("OpenGL Renderbackend", Float) = 0
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
			
			float _Saturate;
			float _Invert;
			float _OpenGL;

			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed depthVal = tex2D(_MainTex, i.uv).r;
				if(_OpenGL) {
					depthVal = (depthVal + 1) / 2;
				}
				fixed4 col = fixed4(depthVal.rrr, 1);

				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				col *= _Saturate;
				if(_Invert) {
					return 1 - col;
				}
				return col;
 			}
			ENDCG
		}
	}
}
