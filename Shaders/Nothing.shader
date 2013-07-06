// http://forum.unity3d.com/threads/148950-Cheapest-invisible-shader
// Useful if you need and active but invisible mesh (eg filling gaps for NavMesh generation)
Shader "Nothing" {
     
    Subshader {Pass {   
        GLSLPROGRAM
        #ifdef VERTEX
        void main() {}
        #endif
       
        #ifdef FRAGMENT
        void main() {}
        #endif
        ENDGLSL
    }}
     
    Subshader {Pass {
        CGPROGRAM
        #pragma vertex vert
        #pragma fragment frag
        struct v2f {
            fixed4 position : SV_POSITION;
        };
       
        v2f vert() {
            v2f o;
            o.position = fixed4(0,0,0,0);
            return o;
        }
       
        fixed4 frag() : COLOR {
            return fixed4(0,0,0,0);
        }
        ENDCG
    }}
}