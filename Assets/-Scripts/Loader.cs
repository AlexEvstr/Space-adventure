using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loader : MonoBehaviour
{
    private string _sceneName = "MenuScene";

    private void Start()
    {
        StartCoroutine(OpenMenu());
    }

    IEnumerator OpenMenu()
    {
        yield return new WaitForSeconds(2.5f);
        SceneManager.LoadScene(_sceneName);
    }
}
