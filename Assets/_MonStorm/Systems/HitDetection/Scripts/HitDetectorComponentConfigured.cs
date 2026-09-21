using UnityEngine;

public class HitDetectorComponentConfigured : MonoBehaviour
{
    public HitDetector Data { get; private set; }


    public void Initialize(HitDetector hitDetector)
    {
        Data = hitDetector;
    }
}
