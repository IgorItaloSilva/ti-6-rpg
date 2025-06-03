Shader "Unlit/ShearTopOnly"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Slider  ("Slider", Range(-2, 2)) = 1
        _HeightMin("Height Min", Float) = 0
        _HeightMax("Height Max", Float) = 1
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" "RenderPipeline"="UniversalPipeline" }
        LOD 100

        Pass
        {
            Cull Off
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata 
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Slider;
            float _HeightMin;
            float _HeightMax;

            v2f vert (appdata v)
            {
                v2f o;
                
                float localY = v.vertex.y;

                // Normaliza a altura entre 0 e 1
                float normalizedY = saturate((localY - _HeightMin) / (_HeightMax - _HeightMin));

                // Calcula o fator de deslocamento apenas para a parte de cima
                float shearOffset = sin(_Time.y + v.vertex.y * _Slider) * 0.08 * normalizedY;

                float4x4 m = float4x4(
                    1, shearOffset, 0, 0,
                    0, 1, 0, 0,
                    0, 0, 1, 0,
                    0, 0, 0, 1
                );

                o.vertex = UnityObjectToClipPos(mul(m, v.vertex));
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                return col;
            }
            ENDCG
        }
    }
}
