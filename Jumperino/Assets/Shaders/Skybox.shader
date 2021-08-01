Shader "HummeL/Skybox"
{
    Properties
    {
        _TopColor("Top Color", Color) = (1, 1, 1, 1)
        _BottomColor("Bottom Color", Color) = (1, 1, 1, 1)
        _BlendHeight("Blend height", Range(0, 1)) = 0.5
    }

        SubShader
    {
        Tags {"Queue" = "Background"  "IgnoreProjector" = "True"}
        LOD 100

        ZWrite On
        Pass
        {
            CGPROGRAM
            #pragma vertex vert  
            #pragma fragment frag
            #include "UnityCG.cginc"

            fixed4 _TopColor;
            fixed4 _BottomColor;
            float  _BlendHeight;
            float2 res;

            struct v2f {
                float4 pos : SV_POSITION;
                float4 texcoord : TEXCOORD0;
             };

            v2f vert(appdata_full v) {
               v2f o;
               o.pos = UnityObjectToClipPos(v.vertex);
               o.texcoord = v.texcoord;
               return o;
            }

            fixed4 frag(v2f i) : COLOR{
                fixed4 c = lerp(_TopColor, _BottomColor, (i.pos.y / _ScreenParams.y) + (0.5 - _BlendHeight));
                c.a = 1;
                return c;
            }
         ENDCG
        }
    }
}