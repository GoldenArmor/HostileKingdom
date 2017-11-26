// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "YueChuan/FogOfWar/FogOfWarCast"
{
	Properties 
    {
		_FogOfWarColor ("FogOfWar Color", Color) = (0.0,0.0,0.0,1.0)
		_FogOfWarTex ("FogOfWar Tex", 2D) = "black" {}
	  }
    SubShader 
    {
        Tags {"Queue"="Transparent"}
        Pass 
        {
        	ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha
            CGPROGRAM

            #pragma vertex MainVS
            #pragma fragment  MainPS
            #pragma target 2.0
           
            #include "UnityCG.cginc"
            
            uniform fixed4 _FogOfWarColor;
            uniform sampler2D _FogOfWarTex;
            uniform float4x4 _MatCastViewProj;
        
            struct VS_OUTPUT
            {
                float4 Position : SV_POSITION;
                half4 UVFogOfWar : TEXCOORD0;
            };

            VS_OUTPUT MainVS(appdata_base input)
            {
                VS_OUTPUT output = (VS_OUTPUT)0;
                output.Position = UnityObjectToClipPos (input.vertex);
			    float4x4 matWVP = mul (_MatCastViewProj,unity_ObjectToWorld);
			    output.UVFogOfWar = mul (matWVP, input.vertex);
                return output;
            }

            float4 MainPS(VS_OUTPUT input) : COLOR 
            {
                half2 UV = 0.5 * input.UVFogOfWar.xy/input.UVFogOfWar.w + 0.5;
                #if !UNITY_UV_STARTS_AT_TOP
                UV.y = 1 - UV.y;
                #endif
                half fAlpha = tex2D (_FogOfWarTex, UV).a;
                          
                return half4(0,0,0,fAlpha);
            }

            ENDCG
        }
    }
}
