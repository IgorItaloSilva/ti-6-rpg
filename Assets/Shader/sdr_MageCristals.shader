Shader "Custom/sdr_MageCristals"
{
    Properties
    {
        _Color0 ("CristalColor0", Color) = (1,1,1,1)
        _Color1 ("EmissionColor", Color) = (1,1,1,1)
        _Color2 ("GroundColor", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Slider("Slider", Range(0, 1)) = 0
        _SpecularColor("Specular Color", Color) = (1,1,1,1)
        _Glossiness("Glossiness", Range(8, 128)) = 32
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry" "RenderPipeline"="UniversalPipeline"}
        Pass {
            Tags {"LightMode"="UniversalForward"}
            HLSLPROGRAM
            
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/UnityInput.hlsl"

            struct appdata {
                float4 vertex: POSITION;
                float4 color: COLOR;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float4 vertex : SV_POSITION;
                float4 color: COLOR;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _Color0, _Color1, _Color2;
            float _Slider;
            float4 _SpecularColor;
            float _Glossiness;

            v2f vert (appdata v){
                v2f o;
                o.vertex = TransformObjectToHClip(v.vertex);
                o.color = v.color;
                o.normal = mul(unity_ObjectToWorld, v.normal);
                return o;
            }

            float4 frag (v2f i): SV_Target {
                float2 uv = i.normal.y;
                uv *= (_Time.x * 2);
                float4 tex = tex2D(_MainTex, uv);
                float4 c2 = lerp(_Color0, _Color1, tex);
                float4 c3 = lerp(_Color2, c2, i.color);
                o.Albedo = c2;
                i.color = c3;
                return i.color.r;
                
            }
            ENDHLSL
        }
    }
}
