using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    //track the animation component 
    //track the animation clips for fade in/out 
    //functions that can receive animation events 
    //functions to play the fade in/out animations


    [SerializeField] private Animation _mainMenuAnimator;
    [SerializeField] private AnimationClip _fadeOutAnimation;
    [SerializeField] private AnimationClip _fadeInAnimation;
    //[SerializeField] private AnimationClip _pressAnyKey;

    public Events.EventFadeComplete OnFadeComplete;

    private void Start()
    {
        GameManager.Instance.OnGameStateChanged.AddListener(HandleGameStateChanged);

        //_mainMenuAnimator.clip = _pressAnyKey;
        _mainMenuAnimator.Play();
    }

    public void OnFadeOutComplete()
    {
        OnFadeComplete.Invoke(true);
    }

    public void OnFadeInComplete()
    {
        OnFadeComplete.Invoke(false);
        UIManager.Instance.SetDummyCameraActive(true);

        //_mainMenuAnimator.clip = _pressAnyKey;
        _mainMenuAnimator.Play();
    }

    void HandleGameStateChanged(GameManager.GameState currentState, GameManager.GameState previousState)
    {
        if (_mainMenuAnimator == null)
        {
            return;
        }

        if (previousState == GameManager.GameState.PREGAME && currentState != GameManager.GameState.PREGAME)
        {
            UIManager.Instance.SetDummyCameraActive(false);
            _mainMenuAnimator.clip = _fadeOutAnimation;
            _mainMenuAnimator.Play();
        }

        if (previousState != GameManager.GameState.PREGAME && currentState == GameManager.GameState.PREGAME)
        {
            _mainMenuAnimator.Stop();
            _mainMenuAnimator.clip = _fadeInAnimation;
            _mainMenuAnimator.Play();
        }
    }
}
