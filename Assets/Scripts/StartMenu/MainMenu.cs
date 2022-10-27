using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Util;

public class MainMenu : MonoBehaviour
{

  private void Awake()
  {
    AudioListener.volume = Util.TetrisUtil.GetPlayerPrefVolume();
  }
  public void PlayGame()
  {
    PauseMenu.GameIsPaused = false;
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
  }

  public void QuitGame()
  {
    Debug.Log("Quit Game");
    Application.Quit();
  }
}
