using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour
{
    public Image blackScreen;

    private void Start()
    {
        StartCoroutine(Transition());
    }

    public void Exit()
    {
#if UNITY_STANDALONE
        Application.Quit();
#endif

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    IEnumerator Transition()
    {
        Time.timeScale = 0;

        float elapsedTime = 0.0f;

        while (elapsedTime < 1.0f)
        {
            blackScreen.color = Color.Lerp(new Color(0, 0, 0, 1), new Color(0, 0, 0, 0), elapsedTime);
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }

        Time.timeScale = 1;
        yield return null;
    }
}
