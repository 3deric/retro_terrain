Shader "RetroTerrain/Terrain" 
{
    Properties
    {
        _MainTex("MainTex", 2D) = "white"{}
        _LineThickness ("LineThickness", Range(0, 0.025)) = 0.02
        _LineVisibility ("LineVisibility", Range(0, 1)) = 0.2
    }

    SubShader 
    {
    Tags { "RenderType" = "Opaque" }
    CGPROGRAM
    #pragma surface surf Lambert addshadow 
      
    struct Input 
    {
          float2 uv_MainTex;
    };

    sampler2D _MainTex;
    float _LineThickness;
    float _LineVisibility;

    void surf (Input IN, inout SurfaceOutput o) 
    {
      float4 lines = float(1.0);
      fixed4 main = tex2D(_MainTex, IN.uv_MainTex).rgba;

      if((IN.uv_MainTex.x < _LineThickness || IN.uv_MainTex.x > 1.0 - _LineThickness) || (IN.uv_MainTex.y < _LineThickness || IN.uv_MainTex.y > 1.0 - _LineThickness))
      {
          lines = 1 - _LineVisibility;
      }
      o.Albedo =  main * lines;
    }
      ENDCG
    }

    Fallback "Diffuse"
  }