Shader "Unlit/Wave"
{
    Properties
    {
        _PatternNum("Pattern Quantity",Int) = 5
        _WaveColorTop("Wave Color Top",Color) = (1,1,1,1)
        _WaveColorBottom("Wave Color Bottom",Color) = (1,1,1,1)
    }
        SubShader
    {
        Tags
        {
            "RenderType" = "Transparent"
            "Queue" = "Transparent"
        }


        Pass
        {
            //src*A +/- dst*B
            //src is the color coming out of fragment shader
            //dst is the background that you are rendering into
            Blend One One //additive
            //Blend DstColor Zero //multiply

            ZWrite Off
            Lighting Off
            Cull Off


            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag            

            #include "UnityCG.cginc"
            #define TAU 6.28

            struct VertexInput
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal :NORMAL;
            };

            struct VertexOutput
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 normal :TEXCOORD1;
            };


            int _PatternNum;
            float3 _WaveColorTop;
            float3 _WaveColorBottom;
            VertexOutput vert(VertexInput i)
            {
                VertexOutput o;
                o.vertex = UnityObjectToClipPos(i.vertex);
                o.uv = i.uv;
                o.normal = i.normal;
                return o;
            }

            fixed4 frag(VertexOutput o) : SV_Target
            {
                float2 uv = o.uv;
                float xoffset = sin(uv.x * TAU * _PatternNum) * 0.5;
                float pattern = sin(uv.y * TAU * _PatternNum + xoffset - _Time.y * 4) * 0.5 + 0.5;
                pattern *= 1 - uv.y;//fade along y
                float3 waveColor = lerp(_WaveColorTop, _WaveColorBottom, 1 - uv.y);
                float3 wave = pattern * waveColor * (abs(o.normal.y) < 0.99);


                return float4(wave,1);
            }
            ENDCG
        }
    }
}
