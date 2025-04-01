using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlayerSelectManager : MonoBehaviour
{
    public static PlayerSelectManager Instance;

    public Sprite[] playerSprites;
    public Sprite[] colorSprites;
    public Sprite playerEmpty;

    public Button nextButton;

    private List<PlayerSlotController> slots = new List<PlayerSlotController>();

    private void Awake()
    {
        Instance = this;
    }

    public void RegisterSlot(PlayerSlotController slot)
    {
        if (!slots.Contains(slot))
            slots.Add(slot);
    }

    public void UpdateNextButton()
    {
        int activePlayers = 0;

        foreach (var slot in slots)
        {
            if (slot.HasValidName())
                activePlayers++;
        }

        nextButton.gameObject.SetActive(activePlayers >= 2);

    }

    public Sprite GetPlayerSprite(int index)
    {
        return playerSprites[index];
    }

    public Sprite GetColorSprite(int index)
    {
        return colorSprites[index];
    }

    public void SwapColors(PlayerSlotController from, int newIndex)
    {
        foreach (var slot in slots)
        {
            if (slot == from) continue;
            if (!slot.IsActive()) continue; // ⬅️ добавлено: только активные

            if (slot.CurrentColorIndex == newIndex)
            {
                int oldIndex = from.CurrentColorIndex;
                slot.SetColor(oldIndex);
                break;
            }
        }

        from.SetColor(newIndex);
        UpdateAllButtonStates();
    }


    public void CloseAllColorPanelsExcept(PlayerSlotController caller)
    {
        foreach (var slot in slots)
        {
            if (slot != caller)
                slot.HideColorPanel();
        }
    }

    public void UpdateAllButtonStates()
    {
        foreach (var slot in slots)
        {
            slot.UpdateButtonStates();
        }
    }

}
