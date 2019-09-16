using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    CharacterController characterController;

    //public float speed = 6.0f;
    //public float jumpSpeed = 8.0f;
    //public float gravity = 20.0f;

    public bool voluntaryMovement;

    private Vector3 moveDirection = Vector3.zero;

    bool jumping, landing, playerCanJump;

    void Start()
    {
        characterController = GetComponent<CharacterController>();



    }

    void Update()
    {

        if (voluntaryMovement == true)
        {
            //INVOLUNTARY MOVEMENT
            characterController.Move(new Vector3(CharacterData.Instance.sidewaysForce * Time.deltaTime, 0, CharacterData.Instance.forwardForce * Time.deltaTime));

            //GOING UP FOR AIR
            characterController.Move(new Vector3(0, CharacterData.Instance.upWardForce * Time.deltaTime, 0));
        }


        if (characterController.isGrounded)
        {
            // We are grounded, so recalculate
            // move direction directly from axes

            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
            if (Input.GetAxis("Vertical") >= 0)
            {
                moveDirection *= CharacterData.Instance.speed;
            }


            if (characterController.velocity.x > 0.1 || characterController.velocity.z > 0.1)
            {
                CharacterInstructions.Instance.PlayMovementAudio();
            }
            else {
                CharacterInstructions.Instance.StopMovementAudio();
            }

            if (Input.GetButtonDown("Jump"))
            {
                moveDirection *= CharacterData.Instance.speed * 3;
                moveDirection.y = CharacterData.Instance.verticalVelocity;

                jumping = true;
            }
            else {
                moveDirection *= CharacterData.Instance.speed;
            }
        }

        if (!characterController.isGrounded)
        {
            playerCanJump = false;
        }
        else {
            playerCanJump = true;
        }

        //S THE PLAYER JUMPING OR LANDING

        if (playerCanJump == true)
        {
            if (characterController.velocity.y < -0.1)
            {
                landing = true;
            }
        }

        if (jumping == true && playerCanJump == false)
        {
            jumping = false;
            CharacterInstructions.Instance.PlayJumpAudio();
        }

        if (landing == true && playerCanJump == true)
        {
            landing = false;
            CharacterInstructions.Instance.PlayLandAudio();
        }

        // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
        // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
        // as an acceleration (ms^-2)
        moveDirection.y -= CharacterData.Instance.gravity * Time.deltaTime;

        // Move the controller
        characterController.Move(moveDirection * Time.deltaTime);
    }
}
