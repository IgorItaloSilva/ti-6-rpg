Shader "Custom/Cristals"
{
    Properties
    {
        _Color ("ground Color", Color) = (1,1,1,1)
        _Tiling ("Tiling", Vector) = (1, 1, 0, 1)
        _MainTex ("Ground Texture", 2D) = "white" {}

        [HDR]_Color0 ("Cristal Color", Color) = (1, 1, 1, 1)
        [HDR]_Color1 ("Emission Color", Color) = (1, 1, 1, 1)
        _EmitTex ("Textura Emissão", 2D) = "white" {}
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            // Variáveis definidas no Properties
            sampler2D _MainTex, _CristalMask, _EmitTex;
            float4 _Color, _Color0, _Color1, _Tiling;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 color : COLOR;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.color = v.color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv * sin(_Time.y);
                fixed4 groundTex = tex2D(_MainTex, i.uv);
                fixed4 emitMask = tex2D(_EmitTex, uv);
                float4 cristal = lerp(_Color0, _Color1, emitMask);
                
                //return lerp(groundTex * _Color, cristal, i.color);
                fixed3 lightDir = normalize(_WorldSpaceLightPos0.xyz);
                fixed3 normal = float3(0, 0, 1); // usa um normal fake ou calculado
                fixed NdotL = saturate(dot(normal, lightDir));
                fixed3 diffuse = groundTex.rgb * NdotL;

                float metallic = i.color.r;
                float3 finalColor = lerp(diffuse, cristal.rgb, i.color);
                return float4(finalColor, 1);
            }
            ENDCG
        }
    }

    FallBack "Diffuse"
}