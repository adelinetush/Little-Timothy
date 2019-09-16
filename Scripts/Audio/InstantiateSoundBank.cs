using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateSoundBank : MonoBehaviour
{
    uint bankId;

    private void Awake()
    {
        AkSoundEngine.LoadBank("Main_SoundBank", AkSoundEngine.AK_DEFAULT_POOL_ID, out bankId);
    }


}
