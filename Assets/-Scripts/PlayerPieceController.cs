using UnityEngine;
using System.Collections;

public class PlayerPieceController : MonoBehaviour
{
    public RectTransform[] tiles; // Привязать все позиции
    public float moveSpeed = 600f;

    private int currentTileIndex = -1;
    private RectTransform pieceRect;

    private void Awake()
    {
        pieceRect = GetComponent<RectTransform>();
    }

    public void MoveBySteps(int steps)
    {
        StartCoroutine(MoveStepByStepCoroutine(steps));
    }

    public IEnumerator MoveStepByStepCoroutine(int steps)
    {
        for (int i = 0; i < steps; i++)
        {
            int nextIndex = currentTileIndex + 1;

            if (nextIndex >= tiles.Length)
                yield break;

            Vector2 target = tiles[nextIndex].anchoredPosition;
            yield return MoveTo(target);
            currentTileIndex = nextIndex;
        }
    }



    private IEnumerator MoveTo(Vector2 target)
    {
        while (Vector2.Distance(pieceRect.anchoredPosition, target) > 1f)
        {
            pieceRect.anchoredPosition = Vector2.MoveTowards(pieceRect.anchoredPosition, target, moveSpeed * Time.deltaTime);
            yield return null;
        }

        pieceRect.anchoredPosition = target;
        yield return new WaitForSeconds(0.2f); // пауза между шагами
    }
}
