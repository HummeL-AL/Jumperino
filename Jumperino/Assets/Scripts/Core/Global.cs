using System;
using System.IO;
using System.Net;
using System.Collections;
using System.Net.NetworkInformation;
using TMPro;
using UnityEngine;
using static PlayerData;
using static SaveSystem;
using static GameController;
using static DatabaseController;

public class Global : MonoBehaviour
{
    public float maxDifference;
    public TextMeshProUGUI nicknameField;

    public static float soundVolume;
    public static float musicVolume;

    public ParticleSystem bgParticles;

    public static float _maxDifference;
    public static TextMeshProUGUI _nicknameField;

    public float prevSize;
    public Canvas canvas;

    public static string macAdress;

    public float MusicVolume
    {
        get => musicVolume;
        set
        {
            musicVolume = value;
            GetComponent<AudioSource>().volume = musicVolume;
        }
    }

    public BackgroundSkin skin;
    public BackgroundSkin Skin
    {
        get => skin;
        set
        {
            skin = value;
            RenderSettings.skybox = skin.material;
            if(bgParticles)
            {
                Destroy(bgParticles);
            }
            else
            {
                if(skin.ambientParticles)
                {
                    bgParticles = Instantiate(skin.ambientParticles, Vector3.zero, Quaternion.identity, canvas.transform);
                }
            }
        }
    }

    private void Awake()
    {
        _maxDifference = maxDifference;
        _nicknameField = nicknameField;

        GetMacAddress();

        TryToLoadData();
        TryToLoadSettings();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (prevSize != 0f && cam.orthographicSize != prevSize)
        {
            var size = bgParticles.main;
            size.startSize = new ParticleSystem.MinMaxCurve(size.startSize.constantMin * (cam.orthographicSize / prevSize), size.startSize.constantMax * (cam.orthographicSize / prevSize));
            var shape = bgParticles.shape;
            shape.scale = new Vector3(cam.orthographicSize * cam.aspect * 2f, cam.orthographicSize * 2f, 1f);
        }
        prevSize = cam.orthographicSize;
    }

    public void UpdateBackground()
    {

    }

    public static IEnumerator RotateTo(Transform targetObject, Vector3 targetRotation, float rotationSpeed)
    {
        for (; ; )
        {
            if (Mathf.Abs(Vector3.Distance(targetObject.localEulerAngles, targetRotation)) > _maxDifference)
            {
                targetObject.rotation = Quaternion.Slerp(targetObject.rotation, Quaternion.Euler(targetRotation), rotationSpeed * Time.deltaTime);
            }
            else
            {
                targetObject.rotation = Quaternion.Euler(targetRotation);
                break;
            }
            yield return new WaitForFixedUpdate();
        }
    }

    public static IEnumerator MoveTo(Transform targetObject, Vector3 targetPosition, float motionSpeed)
    {
        for (; ; )
        {
            if (Mathf.Abs(Vector3.Distance(targetObject.localPosition, targetPosition)) > _maxDifference)
            {
                targetObject.position = Vector3.Slerp(targetObject.localPosition, targetPosition, motionSpeed * Time.deltaTime);
            }
            else
            {
                targetObject.localPosition = targetPosition;
                break;
            }
            yield return new WaitForFixedUpdate();
        }
    }

    public static IEnumerator ChangeCameraSize(Camera cam, float targetSize, float lerpSpeed)
    {
        for (; ; )
        {
            if (Mathf.Abs(cam.orthographicSize - targetSize) > _maxDifference)
            {
                cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetSize, lerpSpeed);
            }
            else
            {
                cam.orthographicSize = targetSize;
                break;
            }
            yield return new WaitForFixedUpdate();
        }
    }

    public static void GetMacAddress()
    {
        foreach (NetworkInterface ninf in NetworkInterface.GetAllNetworkInterfaces())
        {
            if (ninf.Description == "en0")
            {
                macAdress = ninf.GetPhysicalAddress().ToString();
                break;
            }
            else
            {
                macAdress = ninf.GetPhysicalAddress().ToString();

                if (macAdress != "")
                {
                    break;
                };
            }
        }
    }

    public static string GetTime()
    {
        string time = "";

        time += DateTime.Now.Year;
        time += checkDec(DateTime.Now.Month);
        time += checkDec(DateTime.Now.Day);
        time += checkDec(DateTime.Now.Hour);
        time += checkDec(DateTime.Now.Minute);
        time += checkDec(DateTime.Now.Second);

        return time;
    }

    public static string checkDec(int toCheck)
    {
        string doubleCharInt = "" + toCheck;
        if (toCheck / 10 == 0)
        {
            doubleCharInt = "0" + toCheck;
        }
        return doubleCharInt;
    }

    public static void SetNickname()
    {
        _nickname = _nicknameField.text;
        _nicknameEntered = true;
        SaveDataOnline();
    }

    public static string GetHtmlFromUrl(string resource)
    {
        string html = string.Empty;
        HttpWebRequest req = (HttpWebRequest)WebRequest.Create(resource);
        try
        {
            using (HttpWebResponse resp = (HttpWebResponse)req.GetResponse())
            {
                bool isSuccess = (int)resp.StatusCode < 299 && (int)resp.StatusCode >= 200;
                if (isSuccess)
                {
                    using (StreamReader reader = new StreamReader(resp.GetResponseStream()))
                    {
                        //We are limiting the array to 80 so we don't have
                        //to parse the entire html document feel free to 
                        //adjust (probably stay under 300)
                        char[] cs = new char[80];
                        reader.Read(cs, 0, cs.Length);
                        foreach (char ch in cs)
                        {
                            html += ch;
                        }
                    }
                }
            }
        }
        catch
        {
            return "";
        }
        return html;
    }

    public static bool IsConnected()
    {
        string HtmlText = GetHtmlFromUrl("http://google.com");
        if (HtmlText == "")
        {
            Debug.Log("No internet connection!");
            return false;
        }
        else
        {
            if (!HtmlText.Contains("schema.org/WebPage"))
            {
                Debug.Log("Redirection!");
                return true;
            }
            else
            {
                Debug.Log("Internet connection detected.");
                return true;
            }
        }
    }
}
