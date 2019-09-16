using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockMovement : MonoBehaviour
{
    void Update()
    {
        transform.Translate(new Vector3(0,0,-2) * Time.deltaTime);
    }
}
