using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class RebindKey : MonoBehaviour
{
    public InputActionReference inputAction;
    public int inputBinding;
    public GameObject rebindOverlay;
    public Image keyIcon;
    public PlayerInput player;

    private void Start()
    {
        var icon = Resources.Load<Sprite>($"KeyIcons/{inputAction.action.GetBindingDisplayString(inputBinding).Replace("\\", "BackSlash").Replace(".", "Dot").Replace("/", "Slash")}");
        if (icon != null)
            keyIcon.sprite = icon;
    }

    public void Rebind()
    {
        GetComponent<AudioSource>().Play();
        var again = false;
        inputAction.action.Disable();
        rebindOverlay.SetActive(true);
        var rebindOperation = inputAction.action.PerformInteractiveRebinding(inputBinding)
            .WithCancelingThrough("<Keyboard>/escape")
            .Start();

        rebindOperation.OnPotentialMatch(
            operation =>
            {
                var icon = Resources.Load<Sprite>($"KeyIcons/{operation.candidates[0].displayName.Replace("\\", "BackSlash").Replace(".", "Dot").Replace("/", "Slash")}");
                if (icon == null)
                {
                    again = true;
                    operation.Cancel();
                }
                else
                {
                    keyIcon.sprite = icon;
                }
            });

        rebindOperation.OnComplete(
            operation =>
            {
                rebindOverlay.SetActive(false);
                operation.Dispose();

                var rebinds = player.actions.SaveBindingOverridesAsJson();
                PlayerPrefs.SetString("rebinds", rebinds);
                inputAction.action.Enable();
            });

        rebindOperation.OnCancel(
            operation =>
            {
                rebindOverlay.SetActive(false);
                operation.Dispose();
                inputAction.action.Enable();
                if (again)
                    Rebind();
            });
    }
}
