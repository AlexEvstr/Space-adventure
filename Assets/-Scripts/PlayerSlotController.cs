using UnityEngine;
using UnityEngine.UI;

public class PlayerSlotController : MonoBehaviour
{
    private Image slotImage;
    private Transform colorPanel;
    public Button colorButton; // Кнопка смены цвета (привяжи в инспекторе)
    private InputField inputField;

    public int CurrentColorIndex { get; private set; } = -1;

    private void Start()
    {
        slotImage = GetComponent<Image>();
        inputField = transform.GetChild(3).GetComponent<InputField>();
        colorPanel = transform.GetChild(4);
        PlayerSelectManager.Instance.RegisterSlot(this);
        ResetSlot();
    }

    public void OnAddPlayer()
    {
        int myIndex = transform.GetSiblingIndex();

        if (myIndex > 0)
        {
            var parent = transform.parent;
            if (parent.childCount > myIndex - 1)
            {
                var prevSlot = parent.GetChild(myIndex - 1).GetComponent<PlayerSlotController>();
                if (prevSlot == null || !prevSlot.IsActive())
                {
                    Debug.Log($"Сначала добавьте игрока {myIndex}");
                    return;
                }
            }
        }


        int index = FindFreeColorIndex();
        if (index == -1) index = 0; // если всё занято, берём 0

        CurrentColorIndex = index;
        UpdateVisuals();

        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(true);
        transform.GetChild(2).gameObject.SetActive(true);
        transform.GetChild(3).gameObject.SetActive(true);
        colorPanel.gameObject.SetActive(false);

        PlayerSelectManager.Instance.UpdateNextButton();
        PlayerSelectManager.Instance.UpdateAllButtonStates();
    }

    public void OnColorButton()
    {
        PlayerSelectManager.Instance.CloseAllColorPanelsExcept(this);
        colorPanel.gameObject.SetActive(true);

        for (int i = 0; i < 4; i++)
        {
            Image img = colorPanel.GetChild(i).GetComponent<Image>();
            img.sprite = PlayerSelectManager.Instance.GetColorSprite(i);
        }
    }


    public void OnSelectColor(int newIndex)
    {
        PlayerSelectManager.Instance.SwapColors(this, newIndex);
    }

    public void SetColor(int index)
    {
        CurrentColorIndex = index;
        UpdateVisuals();
        colorPanel.gameObject.SetActive(false);
    }

    private void UpdateVisuals()
    {
        PlayerPrefs.SetInt($"PlayerSpriteIndex{transform.GetSiblingIndex()}", CurrentColorIndex);
        slotImage.sprite = PlayerSelectManager.Instance.GetPlayerSprite(CurrentColorIndex);
        colorButton.image.sprite = PlayerSelectManager.Instance.GetColorSprite(CurrentColorIndex);

    }

    public void OnDelete()
    {
        int myIndex = transform.GetSiblingIndex();

        // Запретить удаление, если следующий активен
        if (myIndex < 3) // ограничено 4 слотами
        {
            var nextSlot = transform.parent.GetChild(myIndex + 1).GetComponent<PlayerSlotController>();
            if (nextSlot.IsActive())
            {
                Debug.Log($"Сначала удалите игрока {myIndex + 1}");
                return;
            }
        }

        slotImage.sprite = PlayerSelectManager.Instance.playerEmpty;
        CurrentColorIndex = -1;

        transform.GetChild(0).gameObject.SetActive(true);
        for (int i = 1; i <= 4; i++)
            transform.GetChild(i).gameObject.SetActive(false);

        inputField.text = "";
        PlayerPrefs.DeleteKey($"PlayerSpriteIndex{myIndex}");
        PlayerPrefs.DeleteKey($"PlayerName{myIndex}");

        PlayerSelectManager.Instance.UpdateNextButton();
        PlayerSelectManager.Instance.UpdateAllButtonStates();
    }

    public void OnNameChanged()
    {
        PlayerPrefs.SetString($"PlayerName{transform.GetSiblingIndex()}", inputField.text);
        PlayerSelectManager.Instance.UpdateNextButton();
        PlayerSelectManager.Instance.UpdateAllButtonStates();
    }

    public bool HasValidName()
    {
        return inputField != null && !string.IsNullOrWhiteSpace(inputField.text);
    }

    private int FindFreeColorIndex()
    {
        // можно выбрать любой, но для начала выберем свободный
        for (int i = 0; i < 4; i++)
        {
            bool taken = false;
            foreach (var other in FindObjectsOfType<PlayerSlotController>())
            {
                if (other != this && other.CurrentColorIndex == i)
                    taken = true;
            }
            if (!taken)
                return i;
        }
        return 0;
    }

    private void ResetSlot()
    {
        OnDelete();
    }

    public void HideColorPanel()
    {
        colorPanel.gameObject.SetActive(false);
    }

    public bool IsActive()
    {
        return CurrentColorIndex != -1;
    }

    public void UpdateButtonStates()
    {
        int myIndex = transform.GetSiblingIndex();

        // === ЛОГИКА AddPlayer ===
        bool canAdd = !IsActive() && (myIndex == 0);
        if (myIndex > 0)
        {
            var prevSlot = transform.parent.GetChild(myIndex - 1).GetComponent<PlayerSlotController>();
            canAdd = !IsActive() && prevSlot != null && prevSlot.IsActive();
        }
        transform.GetChild(0).gameObject.SetActive(canAdd);

        // === ЛОГИКА Delete ===
        bool canDelete = IsActive();
        if (canDelete && myIndex < transform.parent.childCount - 1)
        {
            var nextSlot = transform.parent.GetChild(myIndex + 1).GetComponent<PlayerSlotController>();
            if (nextSlot != null && nextSlot.IsActive())
                canDelete = false;
        }
        transform.GetChild(2).gameObject.SetActive(canDelete);
    }


}
