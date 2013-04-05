Shader "UnlitAlpha" {
	Properties {
		_MainTex ("Texture", 2D) = "white" {}
		_Color ("Main Color", Color) = (1,1,1,1)
	}
	SubShader {
		Cull Off
		Tags { "Queue" = "Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
		Blend SrcAlpha OneMinusSrcAlpha
		Fog {Mode Off}
		Pass {
			SetTexture[_MainTex] {
				constantColor [_Color]
				Combine texture * constant, texture * constant
			}
		}
	}
}