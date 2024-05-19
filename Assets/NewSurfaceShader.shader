Shader "Custom/NewSurfaceShader"
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _Lerp ("Lerp", Range(0, 1)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Lambert

        sampler2D _MainTex;
        float _Lerp;

        struct Input
        {
            float2 uv_MainTex;
        };

        void surf (Input IN, inout SurfaceOutput o)
        {
            half4 tex = tex2D(_MainTex, IN.uv_MainTex);

            // İlk shader: Basit diffuse
            half3 diffuseColor = tex.rgb;

            // İkinci shader: Desaturate
            half gray = dot(tex.rgb, half3(0.3, 0.59, 0.11));
            half3 desaturateColor = half3(gray, gray, gray);

            // İki shader arasında geçiş yap
            o.Albedo = lerp(diffuseColor, desaturateColor, _Lerp);
        }
        ENDCG
    }
    FallBack "Diffuse"
}
