Shader "URP/SimpleWater"
{
    Properties
    {
        _BaseMap("Water Texture", 2D) = "white" {}
        _NormalMap("Normal Map", 2D) = "bump" {}
        _NormalStrength("Normal Strength", Range(0, 1)) = 1
        _Color("Color Tint", Color) = (0.3, 0.5, 0.7, 1)
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
            Tags { "LightMode" = "UniversalForward" "Queue" = "Geometry" }

            Blend SrcAlpha OneMinusSrcAlpha
            Cull Back

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #pragma multi_compile_fog

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float2 uv : TEXCOORD0;
                float4 tangentOS : TANGENT;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 viewDirWS : TEXCOORD1;
                float3x3 tangentToWorld : TEXCOORD2;
                float fogFactor : TEXCOORD5;
            };

            sampler2D _BaseMap;
            sampler2D _NormalMap;
            float _NormalStrength;
            float4 _BaseMap_ST;
            float4 _NormalMap_ST;
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
                float3 tangentWS = TransformObjectToWorldDir(input.tangentOS.xyz);
                float3 bitangentWS = cross(normalWS, tangentWS) * input.tangentOS.w;

                output.tangentToWorld = float3x3(tangentWS, bitangentWS, normalWS);

                output.viewDirWS = GetCameraPositionWS() - worldPos;
                float4 positionHCS = TransformWorldToHClip(worldPos);
                output.positionHCS = positionHCS;
                output.uv = TRANSFORM_TEX(input.uv, _BaseMap);

                output.fogFactor = ComputeFogFactor(positionHCS.z);
                return output;
            }

            half4 frag(Varyings i) : SV_Target
            {
                // Sample and unpack normal map
                float3 normalTS = UnpackNormal(tex2D(_NormalMap, i.uv) * _NormalStrength);
                float3 normalWS = normalize(mul(i.tangentToWorld, normalTS));

                float3 viewDir = normalize(i.viewDirWS);

                float4 texColor = tex2D(_BaseMap, i.uv);
                texColor.rgb *= _Color.rgb;

                // Lighting
                Light light = GetMainLight();
                float3 lightDir = normalize(light.direction);
                float NdotL = max(0.1, dot(normalWS, -lightDir));
                float3 diffuse = texColor.rgb * NdotL * light.color;

                // Specular
                float3 reflection = reflect(-viewDir, normalWS);
                float specPower = lerp(4.0, 128.0, _Smoothness);
                float spec = pow(max(dot(lightDir, reflection), 0.0), specPower);
                spec *= _Smoothness;

                float3 finalColor = diffuse + spec * light.color.rgb;

                // Apply fog
                finalColor = MixFog(finalColor, i.fogFactor);

                return float4(finalColor, _Color.a);
            }
            ENDHLSL
        }
    }

    FallBack "Hidden/Universal Render Pipeline/FallbackError"
}
