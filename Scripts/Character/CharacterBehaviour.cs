using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBehaviour : MonoBehaviour
{
    CharacterData characterData;

    TriggerScript tScript;

    ActionsManager actionsManager;

    CharacterHealth cHealth;

    bool canFirstJump, canSecondJump, canRun, canWalk, canPickJerrycan, openDoor, canDropJerrycan, jumpingTrunks, pickStick, clearFirstPath, otherPaths, pickMango, canFloat, stayOnLog, pullBranch, crawling, canGetUp;

    string currentInstruction;

    public CharacterData.MovementStats.MovementState CurrentMovementState
    {
        get { return characterData._currentMovementState; }
        set { characterData._currentMovementState = value; }

    }

    int clearPathCount;

    private void Awake()
    {
        clearPathCount = 0;

        //ACTIONS STATUS AT THE START OF THE GAME
        //DO I REALLY NEED TO DO THIS?
        canFirstJump = false;
        canSecondJump = false;
        canRun = false;
        canWalk = false;
        canPickJerrycan = false;
        openDoor = false;
        canDropJerrycan = false;
        jumpingTrunks = false;
        pickStick = false;
        clearFirstPath = false;
        otherPaths = false;
        pickMango = false;
        canGetUp = false;

        Debug.Log(canGetUp);

        GetActionsManager();
    }

    public void GetActionsManager()
    {
        characterData = CharacterData.Instance;

        actionsManager = GameObject.Find("LevelManager").GetComponent<ActionsManager>();

        if (actionsManager != null && actionsManager.possibleActions.Length > 0)
        {
            Debug.Log(actionsManager.possibleActions[0]);
        }
        else
        {
            Debug.Log("Actions Manager is NOT available || there are no actions");
        }


        characterData.maxHealth = 100;
        characterData.currentHealth = 100;

        characterData.baseResistance = 1;
        characterData.currentResistance = 0;

        characterData.outOfBreath = false;

        characterData.verticalVelocity = actionsManager.verticalVelocity;
        //characterData.gravity = actionsManager.gravity;
        MoveState(actionsManager.MovementState);

        cHealth = GetComponent<CharacterHealth>();

    }

    public void RemoveActionsManager()
    {
        actionsManager = null;
    }

    private void Update()
    {

        if (canFirstJump == true && Input.GetButtonDown("Jump"))
        {
            canFirstJump = false;
            CharacterInstructions.Instance.IncompleteAction(currentInstruction, false);
            //characterData.verticalVelocity = 0.0f;
            actionsManager.actionCounter += 1;
            CharacterInstructions.Instance.ActionCompleteFeedback("Jump_Feedback_01", false);
        }

        if (canSecondJump == true && Input.GetButtonDown("Jump"))
        {
            canSecondJump = false;
            CharacterInstructions.Instance.IncompleteAction(currentInstruction, false);
            //characterData.verticalVelocity = 0.0f;
            actionsManager.actionCounter += 1;
            CharacterInstructions.Instance.ActionCompleteFeedback("Jump_Feedback_02", false);
        }

        //RUNNING
        if (canRun == true)
        {
            if (Input.GetButtonDown("Run"))
            {
                UpdateMovementState(CharacterData.MovementStats.MovementState.RUNNING);
            }
             
            if (Input.GetButtonUp("Run"))
            {
                UpdateMovementState(CharacterData.MovementStats.MovementState.TROTTING);
            }
        }

        //CRAWLING
        if (crawling == true)
        {
            if (Input.GetButtonDown("Crouch") || Input.GetKeyDown("CrouchKeyboard"))
            {
                UpdateMovementState(CharacterData.MovementStats.MovementState.CRAWLING);
            }

            if (Input.GetButtonUp("Crouch") || Input.GetKeyUp("CrouchKeyboard"))
            {
                UpdateMovementState(CharacterData.MovementStats.MovementState.STANDING);
            }
        }

        //OPEN DOOR
        if (openDoor == true && Input.GetButton("Action"))
        {
            openDoor = false;
            CharacterInstructions.Instance.IncompleteAction("Open_Door", false);
            actionsManager.actionCounter += 1;
            CharacterInstructions.Instance.ActionCompleteFeedback("door_open_close", true);
            CharacterInstructions.Instance.StartBackgroundMusic();
            if (actionsManager.currentScene == "Scene 14")
            {
                MoveState(CharacterData.MovementStats.MovementState.CRAWLING);
            }
        }

        //DROP JERRYCAN
        if (canDropJerrycan == true && Input.GetButton("Action"))
        {
            canDropJerrycan = false;
            CharacterInstructions.Instance.IncompleteAction("Drop_Jerrycan", false);
            CharacterInstructions.Instance.PlayEventFeedback("Drop_Jerry_Feedback");
            UpdateMovementState(CharacterData.MovementStats.MovementState.TROTTING);
            //Play jerrycan sound
        }

        //JUMPING TRACKS
        if (jumpingTrunks == true && Input.GetButtonDown("Jump"))
        {
            jumpingTrunks = false;
            CharacterInstructions.Instance.IncompleteAction("Jump_Tree_Action", false);
        }

        //PICK STICK
        if (pickStick == true && Input.GetButtonDown("Action"))
        {
            pickStick = false;
            CharacterInstructions.Instance.IncompleteAction("Pick_Stick", false);
            CharacterInstructions.Instance.PlayEventFeedback("Break_Stick");
            //Play break stick sound
            ReceiveAction("PressB_Repeatedly");
        }

        //CLEARPATH
        if (clearFirstPath == true && Input.GetButtonDown("Action"))
        {
            clearPathCount += 1;
            CharacterInstructions.Instance.PlayEventFeedback("Slash");
            if (clearPathCount == 3)
            {
                CharacterInstructions.Instance.WaitForFeedback("Samurai", "Keep_Moving");
               
            }
            if (clearPathCount > 9)
            {
                clearFirstPath = false;
                clearPathCount = 0;
                //UpdateMovementState(CharacterData.MovementStats.MovementState.TROTTING);

            }
        }

        //CLEAR OTHER PATHS
        if (otherPaths == true && Input.GetButtonDown("Action"))
        {
            CharacterInstructions.Instance.PlayEventFeedback("Slash");
            clearPathCount += 1;
            if (clearPathCount > 9)
            {
                otherPaths = false;
                clearPathCount = 0;
                UpdateMovementState(CharacterData.MovementStats.MovementState.WALKING);
                CharacterInstructions.Instance.PlayEventFeedback("Keep_Walking");
            }
        }

        //PICK MANGO
        if (pickMango == true && Input.GetButtonDown("Action"))
        {
            pickMango = false;
            CharacterInstructions.Instance.IncompleteAction("Pick_Mango", false);
            CharacterInstructions.Instance.WaitForFeedback("Mango_Feedback", "Keep_Moving");
            //CharacterInstructions.Instance.PlayFeedbackChangeState("Keep_Moving", CharacterData.MovementStats.MovementState.TROTTING);

        }

        if (canGetUp == true && Input.GetButtonDown("Action"))
        {
            canGetUp = false;
            if (actionsManager.currentScene == "Scene 13")
            {
                actionsManager.actionCounter += 1;
                Debug.Log(actionsManager.actionCounter);
                CharacterInstructions.Instance.ActionCompleteFeedback("Stop_Shouting", false);
            }

            if (actionsManager.currentScene == "Scene 14")
            {
                CharacterInstructions.Instance.ActionCompleteFeedback("Falling_Branches", false);
            }
        }

        //PULL BRANCH
        if (pullBranch == true && Input.GetButtonDown("Action"))
        {
            pullBranch = false;
            canRun = true;
            CharacterInstructions.Instance.PlayFeedbackChangeState("Bee_Hive", CharacterData.MovementStats.MovementState.WALKING);

        }

        if (canWalk == true && (Input.GetAxis("Horizontal") > 0 || Input.GetAxis("Vertical") > 0))
        {
            canWalk = false;
            CharacterInstructions.Instance.IncompleteAction(currentInstruction, false);
            actionsManager.actionCounter += 1;
            CharacterInstructions.Instance.ActionCompleteFeedback("Walk_Feedback", false);
        }


        if (canPickJerrycan == true && Input.GetButtonDown("Action"))
        {
            if (actionsManager != null)
            {
                Debug.Log("Character data is available");
            }
            else
            {
                Debug.Log("Character data is NOT available");
            }
            canPickJerrycan = false;
            CharacterInstructions.Instance.IncompleteAction("Pick_Jerrycan", false);
            //Debug.Log(actionsManager.currentAction);
            actionsManager.actionCounter += 1;
            Debug.Log(actionsManager.actionCounter);
            CharacterInstructions.Instance.ActionCompleteFeedback("Pick_Jerry_Feedback", false);
        }

        //GOING UP FOR AIR
        if (canFloat == true && Input.GetButtonDown("Action"))
        {
            characterData.upWardForce = 80.0f;
            characterData.gravity = 0.5f;
        }
        else
        {
            characterData.upWardForce = 0.0f;
        }

        //STAYING AFLOAT
        if (stayOnLog == true && Input.GetButtonDown("Action"))
        {
            GameObject.Find("StayAfloat").GetComponent<Collider>().isTrigger = false;
            Debug.Log("It is NOT a trigger");
        }

        if (stayOnLog == true && Input.GetButtonUp("Action"))
        {
            GameObject.Find("StayAfloat").GetComponent<Collider>().isTrigger = true;
            Debug.Log("It is a trigger");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //DOES THE PLAYER NEED TO STOP WALKING?
        TriggerScript ts = other.gameObject.GetComponent<TriggerScript>();
        //if (ts != null)
        //{
        //    if (ts.stopWalking == true)
        //    {
        //        UpdateMovementState(CharacterData.MovementStats.MovementState.STANDING);
        //        canRun = false;
        //    }
        //}

        //MOVEMENT TAGS
        if (other.gameObject.tag == "Trot")
        {
            MoveState(CharacterData.MovementStats.MovementState.TROTTING);
        }

        if (other.gameObject.tag == "Walk")
        {
            MoveState(CharacterData.MovementStats.MovementState.WALKING);
            canRun = false;
            //IF CRAWLING
            crawling = false;
        }


        //OTHER TAGS
        if (other.gameObject.name == "ActivateMonster")
        {
            MonsterChase mc = GameObject.Find("Monster").GetComponent<MonsterChase>();
            mc.forwardMvt = true;
        }

        if (other.gameObject.name == "SceneEnd")
        {
            MonsterChase mc = GameObject.Find("Monster").GetComponent<MonsterChase>();
            if (mc != null)
            {
                mc.playerCanDie = false;
            }
            
        }

        if (other.gameObject.name == "AboveWater")
        {
            GameObject.Find("StayAfloat").GetComponent<Collider>().isTrigger = false;
            canFloat = false;
            StartCoroutine(StayOnLog());
        }

        if (other.gameObject.name == "OffTheRiver")
        {
            characterData.sidewaysForce = 3.0f;
            characterData.forwardForce = 5.0f;
        }

        if (other.gameObject.name == "Land")
        {
            characterData.sidewaysForce = 0.0f;
            characterData.forwardForce = 0.0f;
            CharacterInstructions.Instance.PlayEventFeedback("Aquaman");
            MoveState(CharacterData.MovementStats.MovementState.CRAWLING);

            //TURN OFF VOLUNTARY MOVEMENT
            CharacterMovement cm = GetComponent<CharacterMovement>();
            cm.voluntaryMovement = false;

        }

        if (other.gameObject.tag == "ObstacleObj")
        {
            if (cHealth != null)
            {
                cHealth.TakeDamage(100);
            }
        }

        //if (other.gameObject.name == "PassOut")
        //{
        //    UpdateMovementState(CharacterData.MovementStats.MovementState.STANDING);

        //}

        other.gameObject.SetActive(false);
    }

    public void MoveState(CharacterData.MovementStats.MovementState movementState)
    {
        if (movementState == CharacterData.MovementStats.MovementState.CRAWLING)
        {
            crawling = true;
        }
        else if (movementState == CharacterData.MovementStats.MovementState.RUNNING)
        {
            canRun = true;
        }
        else {
            UpdateMovementState(movementState);
            //crawling = false;
            //canRun = false;
            Debug.Log(movementState);
        }
        

    }

    private void UpdateMovementState(CharacterData.MovementStats.MovementState movementState)
    {
        characterData._currentMovementState = movementState;

        switch (CurrentMovementState)
        {
            case CharacterData.MovementStats.MovementState.STANDING:
                canRun = false;
                characterData.speed = 0.0f;
                characterData.outOfBreath = false;
                break;

            case CharacterData.MovementStats.MovementState.WALKING:
                characterData.speed = 2.0f;
                characterData.outOfBreath = false;
                break;

            case CharacterData.MovementStats.MovementState.TROTTING:
                characterData.speed = 3.0f;
                characterData.outOfBreath = false;
                break;

            case CharacterData.MovementStats.MovementState.RUNNING:
                characterData.speed = 4.0f;
                characterData.outOfBreath = false;
                break;
            case CharacterData.MovementStats.MovementState.SCARED_RUNNING:
                characterData.speed = 5.0f;
                characterData.outOfBreath = true;
                break;

            case CharacterData.MovementStats.MovementState.FLOATING:
                //WHAT HAPPENS WHEN THE CHATACTER IS FLOATING
                characterData.forwardForce = 1.0f;
                canRun = false;
                break;

            case CharacterData.MovementStats.MovementState.CRAWLING:
                characterData.speed = 1.0f;
                characterData.outOfBreath = true;
                canRun = false;
                break;

            default:
                break;
        }
    }

    public void ReceiveAction(string actionName)
    {
        //set that action as pending 
        //make that action possible to complete 
        //wait for it to be completed

        //POSSIBLE ACTIONS; 
        //  -JUMP
        //  -RUN


        CharacterInstructions.Instance.IncompleteAction(actionName, true);
        Debug.Log(actionName);

        //if (actionName == "Jump")
        //{

        //    canFirstJump = true;
        //    currentInstruction = "Jump";
        //    characterData.verticalVelocity = 8.0f;

        //}

        currentInstruction = actionName;

        if (actionName == "Pick_Jerrycan")
        {
            canPickJerrycan = true;
        }

        if (actionName == "Open_Door")
        {
            Debug.Log("Receiving new action" + ":" + actionName);
            openDoor = true;
        }

        switch (actionName)
        {
            case "Jump_Action":
                characterData.verticalVelocity = 8.0f;
                canFirstJump = true;
                break;

            case "Jump_Again":
                characterData.verticalVelocity = 8.0f;
                canSecondJump = true;
                break;

            case "Start_Walking":
                MoveState(CharacterData.MovementStats.MovementState.WALKING);
                canWalk = true;
                break;

            case "Keep_Moving":
                MoveState(CharacterData.MovementStats.MovementState.WALKING);
                //canWalk = true;
                break;

            case "Keep_Going_Action":
                MoveState(CharacterData.MovementStats.MovementState.WALKING);
                //canWalk = true;
                break;

            case "Keep_Walking":
                MoveState(CharacterData.MovementStats.MovementState.WALKING);
                //canWalk = true;
                break;

            case "Drop_Jerrycan":
                canDropJerrycan = true;
                break;

            case "RB_Run":
                canRun = true;
                break;

            case "Runnn":
                MoveState(CharacterData.MovementStats.MovementState.WALKING);
                canRun = true;
                break;

            case "Go_Now":
                MoveState(CharacterData.MovementStats.MovementState.WALKING);
                canRun = true;
                break;

            case "Press_B_To_Get_Up":
                MoveState(CharacterData.MovementStats.MovementState.WALKING);
                canGetUp = true;
                break;

            case "Jump_Tree_Action":
                characterData.verticalVelocity = 8.0f;
                MoveState(CharacterData.MovementStats.MovementState.TROTTING);
                jumpingTrunks = true;
                break;

            case "Pick_Stick":
                pickStick = true;
                break;

            case "PressB_Repeatedly":
                StartCoroutine(ClearFirstPath());
                break;

            case "Keep_Clearing":
                otherPaths = true;
                break;

            case "Pick_Mango":
                pickMango = true;
                break;

            case "Pull_Branch":
                pullBranch = true;
                break;

            case "Go_Up_For_Air":
                canFloat = true;
                break;

            case "Run_In_Cave":
                canRun = true;
                break;

            case "RT_Crawl":
                crawling = true;
                break;

            case "Climb_Out":
                characterData.verticalVelocity = 8.0f;
                MoveState(CharacterData.MovementStats.MovementState.WALKING);
                break;

            default:
                break;
        }

    }

    IEnumerator ClearFirstPath()
    {
        yield return new WaitForSeconds(2.0f);
        clearFirstPath = true;
    }

    IEnumerator StayOnLog()
    {
        yield return new WaitForSeconds(8.0f);
        stayOnLog = true;
        yield return new WaitForSeconds(2.0f);
        GameObject.Find("StayAfloat").GetComponent<Collider>().isTrigger = false;
    }
}
