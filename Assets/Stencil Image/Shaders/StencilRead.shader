Shader "Custom/Stencil Read" {
	Properties 
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_ImageTex ("Image Tex", 2D) = "white" {}
	}
	SubShader 
	{
		Stencil 
		{
			Ref 2
			ReadMask 2
			Comp Equal
		}
		
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			sampler2D _MainTex;
			float4 _MainTex_ST;

			struct appdata {
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f {
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert (appdata v) {
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}

			sampler2D _ImageTex;

			fixed4 frag (v2f i) : SV_Target {
				return tex2D(_ImageTex, i.uv);
			}

			ENDCG
		}
	} 
	FallBack "Diffuse"
}
