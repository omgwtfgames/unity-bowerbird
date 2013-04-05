Shader "FX/MobileDiamond"
{
	// http://wiki.unity3d.com/index.php?title=IPhoneGems 
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_Fog("Fog", Color) = (0,0,0,0)
		_ReflectTex ("Reflection Texture", Cube) = "dummy.jpg" {
			TexGen CubeReflect
		}
		_RefractTex ("Refraction Texture", Cube) = "dummy.jpg" {
			TexGen CubeReflect
		}
		_RefractTexlow ("Refraction LowGPU", 2D) = "dummy.jpg" {
			TexGen SphereMap
		}
		_ReflectTexlow ("Reflect LowGPU", 2D) = "dummy.jpg" {
			TexGen SphereMap
		}

		_Shininess ("Shininess", Range (0.01, 1)) = 0.7
        _SpecColor ("Specular", Color) = (1,1,1,1)
        _Emission ("Emissive", Color) = (1,1,1,1)
	}	

	SubShader {
		Tags {
			"Queue" = "Transparent"
		}
		// First pass - here we render the backfaces of the diamonds. Since those diamonds are more-or-less
		// convex objects, this is effectively rendering the inside of them
		Pass {
			Color (0,0,0,0)
			Offset  -1, -1
			Cull Front
			ZWrite Off
			SetTexture [_RefractTex] {
				constantColor [_Color]
				combine texture * constant, primary
			}
			SetTexture [_ReflectTex] {
				combine previous, previous +- texture
			}
		}

		// Second pass - here we render the front faces of the diamonds.
		Pass {
			Fog { Color (0,0,0,0)}
			ZWrite on
			Blend One One
			SetTexture [_RefractTex] {
				constantColor [_Color]
				combine texture * constant
			}
			SetTexture [_ReflectTex] {
				combine texture + previous, previous +- texture
			}
		}
	}

	// Older cards. Here we remove the bright specular highlight
	SubShader {
		Tags{"Queue" = "Transparent"}
		// First pass - here we render the backfaces of the diamonds. Since those diamonds are more-or-less
		// convex objects, this is effectively rendering the inside of them
		Pass {
			Color (0,0,0,0)
			Cull Front
			SetTexture [_RefractTex] {
				constantColor [_Color]
				combine texture * constant, primary
			}
		}

		// Second pass - here we render the front faces of the diamonds.
         
		Pass {
			Fog { Color (0,0,0,0)}
			ZWrite on
			Blend DstColor Zero  
			SetTexture [_RefractTex] {
				constantColor [_Color]
				combine texture * constant
			}
		}
	}

/////////// Start iphone code////////////////
//This will cause a nice gem texture to be rendered using the low-GPU textures defined in the inspector//
//This section of the code is provided by BURNING THUMB SOFTWARE, 2010//

	SubShader {
				Pass {
					
		Lighting On
            SeparateSpecular On
					
			Color (0,0,0,0)
		//	Offset  -1, -1
			Cull Front
			//Blend OneMinusSrcAlpha One
			SetTexture [_ReflectTexlow] {
				constantColor [_Color]
				combine texture * constant, primary
			}
		}

		// Second pass - here we render the front faces of the diamonds.
		Pass {
		

			Fog { Color [_Fog]}
			ZWrite on
			Blend One One
			SetTexture [_RefractTexlow] {
				constantColor [_Emission]
				combine texture * constant
			}
		}
	           
	}
}