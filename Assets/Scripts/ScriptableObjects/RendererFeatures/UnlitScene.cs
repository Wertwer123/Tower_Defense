using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.Universal;

namespace ScriptableObjects.RendererFeatures
{
    public class GlobalGbuffersRendererFeature : ScriptableRendererFeature
    {
        private GlobalGBuffersRenderPass _globalGbuffersRenderPass;

        /// <inheritdoc />
        public override void Create()
        {
            _globalGbuffersRenderPass = new GlobalGBuffersRenderPass
            {
                //  This pass must be injected after rendering the deferred lights or later.
                renderPassEvent = RenderPassEvent.AfterRenderingDeferredLights
            };
        }

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            renderer.EnqueuePass(_globalGbuffersRenderPass);
        }

        private class GlobalGBuffersRenderPass : ScriptableRenderPass
        {
            private readonly string _passName = "Make gBuffer Components Global";
            private Material _material;

            private void SetGlobalGBufferTextures(IRasterRenderGraphBuilder builder, TextureHandle[] gBuffer)
            {
                builder.SetGlobalTextureAfterPass(gBuffer[0], Shader.PropertyToID("_GBuffer0"));
            }

            // RecordRenderGraph is where the RenderGraph handle can be accessed, through which render passes can be added to the graph.
            // FrameData is a context container through which URP resources can be accessed and managed.
            public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
            {
                UniversalRenderingData universalRenderingData = frameData.Get<UniversalRenderingData>();
                // The gBuffer components are only used in deferred mode
                if (universalRenderingData.renderingMode != RenderingMode.Deferred)
                    return;

                // Get the gBuffer texture handles are stored in the resourceData
                UniversalResourceData resourceData = frameData.Get<UniversalResourceData>();
                TextureHandle[] gBuffer = resourceData.gBuffer;

                using (IRasterRenderGraphBuilder builder =
                       renderGraph.AddRasterRenderPass(_passName, out PassData passData))
                {
                    builder.AllowPassCulling(false);
                    // Set the gBuffers to be global after the pass
                    SetGlobalGBufferTextures(builder, gBuffer);
                    builder.SetRenderFunc((PassData data, RasterGraphContext context) =>
                    {
                        /* nothing to be rendered */
                    });
                }
            }

            private class PassData
            {
            }
        }
    }
}