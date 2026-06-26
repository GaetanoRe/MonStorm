Shader "Custom/KuwaharaFilter"
{
    Properties
    {
        _Radius ("Kernel Radius", Range(1, 8)) = 4
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

            // Core first, then Blit (Blit.hlsl gives us Vert, Varyings, _BlitTexture and the samplers)
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Blit.hlsl"

            float4 _BlitTexture_TexelSize; // .xy = (1/width, 1/height)
            float  _Radius;

            #define SAMPLE(uv) SAMPLE_TEXTURE2D_X(_BlitTexture, sampler_LinearClamp, uv)

            // Mean + summed-variance over a rectangular sub-window [lo, hi] (in texels, relative to center)
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
                // variance per channel = E[x^2] - E[x]^2 ; abs() guards against tiny negatives from fp error
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

                // Four overlapping quadrants. They share the center row/column,
                // which is the classic Kuwahara arrangement.
                RegionStats(uv, texel, int2(-r,  0), int2( 0,  r), m, v); if (v < bestVar) { bestVar = v; bestMean = m; } // upper-left
                RegionStats(uv, texel, int2( 0,  0), int2( r,  r), m, v); if (v < bestVar) { bestVar = v; bestMean = m; } // upper-right
                RegionStats(uv, texel, int2(-r, -r), int2( 0,  0), m, v); if (v < bestVar) { bestVar = v; bestMean = m; } // lower-left
                RegionStats(uv, texel, int2( 0, -r), int2( r,  0), m, v); if (v < bestVar) { bestVar = v; bestMean = m; } // lower-right

                return float4(bestMean, 1.0);
            }
            ENDHLSL
        }
    }
}
