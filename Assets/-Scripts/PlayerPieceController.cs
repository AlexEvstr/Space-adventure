using UnityEngine;
using System.Collections;

public class PlayerPieceController : MonoBehaviour
{
    public float moveSpeed = 600f;
    public BoardManager board;
    private int currentTileIndex = -1;
    private RectTransform pieceRect;
    public int CurrentTileIndex => currentTileIndex;
    [SerializeField] private AudioController _audioController;

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
            _audioController.PlayMoveSound();
            int nextIndex = currentTileIndex + 1;

            if (nextIndex >= board.tiles.Count) yield break;

            Vector2 target = board.tiles[nextIndex].anchoredPosition;
            yield return MoveTo(target);
            currentTileIndex = nextIndex;
        }
        foreach (var ladder in board.ladders)
        {
            if (board.tiles[currentTileIndex] == ladder.fromTile)
            {
                int targetIndex = board.tiles.IndexOf(ladder.toTile);
                if (targetIndex != -1)
                {
                    yield return MoveTo(board.tiles[targetIndex].anchoredPosition);
                    currentTileIndex = targetIndex;
                }
                break;
            }
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
