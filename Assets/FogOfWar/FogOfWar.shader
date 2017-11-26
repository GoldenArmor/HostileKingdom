// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "YueChuan/FogOfWar/FogOfWar"
 {
    CGINCLUDE
    #include "UnityCG.cginc"
    
    uniform half4 _ViewCenter;
    uniform half  _BufferAspect;
    uniform half  _FogDensity;
    
	uniform sampler2D _NoiseTex;
	
    struct VS_OUTPUT
    {
         float4 Position : SV_POSITION;
		 half2 v2Texcoord : TEXCOORD0;
    };
            
    VS_OUTPUT MainVS(appdata_base input)
    {
          VS_OUTPUT output = (VS_OUTPUT)0;
                
          output.Position = UnityObjectToClipPos(input.vertex);
          output.v2Texcoord = input.texcoord;

          return output;
     }

     fixed4 MainPS(VS_OUTPUT input) : COLOR 
     {    
           fixed noise = tex2D(_NoiseTex,input.v2Texcoord).r;
           half2 AB = half2(_ViewCenter.z,_ViewCenter.z * _BufferAspect) * noise;
           half M = ((input.v2Texcoord.x - _ViewCenter.x) * (input.v2Texcoord.x - _ViewCenter.x))/(AB.x * AB.x);
           half S = ((input.v2Texcoord.y - _ViewCenter.y) * (input.v2Texcoord.y - _ViewCenter.y))/(AB.y * AB.y);
           half dist = M + S;
           half alpha = smoothstep(AB.x - _ViewCenter.w,AB.x + _ViewCenter.w,dist) * _FogDensity;
		  
		   return float4(0,0,0,alpha);
     }
    ENDCG
    SubShader 
	{
        Tags{ "RenderType" = "Opaque" "Queue"="Geometry" }

		Pass
		{
        	Name "FogOfWar"
        	Lighting Off
			ZWrite Off
			AlphaTest Off
			BlendOp Min
	        Blend SrcAlpha DstAlpha
			//cull front
			
			CGPROGRAM

            #pragma vertex MainVS
            #pragma fragment MainPS
            #pragma target 2.0
           
            ENDCG
		}
    }
}
