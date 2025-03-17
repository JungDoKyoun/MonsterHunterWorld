Shader "Custom/AdvancedShader"
{
    Properties
    {
        // Albedo & Metallic (ALBD)
        _AlbedoMetallicTex ("Albedo & Metallic (ALBD)", 2D) = "white" {}

        // Alpha Mask (ALP)
        _AlphaTex ("Alpha Mask (ALP)", 2D) = "white" {}

        // Normal, Roughness (NRMR)
        _NormalRoughnessTex ("Normal & Roughness (NRMR)", 2D) = "bump" {}

        // Normal, Roughness, Subsurface (NRRT)
        _NormalRoughnessSSSTex ("Normal, Roughness, Subsurface (NRRT)", 2D) = "bump" {}

        // Emission Textures
        _EmissionTex ("Emission (EMI)", 2D) = "black" {}
        _DetailEmissionTex ("Detail Emission (DEMI)", 2D) = "black" {}
        _WeaponEmissionTex ("Weapon Emission (WEMI)", 2D) = "black" {}

        // User Color Change Map (CMM)
        _ColorChangeTex ("User Color Change Map (CMM)", 2D) = "white" {}

        // CubeMap Reflection
        _CubeMap ("Reflection Cubemap (HDR)", Cube) = "" {}

        // UI Texture
        _UIMap ("UI Texture (IAM)", 2D) = "white" {}

        // Additional Settings
        _Color ("Color Tint", Color) = (1,1,1,1)
        _EmissionIntensity ("Emission Intensity", Range(0,5)) = 1.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 300

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows
        #pragma target 3.0

        sampler2D _AlbedoMetallicTex, _AlphaTex;
        sampler2D _NormalRoughnessTex, _NormalRoughnessSSSTex;
        sampler2D _EmissionTex, _DetailEmissionTex, _WeaponEmissionTex;
        sampler2D _ColorChangeTex, _UIMap;
        samplerCUBE _CubeMap;

        fixed4 _Color;
        float _EmissionIntensity;

        struct Input
        {
            float2 uv_AlbedoMetallicTex;
            float2 uv_AlphaTex;
            float2 uv_NormalRoughnessTex;
            float2 uv_EmissionTex;
            float2 uv_UIMap;
            float3 worldRefl;
            INTERNAL_DATA
        };

     void surf (Input IN, inout SurfaceOutputStandard o)
{
    // ALBD: Albedo (RGB) + Metallic (A)
    fixed4 albdTex = tex2D(_AlbedoMetallicTex, IN.uv_AlbedoMetallicTex);
    o.Albedo = albdTex.rgb * _Color.rgb; // Albedo 강제 적용

    // NRMR: Normal (RGB) + Roughness (A)
    fixed4 nrmrTex = tex2D(_NormalRoughnessTex, IN.uv_NormalRoughnessTex);
    o.Normal = UnpackNormal(nrmrTex); // Normal 적용
    o.Smoothness = 1.0 - nrmrTex.a; // Roughness 변환 적용

    // NRRT: Subsurface Scattering 비활성화 (일단 제외하고 테스트)
    // fixed4 nrrtTex = tex2D(_NormalRoughnessSSSTex, IN.uv_NormalRoughnessTex);
    // float sssIntensity = nrrtTex.a; // SSS 강도

    // Alpha Mask 제거 (하얀색 문제 해결용)
    // fixed4 alphaMask = tex2D(_AlphaTex, IN.uv_AlphaTex);
    // o.Albedo = lerp(o.Albedo, alphaMask.rgb, alphaMask.a);

    // Metallic 값 강제 적용
    o.Metallic = albdTex.a; 

    // Emission 제거 (문제 원인 분석)
    // fixed4 emission = tex2D(_EmissionTex, IN.uv_EmissionTex);
    // o.Emission = emission.rgb * _EmissionIntensity;
}

        ENDCG
    }
    FallBack "Diffuse"
}
