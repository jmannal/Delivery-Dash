// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/GrowShader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
     
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
       
        _Amplitude ("Amplitude", Range(0,10)) = 1.0

    }
    SubShader
    {
        Pass
		{
			Cull Off

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			uniform sampler2D _MainTex;	
            float _Amplitude;
			struct vertIn
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct vertOut
			{
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			// Implementation of the vertex shader
			vertOut vert(vertIn v)
			{
			
				// float4 displacement = float4(0.0f, _Amplitude*sin(_Time.y), 0.0f, 0.0f); 
				// v.vertex += displacement;

				vertOut o;
				o.vertex = UnityObjectToClipPos( v.vertex *(abs(_Amplitude*sin(_Time.y))+1));
				
				o.uv = v.uv;
				return o;
			}
			
			// Implementation of the fragment shader
			fixed4 frag(vertOut v) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, v.uv);
				return col;
			}
			ENDCG
		}
    }
    FallBack "Diffuse"
}
