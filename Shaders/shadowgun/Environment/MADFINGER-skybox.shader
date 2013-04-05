Shader "MADFINGER/Environment/Skybox - opaque - no fog" {

Properties {
	_MainTex ("Base (RGB)", 2D) = "white" {}
	_Color ("HACK: temporary to fix lightmap bouncing light (will be fixed in RC1)", Color) = (1,1,1,1)
}

SubShader {
	Tags {"Queue"="Geometry+10" "IgnoreProjector"="True" "RenderType"="Opaque"}
	LOD 100
	
	ZWrite Off
	
	// Non-lightmapped
	Pass {
		Tags { "LightMode" = "Vertex" }
		Lighting Off
		Fog {Mode Off}
		SetTexture [_MainTex] { combine texture } 
	}
	
	

}
}
