Shader "Custom/URPWater"
{
    Properties
    {
        _Color ("Color", Color) = (0.2, 0.4, 0.6, 0.6)
        _MainTex ("Water Texture", 2D) = "white" {}
        _NoiseTex ("Noise", 2D) = "white" {}
        _WaveStrength ("Wave Height", Range(0, 1)) = 0.1
        _Distortion ("Refraction Distortion", Range(0, 0.2)) = 0.0
        _SpecularStrength ("Specular Strength", Range(0,1)) = 0.5
        _TessellationFactor ("Tessellation Factor", Range(1,64)) = 16
    }
    SubShader
    {
        Tags
        {
            "RenderType"="Transparent" "Queue"="Transparent"
        }
        LOD 200
        Pass
        {
            Name "ForwardLit"
            Tags
            {
                "LightMode"="UniversalForward"
            }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

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
                float4 screenPos : TEXCOORD3;
            };

            CBUFFER_START(UnityPerMaterial)
                float4 _Color;
                float _WaveStrength;
                float _Distortion;
                float _SpecularStrength;
                float _TessellationFactor;
            CBUFFER_END

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            TEXTURE2D(_NoiseTex);
            SAMPLER(sampler_NoiseTex);

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                float2 uv = IN.uv;
                float time = _Time.y;
                float noise = SAMPLE_TEXTURE2D_LOD(_NoiseTex, sampler_NoiseTex, uv + float2(time * 0.2, time * 0.15),
                                                   0).r;
                float wave = sin(uv.x * 10 + time) * 0.5 + sin(uv.y * 15 + time * 0.8) * 0.5;
                float height = (noise + wave) * 0.5 * _WaveStrength;
                float3 posWS = TransformObjectToWorld(IN.positionOS.xyz + IN.normalOS * height);
                OUT.positionHCS = TransformWorldToHClip(posWS);
                OUT.uv = uv;
                OUT.normalWS = TransformObjectToWorldNormal(IN.normalOS);
                OUT.viewDirWS = GetWorldSpaceViewDir(posWS);
                OUT.screenPos = ComputeScreenPos(OUT.positionHCS);
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                float2 movingUV = IN.uv + float2(_Time.y * 0.05, _Time.y * 0.02);
                half4 waterTex = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, movingUV) * _Color;
                float2 noiseUV = IN.uv * 1.5;
                float2 distortion = SAMPLE_TEXTURE2D(_NoiseTex, sampler_NoiseTex, noiseUV).rg - 0.5;
                float2 distortionUV = IN.screenPos.xy / IN.screenPos.w + distortion * _Distortion;

                // Refraction: use _CameraOpaqueTexture if enabled in URP
                #ifdef UNITY_DECLARE_SCREENSPACE_TEXTURE
                        half4 refracted = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_CameraOpaqueTexture, distortionUV);
                #else
                half4 refracted = half4(0, 0, 0, 1);
                #endif

                half3 baseColor = lerp(refracted.rgb, waterTex.rgb, 0.5) * 0.4;
                half alpha = _Color.a;

                // Simple specular
                half3 normal = normalize(IN.normalWS);
                half3 viewDir = normalize(IN.viewDirWS);
                half3 lightDir = normalize(_MainLightPosition.xyz);
                half NdotL = saturate(dot(normal, lightDir));
                half3 halfDir = normalize(lightDir + viewDir);
                half NdotH = saturate(dot(normal, halfDir));
                half spec = pow(NdotH, 32) * _SpecularStrength;

                baseColor += spec * _MainLightColor.rgb * NdotL;

                return half4(baseColor, alpha);
            }
            ENDHLSL
        }
    }
    FallBack "Universal Render Pipeline/Unlit"
}