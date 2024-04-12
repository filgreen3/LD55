using UnityEngine;
using UnityEngine.Rendering.Universal;

[System.Serializable]
public class FixedPattletPostProcessRenderer : ScriptableRendererFeature
{
    FixedPattletPostProcessPass pass;

    [SerializeField] private Shader _defShader;

    public override void Create()
    {
        pass = new FixedPattletPostProcessPass(_defShader);
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(pass);
    }
}
