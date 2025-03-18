Shader "Custom/FullFeatureShader"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _ALBA ("Albedo + Alpha", 2D) = "white" {}
        _ALBD ("Albedo + Metallic", 2D) = "white" {}
        _NRM ("Normal Map", 2D) = "white" {}
        _NRMR ("Normal + Roughness", 2D) = "white" {}
        _NRRT ("Normal + Roughness + Subsurface", 2D) = "white" {}
        _MSK1 ("Translucency + AO", 2D) = "white" {}
        _MSK2 ("Flow Map", 2D) = "white" {}
        _MSK4 ("EFX Color Mask", 2D) = "white" {}
        _EMI ("Emission Map", 2D) = "black" {}
        _ALP ("Alpha Mask", 2D) = "white" {}
        _TranslucencyStrength ("Translucency Strength", Range(0, 1)) = 0.5
        _SSSColor ("Subsurface Color", Color) = (1,1,1,1)
        _EmissionStrength ("Emission Strength", Range(0, 5)) = 1.0
        _AlphaCutoff ("Alpha Cutoff", Range(0, 1)) = 0.2
    }
    SubShader
    {
        Tags { "RenderType"="TransparentCutout" }
        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows alpha:test
        #pragma target 3.0
        
        sampler2D _MainTex;
        sampler2D _ALBA;
        sampler2D _ALBD;
        sampler2D _NRMR;
        sampler2D _NRRT;
        sampler2D _MSK1;
        sampler2D _MSK2;
        sampler2D _MSK4;
        sampler2D _EMI;
        sampler2D _ALP;
        float _TranslucencyStrength;
        fixed4 _SSSColor;
        float _EmissionStrength;
        float _AlphaCutoff;
        
        struct Input
        {
            float2 uv_MainTex;
            float2 uv_MSK;
            INTERNAL_DATA
        };
        
        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 albedo = tex2D(_ALBD, IN.uv_MainTex);
            fixed4 albedoAlpha = tex2D(_ALBA, IN.uv_MainTex);
            fixed4 nrmr = tex2D(_NRMR, IN.uv_MainTex);
            fixed4 nrrt = tex2D(_NRRT, IN.uv_MainTex);
            fixed4 msk1 = tex2D(_MSK1, IN.uv_MSK);
            fixed4 msk2 = tex2D(_MSK2, IN.uv_MSK);
            fixed4 msk4 = tex2D(_MSK4, IN.uv_MSK);
            fixed4 emi = tex2D(_EMI, IN.uv_MSK);
            fixed4 alp = tex2D(_ALP, IN.uv_MSK);
            
            float3 worldNormal = WorldNormalVector(IN, o.Normal);
            float3 lightDir = normalize(_WorldSpaceLightPos0.xyz);
            float facing = saturate(dot(worldNormal, lightDir));
            float translucency = (1.0 - facing) * msk1.g * _TranslucencyStrength;
            
            // Translucency 값이 너무 크지 않도록 제한
            translucency = clamp(translucency, 0.0, 0.5);
            
            fixed3 translucencyColor = _SSSColor.rgb * translucency;
            fixed3 emissionColor = emi.rgb * _EmissionStrength;
            
            // Alpha 값 보정 (Alpha Mask 값을 직접 사용하여 예상대로 적용되도록 함)
            float alphaValue = albedoAlpha.a * alp.r;
            
            // Alpha 컷오프 조정하여 자연스럽게 유지
            clip(alphaValue - _AlphaCutoff);
            
            o.Albedo = albedo.rgb * (1.0 - translucency) + translucencyColor; // Translucency 컬러가 알베도를 덮지 않도록
            o.Metallic = albedo.a;
            o.Smoothness = 1.0 - nrmr.b;
            o.Occlusion = msk1.b;
            o.Emission = emissionColor;
            o.Normal = UnpackNormal(nrrt);
            o.Alpha = alphaValue;
        }
        ENDCG
    }
}
