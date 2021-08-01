using System;
using System.Linq;
using UnityEngine;
using static GameController;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public int maxPlatformsCount;
    public float maxAngleBetweenPlatforms;
    public float minDistanceBetweenPlatforms;
    public float minMoveDistance;
    public float minMoveSpeed;
    public float maxMoveSpeed;
    public Vector3 startPlatformPosition;

    [Header("Prefabs Settings")]
    public GameObject platformPrefab;
    public GameObject coinPrefab;
    public PlatformSkin platformSkin;

    [Header("Coin spawn chances")]
    public AnimationCurve coinSpawnChances;

    [Header("Distance settings")]
    public AnimationCurve platformsMinDistance;
    public AnimationCurve platformsMaxDistance;

    [Header("Moving platforms settings")]
    public AnimationCurve movingSpawnChances;
    public AnimationCurve moveMinDistance;
    public AnimationCurve moveMaxDistance;
    public AnimationCurve moveMinSpeed;
    public AnimationCurve moveMaxSpeed;

    [Header("Platforms rotation settings")]
    public AnimationCurve rotatedSpawnChances;
    public AnimationCurve minRotationAngle;
    public AnimationCurve maxRotationAngle;

    public static int _maxPlatformsCount;
    public static float _maxAngleBetweenPlatforms;
    public static float _minDistanceBetweenPlatforms;
    public static float _minMoveDistance;
    public static Vector3 _startPlatformPosition;
    public static GameObject _platformPrefab;
    public static GameObject _coinPrefab;
    public static PlatformSkin _platformSkin;

    public static AnimationCurve _coinSpawnChances;
    public static AnimationCurve _platformsMinDistance;
    public static AnimationCurve _platformsMaxDistance;
    public static AnimationCurve _movingSpawnChances;
    public static AnimationCurve _moveMinDistance;
    public static AnimationCurve _moveMaxDistance;
    public static AnimationCurve _moveMinSpeed;
    public static AnimationCurve _moveMaxSpeed;
    public static AnimationCurve _rotatedSpawnChances;
    public static AnimationCurve _minRotationAngle;
    public static AnimationCurve _maxRotationAngle;

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

        _coinSpawnChances = coinSpawnChances;
        _platformsMinDistance = platformsMinDistance;
        _platformsMaxDistance = platformsMaxDistance;
        _movingSpawnChances = movingSpawnChances;
        _moveMinDistance = moveMinDistance;
        _moveMaxDistance = moveMaxDistance;
        _moveMinSpeed = moveMinSpeed;
        _moveMaxSpeed = moveMaxSpeed;
        _minMoveDistance = minMoveDistance;
        _rotatedSpawnChances = rotatedSpawnChances;
        _minRotationAngle = minRotationAngle;
        _maxRotationAngle = maxRotationAngle;

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
                int difficulty = curScores + platformId - 1;

                float minHeight = -curCameraSize.y * 0.45f;
                float maxHeight = curCameraSize.y * 0.3f;

                Vector3 spawnPos = new Vector3();
                Vector3 prevPlatform = platforms[platformId - 1].position;

                float minX = prevPlatform.x + (_minDistanceBetweenPlatforms * (1f - _platformsMinDistance.Evaluate(difficulty)) + (curCameraSize.x * 0.4f * _platformsMinDistance.Evaluate(difficulty)));
                float maxX = prevPlatform.x + (_minDistanceBetweenPlatforms * (1f - _platformsMaxDistance.Evaluate(difficulty)) + (curCameraSize.x * 0.7f * _platformsMaxDistance.Evaluate(difficulty)));
                spawnPos.x = Random.Range(minX, maxX);
                float maxY = ((spawnPos.x - prevPlatform.x) * Mathf.Sin(_maxAngleBetweenPlatforms * Mathf.Deg2Rad)) / Mathf.Sin((90 - _maxAngleBetweenPlatforms) * Mathf.Deg2Rad);
                spawnPos.y = Random.Range(Mathf.Clamp(prevPlatform.y - maxY, minHeight, Mathf.Infinity), Mathf.Clamp(prevPlatform.y + maxY, -Mathf.Infinity, maxHeight));

                Platform createdPlatform = CreatePlatform(spawnPos, Quaternion.identity).GetComponent<Platform>();
                platforms[platformId] = createdPlatform.transform;

                if (Random.Range(0f, 1f) < _coinSpawnChances.Evaluate(difficulty))
                {
                    createdPlatform.withCoin = true;
                    createdPlatform.coinStructure = (coinType)Random.Range(0, 3);
                }

                if (Random.Range(0f, 1f) < _rotatedSpawnChances.Evaluate(difficulty))
                {
                    Vector3 targetRotation = Vector3.forward * Random.Range(_minRotationAngle.Evaluate(difficulty), _maxRotationAngle.Evaluate(difficulty));

                    if (Random.Range(0, 2) == 0)
                    {
                        targetRotation = -targetRotation;
                    }

                    createdPlatform.transform.rotation = Quaternion.Euler(targetRotation);
                }

                if (Random.Range(0f, 1f) < _movingSpawnChances.Evaluate(difficulty))
                {
                    createdPlatform.moving = true;
                    createdPlatform.gameObject.isStatic = false;

                    bool up = false;

                    if (spawnPos.y + _moveMinDistance.Evaluate(difficulty) < minHeight && spawnPos.y - _moveMinDistance.Evaluate(difficulty) > maxHeight)
                    {
                        up = Convert.ToBoolean(Random.Range(0, 2));
                    }
                    else if (spawnPos.y + _moveMinDistance.Evaluate(difficulty) < minHeight) up = true;

                    Vector3 targetPos = new Vector3();
                    targetPos.x = spawnPos.x;

                    if (up)
                    {
                        float minY = Mathf.Clamp(spawnPos.y + _moveMinDistance.Evaluate(difficulty), minHeight, maxHeight);
                        maxY = Mathf.Clamp(spawnPos.y + _moveMaxDistance.Evaluate(difficulty), minHeight, maxHeight);
                        targetPos.y = Random.Range(minY, maxY);
                    }
                    else
                    {
                        float minY = Mathf.Clamp(spawnPos.y - _moveMinDistance.Evaluate(difficulty), minHeight, maxHeight);
                        maxY = Mathf.Clamp(spawnPos.y - _moveMaxDistance.Evaluate(difficulty), minHeight, maxHeight);
                        targetPos.y = Random.Range(minY, maxY);
                    }

                    createdPlatform.targetPosition = targetPos;
                    createdPlatform.speed = Random.Range(_moveMinSpeed.Evaluate(difficulty), _moveMaxSpeed.Evaluate(difficulty));
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
