using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    public Image blackScreen;
    public Image progressBar;

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

        while (elapsedTime < 0.5f)
        {
            blackScreen.color = Color.Lerp(new Color(0, 0, 0, 1), new Color(0, 0, 0, 0), elapsedTime * 2);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        blackScreen.color = new Color(0, 0, 0, 0);

        yield return null;

        AsyncOperation loadingOperation = SceneManager.LoadSceneAsync("Menu");
        loadingOperation.allowSceneActivation = false;

        while (!loadingOperation.isDone)
        {
            progressBar.fillAmount = loadingOperation.progress / 0.9f;

            if (loadingOperation.progress >= 0.9f)
            {
                elapsedTime = 0.0f;

                while (elapsedTime < 0.5f)
                {
                    blackScreen.color = Color.Lerp(new Color(0, 0, 0, 0), new Color(0, 0, 0, 1), elapsedTime * 2);
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }
                blackScreen.color = new Color(0, 0, 0, 1);

                loadingOperation.allowSceneActivation = true;
            }

            yield return null;
        }


        yield return null;
    }
}
