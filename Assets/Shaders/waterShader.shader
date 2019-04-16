Shader "RetroTerrain/Water" 
{
    Properties
    {
        _WaterTex("WaterNormal", 2D) = "white"{}
        _WaterDispTex("WaterDisp", 2D) = "white"{}
        _FoamTex("FoamMask", 2D) = "white"{}
        _FoamThickness ("FoamThickness", Range(0.0, 0.1)) = 0.015
        _FoamBias ("FoamBias", Range(0, 10)) = 0.5
        _FoamFalloff ("FoamFalloff", Range(0, 1)) = 0.5
    }

    SubShader 
    {
    Tags { "RenderType"="Opaque" "Queue"="Transparent"}
		GrabPass{
		}
    ZWrite Off

    CGPROGRAM
    #pragma surface surf BlinnPhong vertex:vert
  
      
    struct Input 
    {
          float2 uv_WaterTex;
          float2 uv_FoamTex;
          float4 screenPos;
    };

    sampler2D _GrabTexture;
    sampler2D _WaterDispTex;

    void vert(inout appdata_full v)
    {
      float2 offset1 =float2(1.0, 0.5) * _Time * 0.025;
      float2 offset2 = float2(0.25, 0.5) * _Time * 0.015;

      float disp1 = tex2Dlod(_WaterDispTex, float4(v.texcoord.xy *  + offset1, 0,0)).r * 0.075;
      float disp2 = tex2Dlod(_WaterDispTex, float4(v.texcoord.xy * 5 - offset2, 0,0)).r * 0.025;
      v.vertex.y = (disp1 + disp2);
    }

    sampler2D _FoamTex;
    sampler2D _WaterTex;
    sampler2D _CameraDepthTexture;
    float3 _SpecularColor;
    float _Glossiness;
    float _FoamThickness;
    float _FoamBias;
    float _FoamFalloff;

    void surf (Input IN, inout SurfaceOutput o) 
    {
      float2 screenUV = IN.screenPos.xy;
      
      float channelA = tex2D(_FoamTex, IN.uv_FoamTex + (_Time.x * 10 , sin(IN.uv_FoamTex.x))).r;
      float channelB = tex2D(_FoamTex, IN.uv_FoamTex + 0.5 * (cos(IN.uv_FoamTex.y), _Time.x * 5)).b;
      
      float mask = saturate(pow(channelA+channelB, 2));

      float depth = tex2D(_CameraDepthTexture,screenUV).r;
      float fragmentDepth = IN.screenPos.z;
      float depthDifference = abs(depth- fragmentDepth);

      if(depthDifference < _FoamThickness * _FoamFalloff)
      {
        float leading = depthDifference /(_FoamThickness * _FoamFalloff);
        mask *=leading;
      }
      float falloff = 1.0 - (depthDifference / _FoamThickness) + _FoamBias;
      float foamMask = saturate(falloff - mask);

      float3 normal1 = UnpackNormal (tex2D (_WaterTex, IN.uv_WaterTex + float2(_Time.x * 1.5 , 0.5)));
      float3 normal2 = UnpackNormal (tex2D (_WaterTex, IN.uv_WaterTex + 0.5 * float2( 1.0, -_Time.x * 2.5)));
      float3 normal = normalize(normal1 + normal2);

      float4 waterColor = float4(0.65, 0.8, 0.95, 1.0) * 0.85;
      float4 foamColor = float4(0.6, 0.6, 0.6, 1.0) * foamMask;

      float2 distortion = normal * pow (0.1 , 2.0);
      float4 grabColor = tex2D(_GrabTexture, screenUV + distortion);

      o.Albedo = saturate(waterColor * grabColor + foamColor);
      o.Normal = normal;
    }
    ENDCG
    }

    Fallback "Diffuse"
  }