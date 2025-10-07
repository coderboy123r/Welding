Shader "Universal Render Pipeline/PipeGlowUV"
{
    Properties
    {
        _LineColor ("Line Color", Color) = (0,1,1,1)
        _LineThickness ("Line Thickness (0..1 in UV space)", Range(0,1)) = 0.05
        _LinePosition ("Line Position (0=bottom,1=top)", Range(0,1)) = 0.0
    }

    SubShader
    {
        Tags { "RenderPipeline"="UniversalRenderPipeline" "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            Name "ForwardUnlit"
            Tags { "LightMode"="UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv         : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv          : TEXCOORD0;
            };

            float4 _LineColor;
            float _LineThickness;
            float _LinePosition;

            Varyings vert (Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS);
                OUT.uv = IN.uv;
                return OUT;
            }

            half4 frag (Varyings IN) : SV_Target
            {
                // V coordinate along pipe (0 at bottom, 1 at top in UV unwrap)
                float v = saturate(IN.uv.y);

                // distance from the glow center
                float dist = abs(v - _LinePosition);

                // hard cutoff band (no fading)
                float inside = step(dist, _LineThickness * 0.5);

                return half4(_LineColor.rgb, _LineColor.a * inside);
            }
            ENDHLSL
        }
    }
}
