// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

Shader "UI/ColorPicker"
{
    Properties
    {
        [PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
        _Color("Tint", Color) = (1,1,1,1)

    }

        SubShader
        {
            Tags
            {
                "Queue" = "Geometry"
               // "IgnoreProjector" = "True"
              //  "RenderType" = "Transparent"
              //  "PreviewType" = "Plane"
              //  "CanUseSpriteAtlas" = "True"
            }
           

          //  Cull Off
            Lighting Off
            ZWrite Off
          //  ZTest[unity_GUIZTestMode]
          //  Blend SrcAlpha OneMinusSrcAlpha
          //  ColorMask[_ColorMask]

            Pass
            {
                Name "Default"
            CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag

                #include "UnityCG.cginc"
                #include "UnityUI.cginc"

                struct VertexInput
                {
                    float4 vertex   : POSITION;
                    float4 color    : COLOR;
                    float2 uv : TEXCOORD0;
             
                };

                struct VertexOutput
                {
                    float4 vertex   : SV_POSITION;
                    fixed4 color : COLOR;
                    float2 uv  : TEXCOORD0;
                    float4 worldPosition : TEXCOORD1;
               
                };

                sampler2D _MainTex;
                fixed4 _Color;
                fixed4 _TextureSampleAdd;
                float4 _ClipRect;
                float4 _MainTex_ST;



                VertexOutput vert(VertexInput v)
                {
                    VertexOutput OUT;
                    OUT.worldPosition = v.vertex;
                    OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);

                    OUT.uv = TRANSFORM_TEX(v.uv, _MainTex);
                   
                    OUT.color = v.color * _Color;
                    return OUT;
                }

                //rgb2hsv and hsv2rgb are from https://stackoverflow.com/questions/15095909/from-rgb-to-hsv-in-opengl-glsl
                // All components are in the range [0…1], including hue.
                float3 hsv2rgb(float3 c)
                {
                    float4 K = float4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
                    float3 p = abs(frac(c.xxx + K.xyz) * 6.0 - K.www);
                    return c.z * lerp(K.xxx, clamp(p - K.xxx, 0.0, 1.0), c.y);
                }
                // All components are in the range [0…1], including hue.
                float3 rgb2hsv(float3 c)
                {
                    float4 K = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
                    float4 p = lerp(float4(c.bg, K.wz), float4(c.gb, K.xy), step(c.b, c.g));
                    float4 q = lerp(float4(p.xyw, c.r), float4(c.r, p.yzx), step(p.x, c.r));

                    float d = q.x - min(q.w, q.y);
                    float e = 1.0e-10;
                    return float3(abs(q.z + (q.w - q.y) / (6.0 * d + e)), d / (q.x + e), q.x);
                }

                //hue
                float3 hue(VertexOutput IN) {
                    float r = max(0,min(0.5, (1 - abs(IN.uv.y * 3 - 3))) * 2)+ max(0,min(0.5, (1 - abs(IN.uv.y * 3))) * 2);//
                    float g =max(0,min(0.5, (1-abs(IN.uv.y * 3-1)))*2);
                    float b = max(0, min(0.5, (1 - abs(IN.uv.y * 3 - 2))) * 2);;// (1 - max(0.5, abs(IN.uv.y * 3 - 2))) * 2;
                    return float3(r, g, b);
                }
                //sv
                float3 saturation_value_squre(VertexOutput IN, float hue) {
                    
                    float s = IN.uv.x;
                    float v = IN.uv.y;
                    float3 color = hsv2rgb(float3(hue, s, v));
                    return color;
                }
                //selecting hue
                float3 selector_hue(VertexOutput IN,float2 pos) {
                    float radius = 0.02;
                    float circle = distance(IN.uv, pos) < radius;
                    return circle;
                }

                //selecting sv
                float3 selector_sv(VertexOutput IN, float2 pos) {
                    float radiusInner = 0.02;
                    float radiusOuter = 0.03;
                    float ring = (distance(IN.uv, pos) > radiusInner) && (distance(IN.uv, pos) < radiusOuter);
                    return ring;
                }
                
                fixed4 frag(VertexOutput IN) : SV_Target
                {
                   

                    float3 h = hue(IN);
                    float3 sv = saturation_value_squre(IN,float3(1,1,1));

                    float right = step( 0.9, IN.uv.x);
                    float left = step(IN.uv.x, 0.8);
                    

                    float selectorHue = selector_hue( IN, float2(0.87,0.5));
                    float selectorSV = selector_sv(IN, float2(0.5, 0.5));
                    float4 color = right * float4(h, 1) + left*float4(sv,1) + selectorHue + selectorSV;

                    return color;
                }
            ENDCG
            }
        }
}