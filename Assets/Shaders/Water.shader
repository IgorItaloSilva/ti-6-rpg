Shader "URP/SimpleWater"
{
    Properties
    {
        _BaseMap("Water Texture", 2D) = "white" {}
        [HDR] _Color("Color Tint", Color) = (0.3, 0.5, 0.7, 1)
        _WaveSpeed("Wave Speed", Float) = 1
        _WaveScale("Wave Scale", Float) = 1
        _WaveStrength("Wave Strength", Float) = 0.05
        _Smoothness("Smoothness", Range(0, 1)) = 0.8
        _Metallic("Metallic", Range(0, 1)) = 0
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 200

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode" = "UniversalForward" }

            Blend SrcAlpha OneMinusSrcAlpha
            Cull Back
            ZWrite Off

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 normalWS : TEXCOORD1;
                float3 viewDirWS : TEXCOORD2;
            };

            sampler2D _BaseMap;
            float4 _BaseMap_ST;
            float4 _Color;
            float _WaveSpeed;
            float _WaveScale;
            float _WaveStrength;
            float _Smoothness;
            float _Metallic;

            Varyings vert(Attributes input)
            {
                Varyings output;

                float wave = sin(_Time.y * _WaveSpeed + input.positionOS.x * _WaveScale + input.positionOS.z * _WaveScale);
                float3 displaced = input.positionOS.xyz;
                displaced.y += wave * _WaveStrength;

                float3 worldPos = TransformObjectToWorld(displaced);
                float3 normalWS = TransformObjectToWorldNormal(input.normalOS);
                output.normalWS = normalize(normalWS);
                output.viewDirWS = GetCameraPositionWS() - worldPos;

                output.positionHCS = TransformWorldToHClip(worldPos);
                output.uv = TRANSFORM_TEX(input.uv, _BaseMap);
                return output;
            }

            half4 frag(Varyings i) : SV_Target
            {
                float3 normal = normalize(i.normalWS);
                float3 viewDir = normalize(i.viewDirWS);

                float4 texColor = tex2D(_BaseMap, i.uv);
                texColor.rgb *= _Color.rgb;

                // Lighting
                Light light = GetMainLight();
                float3 lightDir = normalize(light.direction);
                float NdotL = max(0.1, dot(normal, -lightDir)); // ambient minimum light
                float3 diffuse = texColor.rgb * NdotL * light.color;

                // Specular reflection fix
                float3 reflection = reflect(-viewDir, normal);
                float specPower = lerp(4.0, 128.0, _Smoothness);
                float spec = pow(max(dot(lightDir, reflection), 0.0), specPower);
                spec *= _Smoothness;

                float3 finalColor = diffuse + spec * light.color.rgb;

                return float4(finalColor, _Color.a);
            }
            ENDHLSL
        }
    }

    FallBack "Hidden/Universal Render Pipeline/FallbackError"
}