using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DiceThrow : MonoBehaviour
{
    [SerializeField] private Transform playerDice;
    private Rigidbody playerRb;
    [SerializeField] private Button throwButton;    
    private GameManager gameManager;


    private bool isPlayerThrown = false;
    private int playerResult = 0;

    private Vector3 playerStartPos;
    private Quaternion playerStartRot;

    [SerializeField] private GameObject[] _diceAndFloor;

    void Start()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        gameManager = GetComponent<GameManager>();
        playerRb = playerDice.GetComponent<Rigidbody>();
        playerStartPos = playerDice.position;
        playerStartRot = playerDice.rotation;
        foreach (var item in _diceAndFloor)
        {
            item.SetActive(false);
        }
        throwButton.onClick.AddListener(OnThrowButtonClicked);
    }

    void Update()
    {
        if (isPlayerThrown && playerRb.IsSleeping() && playerResult == 0)
        {
            playerResult = DetermineTopFace(playerDice);
            //Debug.Log("Игрок выбросил: " + playerResult);
            if (playerResult == 0) playerResult = 1;

            gameManager.OnDiceResult(playerResult);

            StartCoroutine(ResetDiceAfterDelay(0.5f));
        }

    }

    public void OnThrowButtonClicked()
    {
        if (isPlayerThrown) return;
        foreach (var item in _diceAndFloor)
        {
            item.SetActive(true);
        }
        ThrowDice(playerRb);
        isPlayerThrown = true;
        throwButton.interactable = false;
    }

    void ThrowDice(Rigidbody rb)
    {
        //Debug.Log("throw!");
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        Vector3 randomDirection = new Vector3(
            Random.Range(-1f, 1f),
            Random.Range(0.5f, 1.5f),
            Random.Range(-1f, 1f)
        ).normalized;

        rb.AddForce(randomDirection * Random.Range(4f, 8f), ForceMode.Impulse);
        rb.AddTorque(Random.insideUnitSphere * Random.Range(8f, 15f), ForceMode.Impulse);
    }

    int DetermineTopFace(Transform dice)
    {
        Vector3 upDirection = dice.up;

        if (Vector3.Dot(upDirection, Vector3.up) > 0.9f)
            return 2;
        else if (Vector3.Dot(-dice.forward, Vector3.up) > 0.9f)
            return 6;
        else if (Vector3.Dot(dice.right, Vector3.up) > 0.9f)
            return 4;
        else if (Vector3.Dot(-dice.right, Vector3.up) > 0.9f)
            return 3;
        else if (Vector3.Dot(dice.forward, Vector3.up) > 0.9f)
            return 1;
        else if (Vector3.Dot(-dice.up, Vector3.up) > 0.9f)
            return 5;

        return 0;
    }

    IEnumerator ResetDiceAfterDelay(float delay)
    {
        foreach (var item in _diceAndFloor)
        {
            item.SetActive(false);
        }
        yield return new WaitForSeconds(delay);

        playerDice.position = playerStartPos;
        playerDice.rotation = playerStartRot;

        isPlayerThrown = false;
        playerResult = 0;
    }
}