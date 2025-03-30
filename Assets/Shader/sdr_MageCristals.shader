Shader "Custom/sdr_MageCristals"
{
    Properties
    {
        _Color0 ("CristalColor0", Color) = (1,1,1,1)
        _Color1 ("CristalColor1", Color) = (1,1,1,1)
        _Color2 ("GroundColor", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Slider("Slider", Range(0, 1)) = 0
        //_SpecularColor("Specular Color", Color) = (1,1,1,1)
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

            struct appdata {
                float4 vertex: POSITION;
                float4 color: COLOR; 
                float3 normal : NORMAL; 
            };

            struct v2f {
                float4 vertex : SV_POSITION;
                float4 color: COLOR; 
                float3 worldNormal : TEXCOORD1; 
                float3 worldPos : TEXCOORD2;
            };

            sampler2D _MainTex;
            float4 _Color0, _Color1, _Color2;
            float _Slider;
            float4 _SpecularColor;
            float _Glossiness;

            v2f vert (appdata v){
                v2f o;
                o.vertex = TransformObjectToHClip(v.vertex);
                o.worldPos = TransformObjectToWorld(v.vertex).xyz;
                o.color = v.color;
                o.worldNormal = normalize(TransformObjectToWorldNormal(v.normal));
                return o;
            }

            float4 frag (v2f i): SV_Target{
                float4 tex = tex2D (_MainTex, i.worldNormal);

                float3 lightDir = normalize(_MainLightPosition.xyz); // Luz principal
                float3 normal = normalize(i.worldNormal);
                float3 viewDir = normalize(_WorldSpaceCameraPos.xyz - i.worldPos); // Direção do observador
                // Iluminação difusa
                float diff = max(dot(normal, lightDir), 0.0);

                // Cor base interpolada
                float4 finalColor =  lerp(lerp(_Color0, _Color1, _Time.x), _Color2, 1-i.color) * diff;
                
                return float4(finalColor.rgb, 1.0);
            }
            ENDHLSL
        }
    }
}
