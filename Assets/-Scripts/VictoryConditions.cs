using UnityEngine;

public class VictoryConditions : MonoBehaviour
{
    [SerializeField] private GameObject[] _doneMars;

    public void ChooseVictoryConditions(int index)
    {
        foreach (var item in _doneMars)
        {
            item.SetActive(false);
        }
        _doneMars[index].SetActive(true);
        PlayerPrefs.SetInt("VictoryConditions", index);
    }
}
