// - Unlit
// - Per-vertex gloss

Shader "MADFINGER/Environment/Lightprobes with VirtualGloss Per-Vertex Additive" {
Properties {
	_MainTex ("Base (RGB) Gloss (A)", 2D) = "white" {}
	_SpecOffset ("Specular Offset from Camera", Vector) = (1, 10, 2, 0)
	_SpecRange ("Specular Range", Float) = 20
	_SpecColor ("Specular Color", Color) = (1, 1, 1, 1)
	_Shininess ("Shininess", Range (0.01, 1)) = 0.078125	
	_SHLightingScale("LightProbe influence scale",float) = 1
}

SubShader {
	Tags { "RenderType"="Opaque" "LightMode"="ForwardBase"}
	LOD 100
	
	
	
	CGINCLUDE
	#pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON
	#include "UnityCG.cginc"
	sampler2D _MainTex;
	float4 _MainTex_ST;
	
	
	float3 _SpecOffset;
	float _SpecRange;
	float3 _SpecColor;
	float _Shininess;
	float _SHLightingScale;
	
	struct v2f {
		float4 pos : SV_POSITION;
		float2 uv : TEXCOORD0;
		float3 refl : TEXCOORD1;
		fixed3 spec : TEXCOORD3;
		fixed3 SHLighting: TEXCOORD4;
	};

	
	v2f vert (appdata_full v)
	{
		v2f o;
		o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
		o.uv = v.texcoord;
		
		float3 worldNormal = mul((float3x3)_Object2World, v.normal);		
		float3 viewNormal = mul((float3x3)UNITY_MATRIX_MV, v.normal);
		float4 viewPos = mul(UNITY_MATRIX_MV, v.vertex);
		float3 viewDir = float3(0,0,1);
		float3 viewLightPos = _SpecOffset * float3(1,1,-1);
		
		float3 dirToLight = viewPos.xyz - viewLightPos;
		
		float3 h = (viewDir + normalize(-dirToLight)) * 0.5;
		float atten = 1.0 - saturate(length(dirToLight) / _SpecRange);

		o.spec = _SpecColor * pow(saturate(dot(viewNormal, normalize(h))), _Shininess * 128) * 2 * atten;
		
		o.SHLighting	= ShadeSH9(float4(worldNormal,1)) * _SHLightingScale;
		
		return o;
	}
	ENDCG


	Pass {
		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		#pragma fragmentoption ARB_precision_hint_fastest		
		fixed4 frag (v2f i) : COLOR
		{
			fixed4 c	= tex2D (_MainTex, i.uv);

			c.rgb *= i.SHLighting;
			c.rgb += i.spec.rgb * c.a;
			
			return c;
		}
		ENDCG 
	}	
}
}

