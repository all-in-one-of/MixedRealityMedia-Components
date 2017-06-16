Shader "Custom/Stencil Write" {
	Properties {
		_Color ("Base (RGB)", Color) = (0.2, 0.95, 0.2, 1)
	}
	SubShader 
	{
		Stencil 
		{
	        Ref 2
	        Comp Always
	        Pass Replace
		}
		
		Tags { "RenderType"="Opaque" }
		LOD 200
		Cull off
		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			float4 _Color;

			struct appdata {
				float4 vertex : POSITION;
			};

			struct v2f {
				float4 vertex : SV_POSITION;
			};

			v2f vert (appdata v) {
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				return o;
			}

			sampler2D _ImageTex;

			fixed4 frag (v2f i) : SV_Target {
				// return fixed4(0, 0, 0, 0);
				return fixed4(_Color.rgb, 0);
			}
			ENDCG
		}
	} 
}
