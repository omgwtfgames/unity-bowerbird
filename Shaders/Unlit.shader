Shader "Unlit" {
	Properties {
		_MainTex ("Texture", 2D) = "white" {}
		_Color ("Main Color", Color) = (1,1,1,1)
	}
	SubShader {
		Cull Off ZWrite On
		Fog {Mode Off}
		Pass {
			SetTexture[_MainTex] {
				constantColor [_Color]
				Combine texture * constant, texture * constant
			}
		}
	}
}