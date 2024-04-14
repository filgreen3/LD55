using UnityEngine;
using System.Collections.Generic;

public class OrganPlacesDisplay : MonoBehaviour, ISystem
{
    private List<SpriteRenderer> _particles = new();

    private void Start()
    {
        OrganBuilder.OnStartBuildingOrgan += OrganBuilder_OnStartBuildingOrgan;
        OrganBuilder.OnEndBuildingOrgan += OrganBuilder_OnEndBuildingOrgan;
    }

    private void OrganBuilder_OnStartBuildingOrgan(Organ organ)
    {
        var layerMask = LayerMask.GetMask("Part");
        foreach (var part in OrganBuilder.ConnectedOrgans)
        {
            foreach (var place in part.AttachPoints)
            {
                if (!Physics2D.OverlapPoint(part.GetTransformedPoint(place), layerMask))
                {
                    _particles.Add(ResourceParticleHelperSystem.GetParticle(OrganResources.TPlace));
                    _particles[^1].transform.position = part.GetTransformedPoint(place);
                }
            }
        }
    }

    private void OrganBuilder_OnEndBuildingOrgan(Organ organ)
    {
        foreach (var item in _particles)
        {
            ResourceParticleHelperSystem.ReleaseParticle(item);
        }
        _particles.Clear();
    }
}