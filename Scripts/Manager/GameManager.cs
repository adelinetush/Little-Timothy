using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.Events;

public class GameManager : Singleton<GameManager>
{
    //load and unload levels 
    //what level is the game currently in 
    //keep track of the game state 
    //generate other persistent systems

    //PREGAME, RUNNING, PAUSED
    public enum GameState
    {
        PREGAME,
        RUNNING,
        PAUSED
    }

    public GameObject[] SystemPrefabs;

    public GameObject player, movieCamera;

    public Events.EventGameState OnGameStateChanged;

    List<AsyncOperation> _loadOperations;
    List<GameObject> _instancedSystemPrefabs;
    GameState _currentGameState = GameState.PREGAME;

    string _currentLevelName;

    CharacterBehaviour characterBehaviour;

    public GameState CurrentGameState
    {
        get { return _currentGameState; }
        private set { _currentGameState = value; }
    }

    void Start()
    {
        DontDestroyOnLoad(gameObject);

        _instancedSystemPrefabs = new List<GameObject>();
        _loadOperations = new List<AsyncOperation>();

        InstantiateSystemPrefabs();

        UIManager.Instance.OnMainMenuFadeComplete.AddListener(HandleMainMenuFadeComplete);

        OnGameStateChanged.Invoke(GameState.PREGAME, _currentGameState);
    }

    void Update()
    {
        if (_currentGameState == GameState.PREGAME)
        {
            return;
        }

        if (Input.GetButtonUp("Back"))
        {
            TogglePause();
        }
    }

    void OnLoadOperationComplete(AsyncOperation ao)
    {
        if (_loadOperations.Contains(ao))
        {
            _loadOperations.Remove(ao);

            if (_loadOperations.Count == 0)
            {
                UpdateState(GameState.RUNNING);

                //THIS IS TEMPORARY TO SEE IF IT REALLY WAITF FOR LOAD TO FINISH

                characterBehaviour = player.GetComponent<CharacterBehaviour>();


                //FIND THE LEVEL MANAGER FROM EACH SCENE WHEN IT HAS FINISHED LOADING
                if (CharacterInstructions.Instance != null)
                {
                    CharacterInstructions.Instance.GetLevelManager();
                }
                else {
                    return;
                }
                
                characterBehaviour.GetActionsManager();

            }
        }

        //WAS ORIGINALLY HERE
        
    }

    void OnUnloadOperationComplete(AsyncOperation ao)
    {
        // Clean up level is necessary, go back to main menu
        characterBehaviour.RemoveActionsManager();
    }

    void UpdateState(GameState state)
    {
        GameState previousGameState = _currentGameState;
        _currentGameState = state;

        switch (CurrentGameState)
        {
            case GameState.PREGAME:
                // Initialize any systems that need to be reset
                Time.timeScale = 1.0f;
                break;

            case GameState.RUNNING:
                //  Unlock player, enemies and input in other systems, update tick if you are managing time
                Time.timeScale = 1.0f;
                break;

            case GameState.PAUSED:
                // Pause player, enemies etc, Lock other input in other systems
                //AkSoundEngine.StopAll();
                Time.timeScale = 0.0f;
                break;

            default:
                break;
        }

        OnGameStateChanged.Invoke(_currentGameState, previousGameState);
    }

    void InstantiateSystemPrefabs()
    {
        GameObject prefabInstance;
        for (int i = 0; i < SystemPrefabs.Length; ++i)
        {
            prefabInstance = Instantiate(SystemPrefabs[i]);
            _instancedSystemPrefabs.Add(prefabInstance);
        }
    }

    void HandleMainMenuFadeComplete(bool fadeIn)
    {
        if (!fadeIn)
        {
            UnloadLevel(_currentLevelName);
        }
    }

    public void LoadLevel(string levelName)
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Additive);
        if (ao == null)
        {
            Debug.LogError("[GameManager] Unable to load level " + levelName);
            return;
        }

        ao.completed += OnLoadOperationComplete;
        _loadOperations.Add(ao);

        _currentLevelName = levelName;

        Debug.Log(_currentGameState);

    }

    public void UnloadLevel(string levelName)
    {
        AkSoundEngine.StopAll(GameObject.Find("Character"));
        AsyncOperation ao = SceneManager.UnloadSceneAsync(levelName);
        ao.completed += OnUnloadOperationComplete;
    }

    public void TogglePause()
    {
        UpdateState(_currentGameState == GameState.RUNNING ? GameState.PAUSED : GameState.RUNNING);
        
    }

    public void RestartGame()
    {
        AkSoundEngine.StopAll(player);
        UpdateState(GameState.PREGAME);
    }

    public void RestartLevel() {
        AkSoundEngine.StopAll(player);
        UnloadLevel(_currentLevelName);
        LoadLevel(_currentLevelName);

    }

    public void StartGame()
    {
        LoadLevel("Scene 01");
        Destroy(movieCamera.gameObject);
        //Instantiate(player);
        
    }

    public void PlayerDead() {
        UpdateState(GameState.PREGAME);
    }

    public void LoadNewScene(string currentScene, string nextScene) {

        //TEMPORARY FOR CHAPTER 01
        //REMOVE AFTER TESTING

        if (currentScene == "Scene 05")
        {
            Debug.Log("We are at the end of scene 5");
            UpdateState(GameState.PREGAME);
        }
        else {
            UnloadLevel(currentScene);

            LoadLevel(nextScene);
        }

    }

    public void QuitGame()
    {
        // Clean up application as necessary
        // Maybe save the players game

        Debug.Log("[GameManager] Quit Game.");

        Application.Quit();
    }

}
