Shader "Chroma Key/Solid Simple Distance"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_TargetColor ("Target Color", Color) = (0, 1, 0, 1)
		_SpillRemoval ("Spill Removal", Range(0, 2)) = 0.2
		_Tolerance ("Tolerance", Range(0, 5)) = 0.2
		_Threshold ("Threshold", Range(0, 5)) = 0.2
		_ColorTimer ("Color Rotation Timer", Range(0, 1.2)) = 1
		[Toggle] _Debug("Debug Out", Float) = 0
		[Toggle] _Black("Color Background", Float) = 0
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
			#include "Generators.cginc"

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
			float _SpillRemoval;
			int _AlphaBlur;
			int _Debug;
			int _Black;

			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
                col.a = greenFilter(col, _TargetColor);
				if(_Debug) {
					return col.aaaa;
				}
				col = fixed4(spillRemoval(col.rgb, _TargetColor.rgb, _SpillRemoval), col.a);

				fixed4 tCol = genColorWheel(pow(_Time.y, _ColorTimer), i.uv);
				tCol *= _ColorTimer * _Black;
				return col * col.a + tCol * (1 - col.a);
				return col;
			}
			ENDCG
		}
	}
}
