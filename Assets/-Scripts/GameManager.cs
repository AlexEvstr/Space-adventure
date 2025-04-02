using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameManager : MonoBehaviour
{
    [System.Serializable]
    public class PlayerData
    {
        public string playerName;
        public int spriteIndex;
        public PlayerPieceController controller;
        public bool hasFinished = false;
    }

    [SerializeField] private GameObject[] maps;
    [SerializeField] private Sprite[] backgrounds;
    [SerializeField] private Image _bgImage;
    private GameObject activeMap;
    private BoardManager board;

    private PlayerData winner = null;
    public Image avatarImage; // куда показываем спрайт игрока
    public Text nameText;     // куда выводим имя игрока

    public Sprite[] playerSprites; // те же, что в выборе
    [SerializeField] private GameObject[] playerPieces; // Привязал вручную
    private List<PlayerData> players = new List<PlayerData>();

    private int currentPlayerIndex = 0;
    [SerializeField] private Button throwButton;
    [SerializeField] private Sprite[] pieceSprites;
    [SerializeField] private Text _winnerNameText;
    [SerializeField] private GameObject _winPopup;
    private WindowManager _windowManager;
    private AudioController _audioController;

    private void Start()
    {
        _audioController = GetComponent<AudioController>();
        _windowManager = GetComponent<WindowManager>();
        int mapIndex = PlayerPrefs.GetInt("ChoosenMap", 0);
        _bgImage.sprite = backgrounds[mapIndex];
        // Отключаем все карты
        foreach (var map in maps)
            map.SetActive(false);

        // Включаем выбранную
        activeMap = maps[mapIndex];
        activeMap.SetActive(true);

        // Получаем ссылку на BoardManager этой карты
        board = activeMap.GetComponent<BoardManager>();
        LoadPlayers();
    }


    private void LoadPlayers()
    {
        PlayerPieceController[] foundControllers = activeMap.GetComponentsInChildren<PlayerPieceController>(true);

        for (int i = 0; i < 4; i++)
        {
            var controller = foundControllers[i];
            controller.board = board;

            GameObject piece = controller.gameObject;
            piece.SetActive(true);

            string nameKey = $"PlayerName{i}";
            string spriteKey = $"PlayerSpriteIndex{i}";

            if (PlayerPrefs.HasKey(nameKey) && PlayerPrefs.HasKey(spriteKey))
            {
                string playerName = PlayerPrefs.GetString(nameKey);
                int spriteIndex = PlayerPrefs.GetInt(spriteKey);

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
                piece.SetActive(false);
            }
        }

        ShowCurrentPlayerUI();
    }

    public void OnDiceResult(int steps)
    {
        var current = players[currentPlayerIndex];

        // ⛔ Пропускаем ход, если игрок уже дошёл до финиша
        

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

        if (current.controller.CurrentTileIndex == current.controller.board.tiles.Count - 1 && !current.hasFinished)
        {
            CheckVictory(current);
        }

    }



    private void ShowCurrentPlayerUI()
    {
        var player = players[currentPlayerIndex];
        avatarImage.sprite = playerSprites[player.spriteIndex];
        nameText.text = player.playerName;
        if (player.hasFinished)
        {
            currentPlayerIndex = (currentPlayerIndex + 1) % players.Count;
            ShowCurrentPlayerUI();
        }
    }

    private void CheckVictory(PlayerData currentPlayer)
    {
        int victoryMode = PlayerPrefs.GetInt("VictoryConditions", 0);

        currentPlayer.hasFinished = true;

        if (victoryMode == 0)
        {
            // Побеждает первый, кто дошёл
            Debug.Log($"🏆 Победитель: {currentPlayer.playerName}");
            // Здесь можешь вызвать окно победы
            int gamesPlayed = PlayerPrefs.GetInt("GamesPlayed", 0);
            gamesPlayed++;
            PlayerPrefs.SetInt("GamesPlayed", gamesPlayed);
            _winnerNameText.text = currentPlayer.playerName;
            _windowManager.OpenWindow(_winPopup);
            _audioController.MuteMusic();
            _audioController.PlayWinSound();
        }
        else if (victoryMode == 1)
        {
            // Если победитель ещё не назначен — назначаем
            if (winner == null)
            {
                winner = currentPlayer;
            }

            // Считаем, сколько НЕ финишировали
            int notFinished = players.Count(p => !p.hasFinished);

            // Если остался только один, игра заканчивается
            if (notFinished == 1)
            {
                Debug.Log($"🏁 Игра окончена. Победитель: {winner.playerName}");
                // Здесь можешь вызвать экран победы или перейти в меню

                int gamesPlayed = PlayerPrefs.GetInt("GamesPlayed", 0);
                gamesPlayed++;
                PlayerPrefs.SetInt("GamesPlayed", gamesPlayed);
                _winnerNameText.text = winner.playerName;
                _windowManager.OpenWindow(_winPopup);
                _audioController.MuteMusic();
                _audioController.PlayWinSound();
            }
        }
    }
}
