using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using static PlayerData;
using static SaveSystem;
using static GameController;

public class Global : MonoBehaviour
{
    public float maxDifference;

    public static float _maxDifference;

    public static string macAdress;

    private void Awake()
    {
        _maxDifference = maxDifference;

        GetMacAddress();
        Debug.Log(macAdress);

        TryToLoadData();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
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
}
