Shader "Custom/Highlight" {
Properties {
	_Color ("Color", Color) = (1,1,1,1)
    _MainTex ("Texture", 2D) = "white" { }
    _Bounds ("Bounds", Vector) = (0,0,.5,.5)
}
SubShader {
	Tags{"Queue"="Transparent"}
	Blend SrcAlpha OneMinusSrcAlpha
    Pass {


CGPROGRAM
#pragma vertex vert
#pragma fragment frag

#include "UnityCG.cginc"

sampler2D _MainTex;
float4 _Bounds;
float4 _Color;

struct v2f {
    float4  pos : SV_POSITION;
    float2  uv : TEXCOORD0;
};

float4 _MainTex_ST;

v2f vert (appdata_base v)
{
    v2f o;
    o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
    o.uv = TRANSFORM_TEX (v.texcoord, _MainTex);
    return o;
}

half4 frag (v2f i) : COLOR
{
    half4 c = tex2D(_MainTex, i.uv) * _Color;
    
    i.uv = i.uv - floor(i.uv);
				
	if(i.uv.x > _Bounds[0] &&
		i.uv.x < _Bounds[2] &&
		i.uv.y > _Bounds[1] &&
		i.uv.y < _Bounds[3]){
		c.a = _Color.a;
		//c.a = c.a;
	}
	else{
		discard;
	}
	return c;
}
ENDCG

    }
}
Fallback "VertexLit"
} 