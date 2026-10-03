using UnityEngine;
using DG.Tweening;

public class BirdAnimation : MonoBehaviour
{

    [Header("Animations")]
    [SerializeField] private float punchScale = 0.01f;
    [SerializeField] private float punchDuration = 0.5f;

    private Tween currentTween;

    public void PlayPickupAnimation()
    {
        if (this != null && gameObject.activeInHierarchy)
        {
            if (currentTween != null && currentTween.IsActive())
                currentTween.Kill();

            currentTween = transform.DOPunchScale(Vector3.one * punchScale, punchDuration);
        }
    }

    private void OnDestroy()
    {
        // Kill tween if this object is destroyed
        if (currentTween != null && currentTween.IsActive())
            currentTween.Kill();
    }

}
