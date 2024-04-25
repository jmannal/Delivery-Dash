

Shader "Custom/miniMapFilter"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _HueShiftTex ("Hue Shift text", 2D) = "white" {}
        _HueShift ("Hue Shift text", Range(0,10)) = 0
        _Saturation ("Saturation", Range(0,5)) = 0
        _Brightness ("Brightness", Range(-1,1))= 0
        _isCrash ("Crash", Range(0,1))= 0
        
        _HeightSaturation ("HeightSat", Range(0,1)) = 1

        _Frequency("Frequency", Range(1,10))=1

        // Stencil components taken from defaultUI in unity shader in order to allow for masking
        _StencilComp ("Stencil Comparison", Float) = 8
		_Stencil ("Stencil ID", Float) = 0
		_StencilOp ("Stencil Operation", Float) = 0
		_StencilWriteMask ("Stencil Write Mask", Float) = 255
		_StencilReadMask ("Stencil Read Mask", Float) = 255
		_ColorMask ("Color Mask", Float) = 15
    }
    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            
            "RenderType"="Transparent"
            
        }
        
        Stencil
		{
			Ref [_Stencil]
			Comp [_StencilComp]
			Pass [_StencilOp] 
			ReadMask [_StencilReadMask]
			WriteMask [_StencilWriteMask]
		}
 		ColorMask [_ColorMask]
        Pass
        {
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

            
            
            float _HueShift;
            float _Saturation;
            float _Brightness;
            float _isCrash;
            float _Frequency;
          
            float _HeightSaturation;
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;

            // Hue shift function based on tutorial from Benjamin Swee https://www.youtube.com/watch?v=kiSKb54cogo
            // Modified to create addtional effects such as changting saturation based on a pixels postion

            float3 HueShift(float3 color, v2f v) {

                float3x3 RGB_YIQ = float3x3 (0.299,0.586,0.114,
                                         0.595,-0.275, -0.3213,
                                         0.2115, -0.5227, 0.3112);
                float3x3 YIQ_RGB = float3x3 (1, 0.956, 0.619,
                                            1,-0.272,0.647,
                                            1, -1.106, 1.702);

             
            
              
                float3 YIQ = mul(RGB_YIQ, color);
                float hue;
                float dist= length(v.uv - fixed2(0.5f,0.5f));
                
                float brit = _Brightness;
                hue = atan2(YIQ.z,YIQ.y) + _HueShift + 2*sin(_Time.y*_Frequency + dist*2);
                
                float sat;
                if (_HeightSaturation > v.uv.y + sin(_Time.y+v.uv.x*30)/40) {
                    sat = _Saturation;
                } else if (_HeightSaturation +0.02 >= v.uv.y + sin(_Time.y+v.uv.x*30)/40) {
                    brit = -1;
                } else {
                    sat = 0;
                }

                float chroma = length(float2(YIQ.y, YIQ.z)) *sat;

                float Y=YIQ.x+ brit;
                float I = chroma * cos(hue);

                float Q= chroma * sin(hue);
                float3 shiftYIQ = float3(Y,I,Q);

                float3 newRGB = mul(YIQ_RGB, shiftYIQ);

                return newRGB;                            

            }

            fixed4 frag (v2f i) : SV_Target
            {
                
               
                fixed4 col = tex2D(_MainTex, i.uv);
              
               
                col.rgb = HueShift(col.rgb,i);
               

                return col;
            }
            ENDCG
        }
    }
}
