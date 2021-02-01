using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    public Image blackScreen;

    private void Start()
    {
        Invoke("LoadMenu", 0.1f);
    }

    private void LoadMenu()
    {
        StartCoroutine(LoadMenuCoroutine());
    }

    IEnumerator LoadMenuCoroutine()
    {
        float elapsedTime = 0.0f;

        while (elapsedTime < 1.0f)
        {
            blackScreen.color = Color.Lerp(new Color(0, 0, 0, 1), new Color(0, 0, 0, 0), elapsedTime);
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }
        blackScreen.color = new Color(0, 0, 0, 0);

        yield return null;

        AsyncOperation loadingOperation = SceneManager.LoadSceneAsync("Menu");
        while (!loadingOperation.isDone)
        {
            yield return null;
        }

        elapsedTime = 0.0f;

        while (elapsedTime < 1.0f)
        {
            blackScreen.color = Color.Lerp(new Color(0, 0, 0, 0), new Color(0, 0, 0, 1), elapsedTime);
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }
        blackScreen.color = new Color(0, 0, 0, 1);

        yield return null;
    }
}
