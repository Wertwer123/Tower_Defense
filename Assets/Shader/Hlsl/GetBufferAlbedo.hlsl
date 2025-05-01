#ifndef DUAL_BUFFER_FUNCTIONS_INCLUDED
#define DUAL_BUFFER_FUNCTIONS_INCLUDED

// Declare textures and samplers
Texture2D _GBuffer0;
SamplerState sampler_GBuffer0;

void GetGBufferAlbedo_float(float2 ScreenPosition, out float4 Albedo)
{
    Albedo = _GBuffer0.SampleBias(sampler_GBuffer0, ScreenPosition, 0);
}

#endif // DUAL_BUFFER_FUNCTIONS_INCLUDED
