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
               
            }
     
            Lighting Off
            ZWrite Off
          

            Pass
            {
                Name "ColorPicker"
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

                float2 _SVMousePos;
                float2 _HueMousePos;



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
                float3 hue(float2 uv) {
                    float r = max(0,min(0.5, (1 - abs(uv.y * 3 - 3))) * 2)+ max(0,min(0.5, (1 - abs(uv.y * 3))) * 2);//
                    float g =max(0,min(0.5, (1-abs(uv.y * 3-1)))*2);
                    float b = max(0, min(0.5, (1 - abs(uv.y * 3 - 2))) * 2);;// (1 - max(0.5, abs(IN.uv.y * 3 - 2))) * 2;
                    return float3(r, g, b);
                }
                //sv
                float3 saturation_value_squre(float2 uv, float hue) {                    
                    float s = uv.x/0.85;
                    float v = uv.y;
                    float3 color = hsv2rgb(float3(hue, s, v));
                    return color;
                }
                //selecting hue
                float selector_hue(float2 uv,float2 pos) {
                    float radius = 0.02;
                    float circle = distance(uv, pos) < radius;
                    return circle;
                }

                //selecting sv
                float selector_sv(float2 uv, float2 pos) {
                    float radiusInner = 0.02;
                    float radiusOuter = 0.03;
                    float ring = (distance(uv, pos) > radiusInner) && (distance(uv, pos) < radiusOuter);
                    return ring;
                }
                
                fixed4 frag(VertexOutput IN) : SV_Target
                {
                    float2 huemouse =_HueMousePos;
                    float2 svmouse = _SVMousePos;


                    float hueX = step(0.9, IN.uv.x);
                    float svX = step(IN.uv.x, 0.85);

                    float2 hueArea = float2(hueX,IN.uv.y);
                    float2 svArea = float2(svX,IN.uv.y);

                    float3 huebar = hue(IN.uv);

                    float h =  huemouse.y;
                    
                    float selectorHue = selector_hue(IN.uv, float2(0.87, h));
                    
                    float selectorSV = selector_sv(IN.uv, float2(svmouse.x, svmouse.y));
                    

                    float3 sv = saturation_value_squre(IN.uv,h);
                    
                    

                    float4 color = hueArea.x * float4(huebar, 1) + svArea.x * float4(sv, 1) +selectorHue + selectorSV;

                    return color;
                }
            ENDCG
            }
        }
}