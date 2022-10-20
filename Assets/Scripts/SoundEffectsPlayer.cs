using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectsPlayer : MonoBehaviour
{
  public AudioSource src;
  public AudioClip sfx1, sfx2, sfx3;


  public void PlayHardDropAudio()
  {
    src.clip = sfx1;
    src.Play();

    Debug.Log("PlayHardDropAudio");
  }
}
