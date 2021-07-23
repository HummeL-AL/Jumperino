using System;
using System.Linq;
using UnityEngine;
using static GameController;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    public int maxPlatformsCount;
    public float maxAngleBetweenPlatforms;
    public float minDistanceBetweenPlatforms;
    public Vector3 startPlatformPosition;

    public GameObject platformPrefab;
    public GameObject coinPrefab;
    public PlatformSkin platformSkin;

    public static int _maxPlatformsCount;
    public static float _maxAngleBetweenPlatforms;
    public static float _minDistanceBetweenPlatforms;
    public static Vector3 _startPlatformPosition;
    public static GameObject _platformPrefab;
    public static GameObject _coinPrefab;
    public static PlatformSkin _platformSkin;

    public static Vector2 curCameraPosition;
    public static Vector2 curCameraSize;
    public static Transform prevPlatform;
    public static Transform curPlatform;
    public static Transform[] platforms;

    public enum coinType
    {
        low,
        medium,
        high
    }

    private void Awake()
    {
        curCameraPosition = cam.transform.position;
        curCameraSize.y = 2f * 6.9f;
        curCameraSize.x = curCameraSize.y * cam.aspect;

        _maxPlatformsCount = maxPlatformsCount;
        _startPlatformPosition = startPlatformPosition;
        _platformPrefab = platformPrefab;
        _coinPrefab = coinPrefab;
        _platformSkin = platformSkin;
        _maxAngleBetweenPlatforms = maxAngleBetweenPlatforms;
        _minDistanceBetweenPlatforms = minDistanceBetweenPlatforms;

        platforms = new Transform[maxPlatformsCount];
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void UpdatePlatforms()
    {
        Platform.skin = _platformSkin;
        if (platforms.All(x => x == null))
        {
            Platform startPlatform = CreatePlatform(_startPlatformPosition, Quaternion.identity).GetComponent<Platform>();
            platforms[0] = startPlatform.transform;
            startPlatform.first = true;
            startPlatform.activated = true;
            SpawnPlatforms();
        }
        else
        {
            SpawnPlatforms();
        }
    }

    public static void UpdateCurPlatform(Transform newPlatform)
    {
        prevPlatform = curPlatform;
        curPlatform = newPlatform;
        int prevPlatformId = Array.IndexOf(platforms, prevPlatform);
        int newPlatformId = Array.IndexOf(platforms, newPlatform);

        if (!newPlatform.GetComponent<Platform>().activated)
        {
            platforms[newPlatformId].GetComponent<Platform>().activated = true;
            platforms[newPlatformId - 1].GetComponent<Platform>().activated = true;
            curScores += newPlatformId - prevPlatformId;
            UpdateScores();
        }

        int offset = Mathf.Clamp(newPlatformId - 1, 0, _maxPlatformsCount);
        if (offset != 0)
        {
            for (int platformId = 0; platformId < platforms.Length; platformId++)
            {
                if(platformId < offset)
                {
                    Destroy(platforms[platformId].gameObject);
                }

                if(platformId+offset < platforms.Length)
                {
                    platforms[platformId] = platforms[platformId + offset];
                }
                else
                {
                    platforms[platformId] = null;
                }
            }
        }
        SpawnPlatforms();
    }

    public static void SpawnPlatforms()
    {
        for(int platformId = 0; platformId < platforms.Length; platformId++)
        {
            if(platforms[platformId] == null)
            {
                int difficulty = curCoins + platformId - 1;
                float scaleValue = Mathf.Sqrt(difficulty / 200f);
                float clampedScaleValue = Mathf.Clamp(scaleValue, 0f, 0.75f);
                float clamped01ScaleValue = Mathf.Clamp01(scaleValue);

                Vector3 spawnPos = new Vector3();
                Vector3 prevPlatform = platforms[platformId - 1].position;

                float minX = prevPlatform.x + (_minDistanceBetweenPlatforms * (1f - clamped01ScaleValue) + (curCameraSize.x * 0.4f * clamped01ScaleValue));
                float maxX = prevPlatform.x + (_minDistanceBetweenPlatforms * (1f - clamped01ScaleValue) + (curCameraSize.x * 0.7f * clamped01ScaleValue));
                spawnPos.x = Random.Range(minX, maxX);
                float maxY = ((spawnPos.x - prevPlatform.x) * Mathf.Sin(_maxAngleBetweenPlatforms * Mathf.Deg2Rad)) / Mathf.Sin((90 - _maxAngleBetweenPlatforms) * Mathf.Deg2Rad);
                spawnPos.y = Mathf.Clamp(Random.Range(prevPlatform.y - maxY, prevPlatform.y + maxY), -curCameraSize.y * 0.45f, curCameraSize.y * 0.3f);

                Platform createdPlatform = CreatePlatform(spawnPos, Quaternion.identity).GetComponent<Platform>();
                platforms[platformId] = createdPlatform.transform;

                if (Random.Range(0f, 1f) < clampedScaleValue)
                {
                    createdPlatform.withCoin = true;
                    createdPlatform.coinStructure = (coinType)Random.Range(0, 3);
                }
            }
        }
    }

    public static Transform CreatePlatform(Vector3 spawnPosition, Quaternion spawnRotation)
    {
        return Instantiate(_platformPrefab, spawnPosition, spawnRotation).transform;
    }

    public static void ClearPlatforms()
    {
        for (int platformId = 0; platformId < platforms.Length; platformId++)
        {
            Destroy(platforms[platformId].gameObject);
            platforms[platformId] = null;
        }
    }
}
