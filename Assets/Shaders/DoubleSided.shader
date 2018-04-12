// Author: Alejandro Orpi 2018
Shader "SevenMasters/DissolveDoubleSided" 
{
	Properties 
	{
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_GlossMapScale ("Smoothness", Range(0,1)) = 0.5
		[NoScaleOffset][Gamma] _MetallicGlossMap("Metalic", 2D) = "gray" {}
		_BumpScale ("Normal scale", Range(0,1)) = 1.0
		[NoScaleOffset][Normal] _BumpMap("Normal", 2D) = "white" {}
		_Occlusion("Occlusion", Range(0,1)) = 1.0
		[NoScaleOffset][Gamma] _OcclusionMap("Ambient Occlusion", 2D) = "white" {}
		[HDR] _EmissionColor("Emission", Color) = (0,0,0)
		[NoScaleOffset]_EmissionTex("Emission (RGB)", 2D) = "white" {}
		[NoScaleOffset]_DissolveMap("Dissolve", 2D) = "white" {}
		_Progress("Dissolve Progress", Range(0,1)) = 0
		_DissolveRange("Dissolve Range", Range(0,1)) = 0.89
		[NoScaleOffset]_DissolveRamp("Dissolve Ramp", 2D) = "white" {}
		_Cutoff("Cutoff", Range(0, 1)) = 0
	}
	SubShader 
	{
		Tags 
		{ 
			"RenderType" = "Opaque"
		}
		LOD 200
		Cull Off

		CGPROGRAM

		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard addshadow alphatest:_Cutoff

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0
		
		sampler2D _MainTex;
		sampler2D _MetallicGlossMap;
		sampler2D _BumpMap;
		sampler2D _OcclusionMap;
		sampler2D _EmissionTex;
		sampler2D _DissolveMap;
		sampler2D _DissolveRamp;

		struct Input 
		{
			float2 uv_MainTex;
		};

		fixed4 _Color;
		half _GlossMapScale;
		half _BumpScale;
		half _Occlusion;
		fixed4 _EmissionColor;
		half _Progress;
		half _DissolveRange;
		
		void surf(Input IN, inout SurfaceOutputStandard o) 
		{
			half dissolveValue = ((tex2D(_DissolveMap, IN.uv_MainTex).rgb) * _Progress) + _Progress;
			half2 rampUV = half2(dissolveValue * _DissolveRange, 0);
			if (dissolveValue > 1 || rampUV.x > 1)
			{
				discard;
			}

			half4 dissolvedPixel = dissolveValue > _DissolveRange ? tex2D(_DissolveRamp, rampUV) : half4(1, 1, 1, 1);
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb * dissolvedPixel.rgb;
			// max between normal emission color and dissolved 'burn effect' emission
			o.Emission = max((tex2D(_EmissionTex, IN.uv_MainTex).rgb * _EmissionColor.rgb * (-_Progress + 1)), (c.rgb * dissolvedPixel.rgb * _Progress));
			o.Alpha = c.a;

			o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_MainTex)) * _BumpScale;
			fixed4 metallic = tex2D(_MetallicGlossMap, IN.uv_MainTex);
			o.Metallic = metallic.r;
			o.Smoothness = _GlossMapScale * metallic.a;
			o.Occlusion = tex2D(_OcclusionMap, IN.uv_MainTex) * _Occlusion;
		}
		ENDCG
	}
	FallBack "Diffuse"
}