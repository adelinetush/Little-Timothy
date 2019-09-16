using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInstructions : Singleton<CharacterInstructions>
{
    ActionsManager actionManager;

    AkCallbackManager.EventCallback ec;

    CharacterBehaviour characterBehaviour;

    private void Start()
    {
        

        //REMOVE THIS FOR THE FINAL BUILD
        GetLevelManager();


        //PLAY FOOTSTEPS AUDIO FROM THE START
        AkSoundEngine.SetRTPCValue("Footsteps_Volume", 0);
        AkSoundEngine.PostEvent("Footsteps_Forest", gameObject);

    }

    public void GetLevelManager()
    {
        ec = new AkCallbackManager.EventCallback(new System.Action<object, AkCallbackType, object>((o1, o2, o3) => PlayCueEventCompleted(actionManager.currentAction)));
        Debug.Log("Load level complete is true");

        actionManager = GameObject.Find("LevelManager").GetComponent<ActionsManager>();

        Debug.Log("Found the level Manager");

        characterBehaviour = GetComponent<CharacterBehaviour>();

        characterBehaviour.GetActionsManager();

        PlayInitialCue(actionManager.nextCue, actionManager.currentAction);

    }

    private void Update()
    {
        AkSoundEngine.SetRTPCValue("Speed", CharacterData.Instance.speed);
        
    }

    //private void Update()
    //{
    //    if (GameManager.Instance.loadLevelComplete == true)
    //    {
    //        //GameManager.Instance.loadLevelComplete = false;
    //        Debug.Log("Load level complete is true");

    //        actionManager = GameObject.Find("LevelManager").GetComponent<ActionsManager>();

    //        Debug.Log("Found the level Manager");

    //        PlayInitialCue(actionManager.nextCue, actionManager.currentAction);


    //        if (actionManager.currentAction != "None")
    //        {

    //        }
    //    }

    //}


    /// <summary>
    /// PLEASE LOOK INTO THIS ONTRIGGER. WHAT'S REALLY NECESSARY
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {

        TriggerScript ts = other.GetComponent<TriggerScript>();


        if (ts != null)
        {

            if (ts.action == "")
            {
                if (ts.playFrom != null)
                {
                    AkSoundEngine.PostEvent(ts.eventName, ts.playFrom);
                    //other.gameObject.SetActive(false);
                }
                else
                {
                    AkSoundEngine.PostEvent(ts.eventName, gameObject);
                    //other.gameObject.SetActive(false);
                }

            }

            if (ts.action != "")
            {
                if (ts.eventName != "")
                {
                    AkCallbackManager.EventCallback newActionEvent;
                    newActionEvent = new AkCallbackManager.EventCallback(new System.Action<object, AkCallbackType, object>((o1, o2, o3) => PlayCueEventCompleted(ts.action)));
                    AkSoundEngine.PostEvent(ts.eventName, gameObject, (uint)AkCallbackType.AK_EndOfEvent, newActionEvent, null);
                    //other.gameObject.SetActive(false);
                }
                else
                {
                    PlayCueEventCompleted(ts.action);
                    //other.gameObject.SetActive(false);
                }

            }

            ChangeBehaviourMoveState(ts.movementStatus);
        }

        if (other.gameObject.name == "SceneEnd")
        {
            if (ts != null)
            {
                AkSoundEngine.StopAll(gameObject);
                AkCallbackManager.EventCallback cutscene_endScene = new AkCallbackManager.EventCallback(new System.Action<object, AkCallbackType, object>((o1, o2, o3) => SceneEnd()));
                AkSoundEngine.PostEvent(ts.eventName, gameObject, (uint)AkCallbackType.AK_EndOfEvent, cutscene_endScene, null);
            }
            else {
                SceneEnd();
            }

        }

        if (other.gameObject.name == "Well_To_Forest")
        {
            AkSoundEngine.PostEvent("Well_Forest", gameObject);

            uint eventId;
            eventId = AkSoundEngine.GetIDFromString("Home_To_Well");
            AkSoundEngine.ExecuteActionOnEvent(eventId, AkActionOnEventType.AkActionOnEventType_Stop, gameObject, 0, AkCurveInterpolation.AkCurveInterpolation_Linear);
        }

        if (other.gameObject.name == "JumpTheTree")
        {
            GameObject bootMusic = GameObject.Find("Boot Music");

            uint eventId;
            eventId = AkSoundEngine.GetIDFromString("global_bg");
            AkSoundEngine.ExecuteActionOnEvent(eventId, AkActionOnEventType.AkActionOnEventType_Stop, bootMusic, 5, AkCurveInterpolation.AkCurveInterpolation_Linear);


            AkCallbackManager.EventCallback pauseBgMusic = new AkCallbackManager.EventCallback(new System.Action<object, AkCallbackType, object>((o1, o2, o3) => ResumeBGMusic()));
            AkSoundEngine.PostEvent("tree_fall_music", bootMusic, (uint)AkCallbackType.AK_EndOfEvent, pauseBgMusic, null);
        }

        if (other.gameObject.name == "WellMusic")
        {
            GameObject bootMusic = GameObject.Find("Boot Music");

            uint eventId;
            eventId = AkSoundEngine.GetIDFromString("global_bg");
            AkSoundEngine.ExecuteActionOnEvent(eventId, AkActionOnEventType.AkActionOnEventType_Stop, bootMusic, 5000, AkCurveInterpolation.AkCurveInterpolation_Linear);


            AkCallbackManager.EventCallback pauseBgMusic = new AkCallbackManager.EventCallback(new System.Action<object, AkCallbackType, object>((o1, o2, o3) => ResumeBGMusic()));
            AkSoundEngine.PostEvent("well_music", bootMusic, (uint)AkCallbackType.AK_EndOfEvent, pauseBgMusic, null);
        }

        if (other.gameObject.name == "MusicFour")
        {
            GameObject bootMusic = GameObject.Find("Boot Music");

            uint eventId;
            eventId = AkSoundEngine.GetIDFromString("global_bg");
            AkSoundEngine.ExecuteActionOnEvent(eventId, AkActionOnEventType.AkActionOnEventType_Stop, bootMusic, 5000, AkCurveInterpolation.AkCurveInterpolation_Linear);


            AkCallbackManager.EventCallback pauseBgMusic = new AkCallbackManager.EventCallback(new System.Action<object, AkCallbackType, object>((o1, o2, o3) => ResumeBGMusic()));
            AkSoundEngine.PostEvent("cutscene_four_music", bootMusic, (uint)AkCallbackType.AK_EndOfEvent, pauseBgMusic, null);
        }

        if (other.gameObject.name == "Forest_Chase")
        {
            //AkSoundEngine.PostEvent("Forest_Chase", gameObject);

            uint eventId;
            eventId = AkSoundEngine.GetIDFromString("Well_Forest");
            AkSoundEngine.ExecuteActionOnEvent(eventId, AkActionOnEventType.AkActionOnEventType_Stop, gameObject, 5000, AkCurveInterpolation.AkCurveInterpolation_Linear);
        }


        if (other.gameObject.name == "SwingStart")
        {

            SwingHandler sh = other.GetComponent<SwingHandler>();
            AnimationEventScript animEvents = sh.swing.GetComponent<AnimationEventScript>();
            if (animEvents != null)
            {
                animEvents.canListenForEvents = true;
            }

            //other.gameObject.SetActive(false);
        }

        if (other.gameObject.name == "SwingEnd")
        {
            //other.gameObject.SetActive(false);
            SwingHandler sh = other.GetComponent<SwingHandler>();
            AnimationEventScript animEvents = sh.swing.GetComponent<AnimationEventScript>();
            if (animEvents != null)
            {
                animEvents.canListenForEvents = false;
                AkSoundEngine.PostEvent("Move_On", gameObject);
            }
        }

        other.gameObject.SetActive(false);

    }
    public void ResumeBGMusic() {
        GameObject bootMusic = GameObject.Find("Boot Music");
        AkSoundEngine.PostEvent("global_bg", bootMusic);


    }

    public void StartBackgroundMusic() {
        GameObject bootMusic = GameObject.Find("Boot Music");
        //if (bootMusic != null)
        //{

        //}
        AkSoundEngine.PostEvent("global_bg", bootMusic);
    }


    public void IncompleteAction(string eventName, bool complete)
    {

        //if bool is true, POST EVENT 
        if (complete == true)
        {
            AkSoundEngine.PostEvent(eventName, gameObject);
        }

        if (complete == false)
        {

            uint eventId;
            eventId = AkSoundEngine.GetIDFromString(eventName);
            AkSoundEngine.ExecuteActionOnEvent(eventId, AkActionOnEventType.AkActionOnEventType_Stop, gameObject, 0, AkCurveInterpolation.AkCurveInterpolation_Linear);

        }

        //if bool is false, STOP EVENT
    }

    void PlayInitialCue(string cue, string nextAction)
    {
        if (cue != "None" && nextAction == "None")
        {
            AkSoundEngine.PostEvent(cue, gameObject);
        }

        if (nextAction != "None")
        {
            //do stuff with the action
            AkSoundEngine.PostEvent(cue, gameObject, (uint)AkCallbackType.AK_EndOfEvent, ec, null);

        }

    }
    //SEND ACTION TO THE CHARACTER BEHAVIOUR
    void PlayCueEventCompleted(string nextAction)
    {
        //the actions that will play
        characterBehaviour.ReceiveAction(nextAction);

    }

    //FOR SINGLE ACTIONS
    public void PlayEventFeedback(string feedback)
    {
        AkSoundEngine.PostEvent(feedback, gameObject);

    }

    //FOR ACTIONS THAT AFFECT MOVEMENT STATE
    public void PlayFeedbackChangeState(string feedback, CharacterData.MovementStats.MovementState state)
    {
        AkCallbackManager.EventCallback changeMovementStateCallback = new AkCallbackManager.EventCallback(new System.Action<object, AkCallbackType, object>((o1, o2, o3) => ChangeBehaviourMoveState(state)));
        AkSoundEngine.PostEvent(feedback, gameObject, (uint)AkCallbackType.AK_EndOfEvent, changeMovementStateCallback, null);

    }

    public void ChangeBehaviourMoveState(CharacterData.MovementStats.MovementState state)
    {

        characterBehaviour.SendMessage("MoveState", state);
    }

    //FOR MULTIPLE ACTIONS
    public void ActionCompleteFeedback(string feedback, bool levelEnd)
    {

        if (actionManager.actionCounter <= actionManager.possibleActions.Length - 1)
        {
            actionManager.actionCounter += 1;
        }

        if (levelEnd == true)
        {
            AkCallbackManager.EventCallback actionCompleteCallback = new AkCallbackManager.EventCallback(new System.Action<object, AkCallbackType, object>((o1, o2, o3) => SceneEnd()));
            AkSoundEngine.PostEvent(feedback, gameObject, (uint)AkCallbackType.AK_EndOfEvent, actionCompleteCallback, null);
        }
        if (levelEnd == false)
        {
            AkCallbackManager.EventCallback endSceneCallback = new AkCallbackManager.EventCallback(new System.Action<object, AkCallbackType, object>((o1, o2, o3) => MoreActions()));
            AkSoundEngine.PostEvent(feedback, gameObject, (uint)AkCallbackType.AK_EndOfEvent, endSceneCallback, null);
        }

    }


    public void MoreActions()
    {

        if (actionManager.possibleActions.Length >= actionManager.actionCounter + 1)
        {
            //There are pending actions
            //actionManager.actionCounter += 1;

            AkSoundEngine.PostEvent(actionManager.currentAction, gameObject);
            characterBehaviour.ReceiveAction(actionManager.currentAction);

        }
        else
        {
            return;
        }
    }

    void SceneEnd()
    {
        Debug.Log("This is the end of the scene");

        GameManager.Instance.LoadNewScene(actionManager.currentScene, actionManager.nextScene);
        actionManager = null;

    }

    public void PlayDeathTrack(string track) {
        AkSoundEngine.StopAll(gameObject);
        AkCallbackManager.EventCallback playerDeathTrack = new AkCallbackManager.EventCallback(new System.Action<object, AkCallbackType, object>((o1, o2, o3) => PlayerDead()));
        AkSoundEngine.PostEvent(track, gameObject, (uint)AkCallbackType.AK_EndOfEvent, playerDeathTrack, null);
    }

    void PlayerDead() {
        GameManager.Instance.LoadNewScene(actionManager.currentScene, actionManager.currentScene);
    }

    public void WaitForFeedback(string firstTrack, string secondTrack) {
        AkCallbackManager.EventCallback playTrackThenFeedback = new AkCallbackManager.EventCallback(new System.Action<object, AkCallbackType, object>((o1, o2, o3) => PlayNextTrack(secondTrack)));
        AkSoundEngine.PostEvent(firstTrack, gameObject, (uint)AkCallbackType.AK_EndOfEvent, playTrackThenFeedback, null);
    }

    public void PlayNextTrack(string track) {
        AkSoundEngine.PostEvent(track, gameObject);
        if (track == "Keep_Moving")
        {
            characterBehaviour.SendMessage("MoveState", CharacterData.MovementStats.MovementState.TROTTING);
        }
    }

    /// <summary>
    /// MOVEMENT AUDIO
    /// </summary>
    public void PlayMovementAudio()
    {
        AkSoundEngine.SetRTPCValue("Footsteps_Volume", 100);
    }

    public void StopMovementAudio()
    {
        AkSoundEngine.SetRTPCValue("Footsteps_Volume", 0);
    }

    public void PlayJumpAudio() {
        AkSoundEngine.PostEvent("Jump_SFX", gameObject);
    }

    public void PlayLandAudio()
    {
        AkSoundEngine.PostEvent("Land_SFX", gameObject);
    }

}
