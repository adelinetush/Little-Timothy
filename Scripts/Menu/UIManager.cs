using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private MainMenu _mainMenu;
    [SerializeField] private PauseMenu _pauseMenu;
    [SerializeField] private Image _chapterSelect;

    [SerializeField] private Camera _dummyCamera;

    public Events.EventFadeComplete OnMainMenuFadeComplete;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        _mainMenu.OnFadeComplete.AddListener(HandleMainMenuFadeComplete);

        GameManager.Instance.OnGameStateChanged.AddListener(HandleGameStateChanged);
    }

    private void Update()
    {
        if (GameManager.Instance.CurrentGameState == GameManager.GameState.PREGAME)
        {
            if (Input.anyKeyDown)
            {
                GameManager.Instance.StartGame();
            }
        }
    }

    private void HandleMainMenuFadeComplete(bool fadeIn)
    {
        // pass it on
        OnMainMenuFadeComplete.Invoke(fadeIn);
    }

    private void HandleGameStateChanged(GameManager.GameState currentState, GameManager.GameState previousState)
    {
        switch (currentState)
        {
            case GameManager.GameState.PAUSED:
                _pauseMenu.gameObject.SetActive(true);
                break;

            default:
                _pauseMenu.gameObject.SetActive(false);
                break;
        }
    }

    public void SetDummyCameraActive(bool active)
    {
        _dummyCamera.gameObject.SetActive(active);
    }

    public void SelectChapter() {
        _chapterSelect.gameObject.SetActive(true);
        _mainMenu.gameObject.SetActive(false);
    }

    public void SceneOne()
    {

        GameManager.Instance.LoadLevel("Scene 01");
        _chapterSelect.gameObject.SetActive(false);
    }


    public void SceneTwo()
    {
        GameManager.Instance.LoadLevel("Scene 02");
        _chapterSelect.gameObject.SetActive(false);

    }

    public void SceneThree()
    {
        GameManager.Instance.LoadLevel("Scene 03");
        _chapterSelect.gameObject.SetActive(false);

    }

    public void SceneFour()
    {
        GameManager.Instance.LoadLevel("Scene 04");
        _chapterSelect.gameObject.SetActive(false);

    }

    public void SceneFive()
    {
        GameManager.Instance.LoadLevel("Scene 05");
        _chapterSelect.gameObject.SetActive(false);

    }

    public void Restart() {
        GameManager.Instance.RestartLevel();
    }
} 
