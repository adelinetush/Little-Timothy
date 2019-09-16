using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterChase : MonoBehaviour
{
    public bool rightTurn, leftTurn, forwardMvt;

    float speedZ, speedX;

    public float x, z;

    //PlayerDeath pDeath;

    public bool playerCanDie;

    GameObject player;
    bool playerFollow;
    private void Start()
    {

        //pDeath = GameObject.Find("PlayerMain").GetComponent<PlayerDeath>();
        rightTurn = false;
        forwardMvt = false;
        leftTurn = false;

        speedX = 0;
        speedZ = 0;

        player = GameObject.Find("Character");
        playerFollow = false;
    }

    void Update()
    {
        transform.Translate(new Vector3(speedX, 0, speedZ) * Time.deltaTime);


        if (rightTurn == true)
        {
            rightTurn = false;
            speedX = x;
            speedZ = 0;
        }


        if (leftTurn == true)
        {
            leftTurn = false;
            speedX = x*-1;
            speedZ = 0;
        }


        if (forwardMvt == true)
        {
            forwardMvt = false;
            speedZ = z;
            speedX = 0;
        }


        if (playerFollow == true)
        {
            gameObject.transform.Translate(new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, player.transform.position.z - 2));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "rightTurn")
        {
            rightTurn = true;

        }

        if (other.name == "forwardTurn")
        {
            forwardMvt = true;
        }

        if (other.name == "leftTurn")
        {
            leftTurn = true;
        }

        if (other.tag == "Character")
        {
            ActionsManager am = GameObject.Find("LevelManager").GetComponent<ActionsManager>();


            if (playerCanDie)
            {
                if (am.currentScene == "Scene 04")
                {
                    speedZ = 0;
                    speedX = 0;

                    playerFollow = true;
                }
                else {
                    speedZ = 0;
                    speedX = 0;
                    CharacterInstructions.Instance.PlayDeathTrack("Too_Slow");

                }
                

            }
            else {
                speedZ = 0;
                speedX = 0;
                rightTurn = false;
                forwardMvt = false;
                leftTurn = false;
            }

        }
    }
}
