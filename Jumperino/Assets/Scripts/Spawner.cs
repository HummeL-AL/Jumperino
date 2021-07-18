using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameController;

public class Spawner : MonoBehaviour
{
    public int maxPlatformsCount;
    public float maxAngleBetweenPlatforms;
    public float minDistanceBetweenPlatforms;
    public float coinChance = 0.3f;
    public Vector3 startPlatformPosition;

    public GameObject platformPrefab;
    public GameObject coinPrefab;

    public static int _maxPlatformsCount;
    public static float _maxAngleBetweenPlatforms;
    public static float _minDistanceBetweenPlatforms;
    public static float _coinChance;
    public static Vector3 _startPlatformPosition;
    public static GameObject _platformPrefab;
    public static GameObject _coinPrefab;

    public static int curPlatformsCount = 0;
    public static Vector3 curCameraPos;
    public static Transform lastPlatform = null;
    public static Transform[] platforms;

    // Start is called before the first frame update
    void Start()
    {
        platforms = new Transform[maxPlatformsCount];
        curCameraPos = cam.transform.position;
        _maxPlatformsCount = maxPlatformsCount;
        _coinChance = coinChance;
        _startPlatformPosition = startPlatformPosition;
        _platformPrefab = platformPrefab;
        _coinPrefab = coinPrefab;
        _maxAngleBetweenPlatforms = maxAngleBetweenPlatforms;
        _minDistanceBetweenPlatforms = minDistanceBetweenPlatforms;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static IEnumerator UpdatePlatforms()
    {
        foreach (Platform platform in FindObjectsOfType<Platform>())
        {
            if(platform.transform.position.x < camTargetPosition.x - cam.orthographicSize * cam.aspect - _platformPrefab.GetComponent<SpriteRenderer>().bounds.size.x * 0.5f)
            {
                Destroy(platform.gameObject);
                curPlatformsCount--;
            }
        }
        yield return new WaitForEndOfFrame();
        UpdatePlatformsList();
        while (curPlatformsCount < _maxPlatformsCount)
        {
            if (curPlatformsCount == 0)
            {
                CreatePlatform(_platformPrefab, _startPlatformPosition, Vector3.zero, true);
            }
            else
            {
                SpawnPlatform();
            }
        }
    }

    public static void SpawnPlatform()
    {
        UpdatePlatformsList();
        float spawnX = Random.Range(lastPlatform.position.x + _minDistanceBetweenPlatforms, lastPlatform.position.x + targetSize * cam.aspect);

        float maxY = ((spawnX - lastPlatform.position.x) * Mathf.Sin(_maxAngleBetweenPlatforms * Mathf.Deg2Rad)) / Mathf.Sin((90-_maxAngleBetweenPlatforms)*Mathf.Deg2Rad);
        float spawnY = Random.Range(lastPlatform.position.y - maxY, lastPlatform.position.y + maxY);
        spawnY = Mathf.Clamp(spawnY, -cam.orthographicSize * 0.9f, cam.orthographicSize * 0.9f);
        Vector3 spawnPosition = new Vector3(spawnX, spawnY, 0f);
        Vector3 spawnRotation = Vector3.zero;

        CreatePlatform(_platformPrefab, spawnPosition, spawnRotation, false);
    }

    public static void CreatePlatform(GameObject platform, Vector3 spawnPosition, Vector3 spawnRotation, bool start)
    {
        UpdatePlatformsList();
        Quaternion qRotation = Quaternion.Euler(spawnRotation);
        Platform createdPlatform = Instantiate(platform, spawnPosition, qRotation).GetComponent<Platform>();
        createdPlatform.first = start;

        lastPlatform = createdPlatform.transform;
        platforms[curPlatformsCount] = lastPlatform;
        curPlatformsCount++;

        if(!start && Random.Range(0f, 1f) < _coinChance)
        {
            Instantiate(_coinPrefab, createdPlatform.transform.position + Vector3.up * 2f, Quaternion.identity, createdPlatform.transform);
        }
    }

    public static void ClearPlatforms()
    {
        UpdatePlatformsList();
        foreach (Transform platform in platforms)
        {
            Debug.Log(platform + " " + curPlatformsCount);
            if (platform)
            {
                Destroy(platform.gameObject);
                curPlatformsCount--;
            }
        }
        UpdatePlatformsList();
        Debug.Log(curPlatformsCount);
    }

    public static void UpdatePlatformsList()
    {
        for(int i = 0; i < _maxPlatformsCount; i++)
        {
            if (!platforms[i])
            {
                for (int j = i+1; j < _maxPlatformsCount; j++)
                {
                    if(platforms[j])
                    {
                        platforms[i] = platforms[j];
                        platforms[j] = null;
                        break;
                    }
                }
            }
        }
    }
}
