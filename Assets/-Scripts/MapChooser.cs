using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapChooser : MonoBehaviour
{
    [SerializeField] private GameObject[] _maps;
    [SerializeField] private GameObject _nextButton;

    private void Start()
    {      
        int gamesPlayed = PlayerPrefs.GetInt("GamesPlayed", 0);
        if (gamesPlayed >= 1)
        {
            _maps[1].transform.GetChild(0).gameObject.SetActive(false);
            _maps[1].GetComponent<Button>().interactable = true;
        }
        else
        {
            _maps[1].transform.GetChild(0).gameObject.SetActive(true);
            _maps[1].GetComponent<Button>().interactable = false;
        }

        if (gamesPlayed >= 3)
        {
            _maps[2].transform.GetChild(0).gameObject.SetActive(false);
            _maps[2].GetComponent<Button>().interactable = true;
        }
        else
        {
            _maps[2].transform.GetChild(0).gameObject.SetActive(true);
            _maps[2].GetComponent<Button>().interactable = false;
        }
    }

    public void ChooseMap(int mapIndex)
    {
        foreach (var map in _maps)
        {
            map.transform.GetChild(1).gameObject.SetActive(false);
        }
        _maps[mapIndex].transform.GetChild(1).gameObject.SetActive(true);
        PlayerPrefs.SetInt("ChoosenMap", mapIndex);
        _nextButton.SetActive(true);
    }
}