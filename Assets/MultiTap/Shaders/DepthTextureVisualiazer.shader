Shader "Unlit/DepthTextureVisualiazer"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Saturate ("Saturation Multiplier", Range(0.5, 15)) = 1
		_CustomProjection ("Projection Paramters", Vector) = (0, 0, 0, 0)
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
			#include "D3D_OpenGL.cginc"

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
			
			float _Saturate;
			float _Invert;
			float4 _CustomProjection;
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed depthVal = tex2D(_MainTex, i.uv).r;
				depthVal = LinearDepth(_CustomProjection, depthVal);

				fixed4 col = fixed4(depthVal.rrr, 1);
				col *= _Saturate;
				col = col * 2 - 1;
				if(_Invert) {
					return 1 - col;
				}
				return col;
 			}
			ENDCG
		}
	}
}
