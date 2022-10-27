using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectsPlayer : MonoBehaviour
{

  public AudioSource BGM;
  public AudioSource Piece;

  public AudioSource Environment;
  public AudioClip sfx_hard_drop, sfx_line_clear, sfx_move, sfx_rotation, sfx_gameover, sfx_lock;



  public void PlayBGM()
  {
    BGM.Play();
  }

  public void PauseBGM()
  {
    BGM.Pause();
  }
  public void PlayHardDropAudio()
  {
    Piece.clip = sfx_hard_drop;
    Piece.Play();

    Debug.Log("PlayHardDropAudio");
  }

  public void PlayLineClearAudio()
  {
    Piece.clip = sfx_line_clear;
    Piece.Play();

    Debug.Log("PlayLineClearAudio");
  }

  public void PlayMoveAudio()
  {
    Piece.clip = sfx_move;
    Piece.Play();

    Debug.Log("PlayMoveAudio");
  }

  public void PlayRotationAudio()
  {
    Piece.clip = sfx_rotation;
    Piece.Play();

    Debug.Log("PlayRotationAudio");
  }

  public void PlayGameOverAudio()
  {
    Environment.clip = sfx_gameover;
    Environment.Play();

    Debug.Log("PlayGameOverAudio");
  }

  public void PlayLockAudio()
  {
    Environment.clip = sfx_lock;
    Environment.Play();
  }
}
