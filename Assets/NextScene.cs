using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour
{
    private void Start()
    {
        AudioManager.Instance.FadeInSound("Globe Background Music", 0.5f);
    }

    public void GoToNextScene()
    {
        AudioManager.Instance.FadeOutSound("Globe Background Music", 0.5f);
        SceneManager.LoadScene(1);
    }
}
