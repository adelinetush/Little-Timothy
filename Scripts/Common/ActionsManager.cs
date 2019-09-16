using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionsManager : MonoBehaviour
{
    public string currentScene, nextScene;

    public float verticalVelocity;
    public float gravity;

    public CharacterData.MovementStats.MovementState MovementState;
    public bool canRun;

    public string nextCue;
    public string[] possibleActions; // <---- the actions that are not triggered by collision

    public string currentAction;
    public int actionCounter;


    private void Start()
    {
        actionCounter = 0;
        if (possibleActions.Length > 0)
        {

            currentAction = possibleActions[actionCounter];
        }
        else {
            currentAction = "None";
        }
        
    }

    private void Update()
    {
        if (actionCounter <= possibleActions.Length -1)
        {
            if (currentAction != possibleActions[actionCounter])
            {
                currentAction = possibleActions[actionCounter];
                Debug.Log("Current Action" + currentAction);
            }
        }

    }
}
