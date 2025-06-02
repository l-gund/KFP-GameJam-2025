using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PopupBounce : MonoBehaviour
{
    public float popupHeight = 800f;
    public float bounceAmount = 50f;
    public float duration = 0.5f;

    private RectTransform rect;

    void Start()
    {
        rect = GetComponent<RectTransform>();
        rect.anchoredPosition = new Vector2(0, -popupHeight);
        StartCoroutine(AnimateBounce());
    }

    IEnumerator AnimateBounce()
    {
        Vector2 target = new Vector2(0, bounceAmount);
        Vector2 settle = new Vector2(0, 0);

        float t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            float eased = Mathf.Sin(t * Mathf.PI * 0.5f); // ease out
            rect.anchoredPosition = Vector2.Lerp(new Vector2(0, -popupHeight), target, eased);
            yield return null;
        }

        t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime / (duration * 0.5f);
            float eased = Mathf.Sin(t * Mathf.PI); // bounce-like ease
            rect.anchoredPosition = Vector2.Lerp(target, settle, eased);
            yield return null;
        }
    }
}
