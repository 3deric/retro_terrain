Shader "RetroTerrain/Terrain" 
{
    Properties
    {
        _MainTex("MainTex", 2D) = "white"{}
        _Layer1("Layer1", 2D) = "white"{}
        _Layer2("Layer2", 2D) = "white"{}
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
          float4 vertColor : COLOR;
    };

    sampler2D _MainTex;
    sampler2D _Layer1;
    sampler2D _Layer2;
    float _LineThickness;
    float _LineVisibility;

    void surf (Input IN, inout SurfaceOutput o) 
    {
      float4 lines = float(1.0);
      fixed4 layer0 = tex2D(_MainTex, IN.uv_MainTex).rgba * (1.0 - IN.vertColor.r - IN.vertColor.g -IN.vertColor.b);
      fixed4 layer1 = tex2D(_Layer1, IN.uv_MainTex).rgba * IN.vertColor.r;
      fixed4 layer2 = tex2D(_Layer2, IN.uv_MainTex).rgba* IN.vertColor.g;

      if((IN.uv_MainTex.x < _LineThickness || IN.uv_MainTex.x > 1.0 - _LineThickness) || (IN.uv_MainTex.y < _LineThickness || IN.uv_MainTex.y > 1.0 - _LineThickness))
      {
          lines = 1 - _LineVisibility;
      }
      o.Albedo =  (layer0 + layer1 + layer2 )* lines;
    }
      ENDCG
    }

    Fallback "Diffuse"
  }