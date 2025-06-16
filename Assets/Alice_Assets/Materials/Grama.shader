// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/GrassSimpleSwayTinted"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color Tint", Color) = (1,1,1,1)
        _WaveSpeed ("Wave Speed", Float) = 1
        _WaveStrength ("Wave Strength", Float) = 0.02
        _WaveScale ("Wave Scale", Float) = 5
        _HeightMin ("Base Y", Float) = 0
        _HeightMax ("Top Y", Float) = 1
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100

        Pass
        {
            Cull Off
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color;
            float _WaveSpeed;
            float _WaveStrength;
            float _WaveScale;
            float _HeightMin;
            float _HeightMax;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)          // Add fog coordinate
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;

                // Compute world position for fog calculation
                float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;

                // Calculate fog factor
                UNITY_TRANSFER_FOG(o, UnityObjectToClipPos(v.vertex));

                // Standard vertex transform to clip space
                o.vertex = UnityObjectToClipPos(v.vertex);

                float y = v.vertex.y;
                float heightFactor = saturate((y - _HeightMin) / (_HeightMax - _HeightMin));
                float offset = sin(_Time.y * _WaveSpeed + y * _WaveScale) * _WaveStrength * heightFactor;

                o.uv = v.uv;
                o.uv.x += offset;

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv) * _Color;

                // Apply fog blending
                UNITY_APPLY_FOG(i.fogCoord, col);

                return col;
            }
            ENDCG
        }
    }
}
