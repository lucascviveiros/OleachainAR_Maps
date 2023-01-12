using UnityEngine;
using UnityEngine.UI;

public class MultiImageTargetGraphics : MonoBehaviour
{
    [SerializeField] private RawImage[] targetImages;

    public RawImage[] GetTargetImages => targetImages;
}