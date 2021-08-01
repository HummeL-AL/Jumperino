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

    private void Awake()
    {
        TryToLoadSettings();

        timeSlider.value = pc.MaxTouchTime;
        musicSlider.value = cam.GetComponent<Global>().MusicVolume;
        soundSlider.value = soundVolume;

        SetText();
    }

    public void UpdateSettings()
    {
        SetText();

        pc.MaxTouchTime = timeSlider.value;
        cam.GetComponent<Global>().MusicVolume = musicSlider.value;
        soundVolume = soundSlider.value;

        TryToSaveSettings();
    }

    public void SetText()
    {
        transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = timeSlider.value.ToString("F2");
        transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = musicSlider.value.ToString("F2");
        transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = soundSlider.value.ToString("F2");
    }
}
