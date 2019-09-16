using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateObjects : MonoBehaviour
{
    public GameObject obstacleObj;

    public float zOffset;

    private void OnTriggerEnter(Collider other)
    {
        other.gameObject.SetActive(false);
        if (other.gameObject.name == "Rock")
        {
            Instantiate(obstacleObj, new Vector3(other.transform.position.x+1, other.transform.position.y, other.transform.position.z + zOffset), transform.rotation);
        }
    }
}
