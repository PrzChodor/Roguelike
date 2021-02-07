using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class GameMaster : MonoBehaviour
{
    public Image blackScreen;

    private void Start()
    {
        StartCoroutine(Transition());
    }

    public void Exit()
    {
        SceneManager.LoadScene("Menu");
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
        blackScreen.color = new Color(0, 0, 0, 0);

        GetComponent<LevelMaster>().currentLevel.Activate();
        Time.timeScale = 1;
        yield return null;
    }

    public void TogglePause(InputAction.CallbackContext context)
    {
        if (context.started && Time.timeScale == 1)
        {
            Time.timeScale = 0;
            GetComponent<UIManager>().ShowPauseMenu();
            var sources = FindObjectsOfType<AudioSource>();
            foreach (var source in sources)
            {
                if (source.outputAudioMixerGroup.name == "SFX")
                    source.Pause();
            }
        }
        else if (context.started && Time.timeScale == 0)
        {
            HidePause();
            var sources = FindObjectsOfType<AudioSource>();
            foreach (var source in sources)
            {
                if (source.outputAudioMixerGroup.name == "SFX")
                    source.UnPause();
            }
        }
    }

    public void HidePause()
    {
        Time.timeScale = 1;
        GetComponent<UIManager>().HidePauseMenu();
    }
}
