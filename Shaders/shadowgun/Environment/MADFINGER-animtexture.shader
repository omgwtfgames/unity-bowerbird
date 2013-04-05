Shader "MADFINGER/FX/Anim texture" {
Properties {
	_MainTex ("Base (RGB)", 2D) = "white" {}
	_NumTexTiles("Num tex tiles",	Vector) = (4,4,0,0)
	_ReplaySpeed("Replay speed - FPS",float) = 4
//	_Randomize("Randomize", float) = 0
	_Color("Color", Color) = (1,1,1,1)
}

SubShader {

	Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
	
	Blend One One
	Cull Off Lighting Off ZWrite Off Fog { Color (0,0,0,0) }

	CGINCLUDE
	#include "UnityCG.cginc"
	sampler2D _MainTex;
	
	float4	_Color;
	float4	_NumTexTiles;
	float		_ReplaySpeed;
	float		_Randomize;
	
	struct v2f {
		float4 pos : SV_POSITION;
		float4 uv : TEXCOORD0;
		fixed4 col: COLOR;
	};


	float2 Rand(float2 ij)
	{
		const float4 a = float4(97.409091034f,54.598150033f,56.205410758f,44.687805943f);
		float4 result  = float4(ij,ij);

		for(int i = 0; i < 2; i++) 
		{
			result.x = frac(dot(result, a));
			result.y = frac(dot(result, a));
			result.z = frac(dot(result, a));
			result.w = frac(dot(result, a));
		}

		return result.xy;
	}

	
	v2f vert (appdata_full v)
	{
		v2f o;
		
		float		time			= (v.color.a * 60 + _Time.y) * _ReplaySpeed;
		float		itime			= floor(time);
		float		ntime		= itime + 1;
		float		ftime			= time - itime;
		
		float2	texTileSize = 1.f / _NumTexTiles.xy;		
		float4	tile;

#if 0
		if (_Randomize > 0)
		{
			itime = floor(Rand(itime) * 1000);
			ntime= floor(Rand(ntime) * 1000);
		}
#endif

		#if 1
		tile.xy = float2(itime,floor(itime /_NumTexTiles.x));
		tile.zw= float2(ntime,floor(ntime /_NumTexTiles.x));
		#endif
		
		
		tile = fmod(tile,_NumTexTiles.xyxy);
		
		o.pos= mul(UNITY_MATRIX_MVP, v.vertex);
		o.uv	= (v.texcoord.xyxy + tile) * texTileSize.xyxy;
		o.col	= float4(_Color.xyz * v.color.xyz,ftime);
		
		
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
			return lerp(tex2D (_MainTex, i.uv.xy),tex2D (_MainTex, i.uv.zw),i.col.a) * i.col;
		}
		ENDCG 
	}	
}
}
