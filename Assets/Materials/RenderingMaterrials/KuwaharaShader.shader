Shader "Custom/KuwaharaShader"
{
    Properties
    {
        _Radius   ("Kernel Radius", Range(1, 8)) = 4
        _Strength ("Effect Strength", Range(0, 1)) = 1
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" }
        ZWrite Off
        ZTest Always
        Cull Off

        Pass
        {
            Name "Kuwahara"

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"
            float _Radius;
            float _Strength;

            #define SAMPLE(uv) SAMPLE_TEXTURE2D_X(_BlitTexture, sampler_LinearClamp, uv)

            // Mean + summed-variance over a rectangular sub-window [lo, hi] (in texels)
            void RegionStats(float2 uv, float2 texel, int2 lo, int2 hi, out float3 mean, out float variance)
            {
                float3 sum   = 0.0;
                float3 sqSum = 0.0;
                float  count = 0.0;

                [loop] for (int x = lo.x; x <= hi.x; x++)
                {
                    [loop] for (int y = lo.y; y <= hi.y; y++)
                    {
                        float3 c = SAMPLE(uv + float2(x, y) * texel).rgb;
                        sum   += c;
                        sqSum += c * c;
                        count += 1.0;
                    }
                }

                mean = sum / count;
                float3 v = abs(sqSum / count - mean * mean);
                variance = v.r + v.g + v.b;
            }

            float4 Frag(Varyings input) : SV_Target
            {
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

                float2 uv    = input.texcoord;
                float2 texel = _BlitTexture_TexelSize.xy;
                int    r     = (int)_Radius;

                float3 bestMean = 0.0;
                float  bestVar  = 3.402823466e+38; // FLT_MAX
                float3 m; float v;

                RegionStats(uv, texel, int2(-r,  0), int2( 0,  r), m, v); if (v < bestVar) { bestVar = v; bestMean = m; } // upper-left
                RegionStats(uv, texel, int2( 0,  0), int2( r,  r), m, v); if (v < bestVar) { bestVar = v; bestMean = m; } // upper-right
                RegionStats(uv, texel, int2(-r, -r), int2( 0,  0), m, v); if (v < bestVar) { bestVar = v; bestMean = m; } // lower-left
                RegionStats(uv, texel, int2( 0, -r), int2( r,  0), m, v); if (v < bestVar) { bestVar = v; bestMean = m; } // lower-right

                // Blend the painterly result back toward the original sharp pixel.
                // _Strength = 1 -> full Kuwahara, lower -> clearer / more detail retained.
                float3 original = SAMPLE(uv).rgb;
                float3 result   = lerp(original, bestMean, _Strength);

                return float4(result, 1.0);
            }
            ENDHLSL
        }
    }
}