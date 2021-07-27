using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Global;
using static Spawner;

public class Platform : MonoBehaviour
{
    public bool first;
    public bool activated;

    public bool withCoin;
    public coinType coinStructure;

    public bool moving;
    public float speed;
    public Vector3 initialPosition;
    public Vector3 targetPosition;

    public static PlatformSkin skin;

    private void Awake()
    {
        GetComponent<SpriteRenderer>().sprite = skin.sprite;
        GetComponent<BoxCollider2D>().size = skin.colliderSize;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (withCoin)
        {
            spawnCoins();
        }

        if(moving)
        {
           StartCoroutine(CycleMove(transform, transform.position, targetPosition, speed));
        }
    }

    public void spawnCoins()
    {
        switch (coinStructure)
        {
            case coinType.low:
                {
                    Instantiate(_coinPrefab, transform.position + Vector3.up * 1f, Quaternion.identity, transform);
                    break;
                }
            case coinType.medium:
                {
                    Instantiate(_coinPrefab, transform.position + Vector3.up * 2f, Quaternion.identity, transform);
                    break;
                }
            case coinType.high:
                {
                    Instantiate(_coinPrefab, transform.position + Vector3.up * 3f, Quaternion.identity, transform);
                    break;
                }
        }
    }

    public static IEnumerator CycleMove(Transform targetObject, Vector3 initialPosition, Vector3 targetPosition, float motionSpeed)
    {
        for (; ; )
        {
            for (; ; )
            {
                if (Mathf.Abs(Vector3.Distance(targetObject.localPosition, targetPosition)) > _maxDifference)
                {
                    targetObject.position = Vector3.Lerp(targetObject.localPosition, targetPosition, motionSpeed * Time.deltaTime);
                }
                else
                {
                    targetObject.localPosition = targetPosition;
                    break;
                }
                yield return new WaitForFixedUpdate();
            }
            for (; ; )
            {
                if (Mathf.Abs(Vector3.Distance(targetObject.localPosition, initialPosition)) > _maxDifference)
                {
                    targetObject.position = Vector3.Lerp(targetObject.localPosition, initialPosition, motionSpeed * Time.deltaTime);
                }
                else
                {
                    targetObject.localPosition = initialPosition;
                    break;
                }
                yield return new WaitForFixedUpdate();
            }
        }
    }
}
