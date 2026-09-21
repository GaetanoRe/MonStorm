using UnityEngine;

public class HitApplierVisual : MonoBehaviour
{
    [SerializeField] HitApplierComponentConfigured hitApplier;
    [SerializeField] MeshRenderer meshRenderer;


    void Start()
    {
        hitApplier.Data.OnActiveChanged += OnActiveChanged;
    }

    void OnDestroy()
    {
        hitApplier.Data.OnActiveChanged -= OnActiveChanged;
    }

    void OnActiveChanged(bool active)
    {
        meshRenderer.material.color = active ? Color.red : Color.gray;
    }
}
