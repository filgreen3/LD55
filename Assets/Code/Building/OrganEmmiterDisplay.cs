using UnityEngine;
using System.Collections.Generic;

public class OrganEmmiterDisplay : MonoBehaviour, ISystem
{
    private List<SpriteRenderer> _particles = new();

    private void Start()
    {
        OrganBuilder.OnStartBuildingOrgan += OrganBuilder_OnStartBuildingOrgan;
        OrganBuilder.OnEndBuildingOrgan += OrganBuilder_OnEndBuildingOrgan;
    }

    private void OrganBuilder_OnStartBuildingOrgan(Organ organ)
    {
        if (!organ.HasOrganComponent<IOrganComponentResourceReceiver>()) return;
        var neededResources = organ.GetOrganComponent<IOrganComponentResourceReceiver>().ResourceType;
        var emiter = (IOrganComponentResourceEmmiter)null;
        foreach (var item in OrganBuilder.ConnectedOrgans)
        {
            if (item == null) continue;
            emiter = item.GetOrganComponent<IOrganComponentResourceEmmiter>();
            if (emiter != null && emiter.ResourceType == neededResources)
            {
                Debug.Log("D");
                var particle = ResourceParticleHelperSystem.GetParticle(neededResources);
                particle.transform.position = item.transform.position;
                _particles.Add(particle);
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
