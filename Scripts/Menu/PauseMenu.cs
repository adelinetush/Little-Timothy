using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    //[SerializeField] private Button ResumeButton;
    [SerializeField] private Button QuitButton;
    //[SerializeField] private Button RestartButton;

    private void Awake()
    {
        //ResumeButton.onClick.AddListener(HandleResumeClicked);
        QuitButton.onClick.AddListener(HandleQuitClicked);
        //RestartButton.onClick.AddListener(HandleRestartClicked);
    }

    void HandleResumeClicked()
    {
        GameManager.Instance.TogglePause();
    }

    void HandleQuitClicked()
    {
        GameManager.Instance.QuitGame();
    }

    void HandleRestartClicked()
    {
        GameManager.Instance.RestartGame();
    }

}
