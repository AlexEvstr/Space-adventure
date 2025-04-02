using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    [System.Serializable]
    public class PlayerData
    {
        public string playerName;
        public int spriteIndex;
        public PlayerPieceController controller;
    }
    public Image avatarImage; // куда показываем спрайт игрока
    public Text nameText;     // куда выводим имя игрока

    public Sprite[] playerSprites; // те же, что в выборе
    [SerializeField] private GameObject[] playerPieces; // Привязал вручную
    private List<PlayerData> players = new List<PlayerData>();

    private int currentPlayerIndex = 0;
    [SerializeField] private Button throwButton;
    [SerializeField] private Sprite[] pieceSprites;

    private void Start()
    {
        LoadPlayers();
    }


    private void LoadPlayers()
    {
        for (int i = 0; i < 4; i++)
        {
            string nameKey = $"PlayerName{i}";
            string spriteKey = $"PlayerSpriteIndex{i}";

            if (PlayerPrefs.HasKey(nameKey) && PlayerPrefs.HasKey(spriteKey))
            {
                string playerName = PlayerPrefs.GetString(nameKey);
                int spriteIndex = PlayerPrefs.GetInt(spriteKey);

                GameObject piece = playerPieces[i];
                piece.SetActive(true);
                var controller = piece.GetComponent<PlayerPieceController>();

                var img = piece.GetComponent<Image>(); // если UI
                if (img != null && spriteIndex >= 0 && spriteIndex < pieceSprites.Length)
                {
                    img.sprite = pieceSprites[spriteIndex];
                }

                players.Add(new PlayerData
                {
                    playerName = playerName,
                    spriteIndex = spriteIndex,
                    controller = controller
                });
            }
            else
            {
                playerPieces[i].SetActive(false);
            }
        }

        ShowCurrentPlayerUI();
    }

    public void OnDiceResult(int steps)
    {
        StartCoroutine(HandlePlayerMove(steps));
    }

    private IEnumerator HandlePlayerMove(int steps)
    {
        throwButton.interactable = false;

        var current = players[currentPlayerIndex];
        ShowCurrentPlayerUI();

        yield return current.controller.MoveStepByStepCoroutine(steps);

        currentPlayerIndex = (currentPlayerIndex + 1) % players.Count;
        ShowCurrentPlayerUI();

        throwButton.interactable = true;
    }



    private void ShowCurrentPlayerUI()
    {
        var player = players[currentPlayerIndex];
        avatarImage.sprite = playerSprites[player.spriteIndex];
        nameText.text = player.playerName;
    }
}
