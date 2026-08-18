using UnityEngine;

  public class HitApplierVisual : MonoBehaviour
  {
      [SerializeField] HitApplier hitApplier;
      [SerializeField] MeshRenderer meshRenderer;

      void OnEnable()
      {
          hitApplier.OnActiveChanged += OnActiveChanged;
      }

      void OnDisable()
      {
          hitApplier.OnActiveChanged -= OnActiveChanged;
      }

      void OnActiveChanged(bool active)
      {
          meshRenderer.material.color = active ? Color.red : Color.gray;
      }
  }
