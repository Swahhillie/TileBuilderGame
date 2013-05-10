Shader "Custom/SurfHightlighting" {
	Properties {
		_MainTex ("Base (RGB) Trans(A)", 2D) = "white" {}
		_Bounds ("Bounds", Vector) = (0,0,.5,.5)
		_Color ("Color", Color) = (1,1,1,1)
	}
	SubShader {
		Tags{"Queue"="Transparent"}
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Lambert

		sampler2D _MainTex;
		float4 _Bounds;
		float4 _Color;
		
		struct Input {
			float2 uv_MainTex;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			half4 c = tex2D (_MainTex, IN.uv_MainTex);
			clip((	IN.uv_MainTex.x > _Bounds[0] &&
					IN.uv_MainTex.x < _Bounds[2] &&
					IN.uv_MainTex.y > _Bounds[1] &&
					IN.uv_MainTex.y < _Bounds[3])? 1:-1);
			o.Albedo = c.rgb * _Color.rgb;
			
			
			
			o.Alpha = _Color.a;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
