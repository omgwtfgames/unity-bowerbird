Shader "MADFINGER/Environment/Skybox - transparent - no fog" {

Properties {
	_MainTex ("Base (RGB)", 2D) = "white" {}
	_Color ("HACK: temporary to fix lightmap bouncing light (will be fixed in RC1)", Color) = (1,1,1,1)
}

SubShader {
	Tags {"Queue"="Transparent-20" "IgnoreProjector"="True" "RenderType"="Transparent"}
	LOD 100
	
	ZWrite Off
	Blend SrcAlpha OneMinusSrcAlpha 
	
	// Non-lightmapped
	Pass {
		Tags { "LightMode" = "Vertex" }
		Lighting Off
		Fog {Mode Off}
		SetTexture [_MainTex] { combine texture } 
	}
	
	

}
}
