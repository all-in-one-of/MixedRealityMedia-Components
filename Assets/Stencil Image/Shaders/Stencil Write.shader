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
		ColorMask RGB
		CGPROGRAM
		#pragma surface surf Lambert

		float4 _Color;

		struct Input 
		{
			float2 uv_MainTex;
		};

		void surf (Input IN, inout SurfaceOutput o) 
		{
			o.Albedo = _Color.rgb;
			// o.Alpha = _Color.a;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
