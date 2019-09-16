using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventScript : MonoBehaviour
{
    public bool canListenForEvents;
    GameObject character;

    private void Start()
    {
        character = CharacterData.Instance.gameObject;
    }

    public void Go() {
        if (canListenForEvents == true)
        {
            AkSoundEngine.PostEvent("Go", character);
        }
    }

    public void Stop()
    {
        if (canListenForEvents == true)
        {
            AkSoundEngine.PostEvent("Stop", character);
        }

    }
}
