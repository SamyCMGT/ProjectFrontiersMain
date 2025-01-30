using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneLoader : MonoBehaviour
{
    public int SceneIndexG = 0;
    public int UnityIndex = 0;
    public void LoadScene(int pSceneIndex)
    {
        Debug.Log("Loading");
        SceneManager.LoadScene(pSceneIndex);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting");
        Application.Quit();
    }
    private void OnTriggerEnter(Collider other)
    {
        SceneManager.LoadScene(SceneIndexG);
    }
}