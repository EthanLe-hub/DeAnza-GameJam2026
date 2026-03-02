using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class UIImageFlipper : MonoBehaviour
{
    public Image imageToFlip;

    [Header("Flip Options")]
    public bool flipX = false;
    public bool flipY = false;

    void Update()
    {
        if (imageToFlip != null)
        {
            Vector3 scale = imageToFlip.rectTransform.localScale;
            scale.x = flipX ? -Mathf.Abs(scale.x) : Mathf.Abs(scale.x);
            scale.y = flipY ? -Mathf.Abs(scale.y) : Mathf.Abs(scale.y);
            imageToFlip.rectTransform.localScale = scale;
        }
    }
}