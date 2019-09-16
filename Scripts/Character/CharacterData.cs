using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "PlayerStats", menuName = "PlayerData", order = 1)]
public class CharacterData : Singleton<CharacterData>
{
    #region Health Definitions
    [System.Serializable]
    public class SurvivalStats {
        public float maxHealth;
        public int baseResistance;
    }
    #endregion

    #region Movement Definitions
    [System.Serializable]
    public class MovementStats
    {
        public enum MovementState {
            WALKING,
            TROTTING, 
            RUNNING,
            SCARED_RUNNING,
            STANDING,
            FLOATING,
            CRAWLING
        }

        public float speed;
        public float verticalVelocity;
        public float gravity;

        public bool outOfBreath;

    }
    #endregion

    #region Fields
    public bool setManually = false;
    public bool saveDataOnClose = false;

    public float maxHealth = 0;
    public float currentHealth = 0;

    public int baseResistance = 0;
    public int currentResistance = 0;

    public float speed = 0;
    public bool outOfBreath = false;

    public float verticalVelocity = 0.0f;
    public float gravity = 0.0f;

    public float forwardForce = 0.0f;
    public float upWardForce = 0.0f;
    public float sidewaysForce = 0.0f;


    public MovementStats.MovementState _currentMovementState = MovementStats.MovementState.STANDING;

    //public SurvivalStats survivalStats;
    //public MovementStats movementStats; 


    #endregion
}
