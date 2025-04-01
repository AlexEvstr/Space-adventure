using UnityEngine;
using System.Collections;

public class FadeUIController : MonoBehaviour
{
    public float fadeDuration = 0.5f;

    public void OpenWindow(GameObject window)
    {
        CanvasGroup cg = GetCanvasGroup(window);
        if (cg == null) return;

        window.SetActive(true);
        StartCoroutine(FadeCanvas(cg, 0f, 1f));
    }

    public void CloseWindow(GameObject window)
    {
        CanvasGroup cg = GetCanvasGroup(window);
        if (cg == null) return;

        StartCoroutine(FadeCanvas(cg, 1f, 0f, () => window.SetActive(false)));
    }

    private IEnumerator FadeCanvas(CanvasGroup canvasGroup, float from, float to, System.Action onComplete = null)
    {
        float elapsed = 0f;
        canvasGroup.alpha = from;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(from, to, elapsed / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = to;
        canvasGroup.blocksRaycasts = (to > 0.9f);
        canvasGroup.interactable = (to > 0.9f);
        onComplete?.Invoke();
    }

    private CanvasGroup GetCanvasGroup(GameObject obj)
    {
        CanvasGroup cg = obj.GetComponent<CanvasGroup>();
        if (cg == null)
        {
            cg = obj.AddComponent<CanvasGroup>();
        }
        return cg;
    }
}
