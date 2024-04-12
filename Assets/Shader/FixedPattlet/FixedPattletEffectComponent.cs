using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[Serializable, VolumeComponentMenuForRenderPipeline("Custom/FixedPattletEffectComponent", typeof(UniversalRenderPipeline))]
public class FixedPattletEffectComponent : VolumeComponent, IPostProcessComponent
{
    // For example, an intensity parameter that goes from 0 to 1
    public ClampedIntParameter colorCount = new ClampedIntParameter(value: 64, min: 1, max: 256, overrideState: true);
    // A color that is constant even when the weight changes
    public Texture2DParameter pallet = new Texture2DParameter(null, true);
    public ColorParameter baseColor = new ColorParameter(Color.white, true);

    // Other 'Parameter' variables you might have

    // Tells when our effect should be rendered
    public bool IsActive() => colorCount.value > 0 && pallet != null;

    // I have no idea what this does yet but I'll update the post once I find an usage
    public bool IsTileCompatible() => true;
}
