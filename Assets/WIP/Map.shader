Shader "Custom/Terrain"
{
    Properties
    {
        _Color ("Gress Color", Color) = (1,1,1,1)
        _Tiling ("Tiling", Vector) = (1, 1, 0, 1)
        _MainTex ("Grass Texture", 2D) = "white" {}
        _Color0 ("Dirt Color", Color) = (1,1,1,1)
        _MainTex0 ("Dirt", 2D) = "white" {}
        _PathMask ("PathMask", 2D) = "white" {}
    }

    SubShader
    {
        Pass
        {
            Tags {"LightMode" = "ForwardBase"}
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #include "Lighting.cginc"

            // Variáveis definidas no Properties
            sampler2D _MainTex, _MainTex0, _PathMask;
            float4 _Color, _Color0, _Tiling;

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float3 worldNormal : TEXCOORD1;
                float3 worldPos : TEXCOORD2;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.worldNormal = UnityObjectToClipPos(v.normal);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;
                uv.x *= _Tiling.x;
                uv.y *= _Tiling.y;
                fixed4 grass = tex2D(_MainTex, uv);
                fixed4 dirt = tex2D(_MainTex0, i.uv);
                fixed4 mask = tex2D(_PathMask, i.uv);

                fixed4 col = lerp(grass * _Color, dirt * _Color0, mask);

                // Normalizado
                float3 normal = normalize(i.worldNormal);

                // Direção da luz principal
                float3 lightDir = normalize(_WorldSpaceLightPos0.xyz);

                // Intensidade difusa
                float diff = max(0, dot(normal, lightDir));

                // Luz final
                fixed3 final = col.rgb * _LightColor0.rgb * diff;


                return fixed4(final, col.a);
            }
            ENDCG
        }
    }

    FallBack "Diffuse"
}
