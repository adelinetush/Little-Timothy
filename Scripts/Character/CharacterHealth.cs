using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHealth : MonoBehaviour
{
    public float playerHeath;

    private void Start()
    {
        //ASSIGN THIS FROM THE CHARACTER DATA SO THAT IT IS PART OF THE SINGLETON
        playerHeath = CharacterData.Instance.currentHealth;
    }
    public void TakeDamage(int damage) {
        playerHeath -= damage;

        if (playerHeath < 1)
        {
            //do death stuff
            Debug.Log("The Player is dead");
        }
    }
}
