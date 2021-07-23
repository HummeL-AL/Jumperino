using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Global;
using static SaveSystem;
using static GameController;

public class Settings : MonoBehaviour
{
    public Slider timeSlider;
    public Slider musicSlider;
    public Slider soundSlider;

    public void UpdateSettings()
    {
        transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = timeSlider.value.ToString("F3");
        transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = musicSlider.value.ToString("F3");
        transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = soundSlider.value.ToString("F3");

        pc.MaxTouchTime = timeSlider.value;
        cam.GetComponent<Global>().MusicVolume = musicSlider.value;
        soundVolume = soundSlider.value;

        TryToSaveSettings();
    }
}
