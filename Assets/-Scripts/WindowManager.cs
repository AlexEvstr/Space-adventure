using UnityEngine;
using System.Collections;

public class WindowManager : MonoBehaviour
{
    [SerializeField] private float animationDuration = 0.25f;

    public void OpenWindow(GameObject targetWindow)
    {
        if (targetWindow == null) return;

        targetWindow.SetActive(true);
        Transform inner = targetWindow.transform.GetChild(0);
        inner.localScale = Vector3.zero;
        StartCoroutine(ScaleOverTime(inner, Vector3.one));
    }

    public void CloseWindow(GameObject targetWindow)
    {
        if (targetWindow == null) return;

        Transform inner = targetWindow.transform.GetChild(0);
        StartCoroutine(ScaleOverTime(inner, Vector3.zero, () => targetWindow.SetActive(false)));
    }

    private IEnumerator ScaleOverTime(Transform target, Vector3 to, System.Action onComplete = null)
    {
        Vector3 from = target.localScale;
        float time = 0f;

        while (time < animationDuration)
        {
            time += Time.unscaledDeltaTime;
            float t = time / animationDuration;
            target.localScale = Vector3.Lerp(from, to, t);
            yield return null;
        }

        target.localScale = to;
        onComplete?.Invoke();
    }
}
