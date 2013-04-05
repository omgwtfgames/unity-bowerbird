// - Unlit
// - Per-vertex (virtual) camera space specular light
// - SUPPORTS lightmap

Shader "MADFINGER/Environment/Virtual Gloss Per-Vertex Additive (Supports Lightmap)" {
Properties {
	_MainTex ("Base (RGB) Gloss (A)", 2D) = "white" {}
	//_MainTexMipBias ("Base Sharpness", Range (-10, 10)) = 0.0
	_SpecOffset ("Specular Offset from Camera", Vector) = (1, 10, 2, 0)
	_SpecRange ("Specular Range", Float) = 20
	_SpecColor ("Specular Color", Color) = (0.5, 0.5, 0.5, 1)
	_Shininess ("Shininess", Range (0.01, 1)) = 0.078125
	_ScrollingSpeed("Scrolling speed", Vector) = (0,0,0,0)
}

SubShader {
	Tags { "RenderType"="Opaque" "LightMode"="ForwardBase"}
	LOD 100
	
	
	
	CGINCLUDE
	#include "UnityCG.cginc"
	sampler2D _MainTex;
	float4 _MainTex_ST;
	samplerCUBE _ReflTex;
	
	#ifndef LIGHTMAP_OFF
	float4 unity_LightmapST;
	sampler2D unity_Lightmap;
	#endif

	//float _MainTexMipBias;
	float3 _SpecOffset;
	float _SpecRange;
	float3 _SpecColor;
	float _Shininess;
	float4 _ScrollingSpeed;
	
	struct v2f {
		float4 pos : SV_POSITION;
		float2 uv : TEXCOORD0;
		#ifndef LIGHTMAP_OFF
		float2 lmap : TEXCOORD1;
		#endif
		fixed3 spec : TEXCOORD2;
	};

	
	v2f vert (appdata_full v)
	{
		v2f o;
		o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
		
		o.uv = v.texcoord + frac(_ScrollingSpeed * _Time.y);
		
		float3 viewNormal = mul((float3x3)UNITY_MATRIX_MV, v.normal);
		float4 viewPos = mul(UNITY_MATRIX_MV, v.vertex);
		float3 viewDir = float3(0,0,1);
		float3 viewLightPos = _SpecOffset * float3(1,1,-1);
		
		float3 dirToLight = viewPos.xyz - viewLightPos;
		
		float3 h = (viewDir + normalize(-dirToLight)) * 0.5;
		float atten = 1.0 - saturate(length(dirToLight) / _SpecRange);

		o.spec = _SpecColor * pow(saturate(dot(viewNormal, normalize(h))), _Shininess * 128) * 2 * atten;
		
		#ifndef LIGHTMAP_OFF
		o.lmap = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
		#endif
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
			fixed4 c = tex2D (_MainTex, i.uv);
			
			fixed3 spec = i.spec.rgb * c.a;
			
			#if 1
			c.rgb += spec;
			#else			
			c.rgb = c.rgb + spec - c.rgb * spec;
			#endif
			
			#ifndef LIGHTMAP_OFF
			fixed3 lm = DecodeLightmap (tex2D(unity_Lightmap, i.lmap));
			c.rgb *= lm;
			#endif
			
			return c;
		}
		ENDCG 
	}	
}
}


