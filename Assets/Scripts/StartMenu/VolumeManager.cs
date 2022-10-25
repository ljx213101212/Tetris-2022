using UnityEngine;
using UnityEngine.UI;

public class VolumeManager : MonoBehaviour
{
  [SerializeField] private Slider volumeSlider = null;

  [SerializeField] private Text volumeTextUI = null;

  private void Start()
  {
    LoadValues();
  }
  public void VolumeSlider(float volume)
  {
    volumeTextUI.text = Mathf.RoundToInt(volume * 100).ToString();
  }

  public void SaveVolumeButton()
  {
    Debug.Log("SaveVolumeButton " + volumeSlider.value.ToString());
    float volumeVolume = volumeSlider.value;
    PlayerPrefs.SetFloat("VolumeValue", volumeVolume);
    LoadValues();
  }

  public void LoadValues()
  {
    float volumeValue = Util.TetrisUtil.GetPlayerPrefVolume();
    Debug.Log("LoadValues " + volumeValue.ToString());
    volumeSlider.value = volumeValue;
    VolumeSlider(volumeValue);
    AudioListener.volume = volumeValue;
  }
}
