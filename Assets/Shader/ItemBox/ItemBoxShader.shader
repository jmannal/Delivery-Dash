Shader "Unlit/ItemBoxShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _RampTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" }
        LOD 100

        Pass
        {
            Cull Front

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct vertIn
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct vertOut
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _RampTex;
            float4 _RampTex_ST;
            sampler2D _MainTex;
            float4 _MainTex_ST;

            vertOut vert (vertIn v)
            {
                vertOut o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _RampTex);
                return o;
            }

            fixed4 frag (vertOut i) : SV_Target
            {
                // Ramping colour by deforming UV coordinates
                fixed4 greyCol = tex2D(_MainTex, i.uv);
                float displacement = greyCol[0] + _Time.y;

                float2 uv = i.uv + float2(displacement, 0);
                uv[0] = uv[0] % 1;

                fixed4 col = tex2D(_RampTex, uv);

                return col;
            }



            ENDCG
        }
    }
}
