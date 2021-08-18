using TMPro;
using UnityEngine;
using Firebase.Analytics;
using static Global;
using static Spawner;
using static SaveSystem;
using static PlayerData;
using static AdsManager;

public class GameController : MonoBehaviour
{
    public float rotateSpeed;
    public float camSpeed;
    public float zoomSpeed;

    public float basicSize = 2.3f;
    public float basicAspect = 0.5625f;

    public static int curCoins = 0;
    public static int curScores = 0;

    public Animator startAnimation;
    public static Animator _startAnimation;

    public static bool inShop;
    public static bool inRank;
    public static bool inSettings;

    public TextMeshProUGUI scoresText;
    public TextMeshProUGUI maxScoresText;
    public TextMeshProUGUI coinsText;
    public TextMeshProUGUI currentCoinsText;

    public static bool gameStarted = false;
    public static bool gameOver = false;
    public static bool secondLifeUsed = false;

    public static Vector3 camTargetPosition;
    public static float targetSize;

    public static TextMeshProUGUI _scoresText;
    public static TextMeshProUGUI _maxScoresText;
    public static TextMeshProUGUI _coinsText;
    public static TextMeshProUGUI _currentCoinsText;

    public static Coroutine moveCoroutine;
    public static Coroutine sizeCoroutine;

    public static PlayerController pc;
    Spawner spawn;

    public bool GameStarted
    {
        get => gameStarted;
        set
        {
            gameStarted = value;
            _startAnimation.SetBool("GameStarted", gameStarted);

            if (gameStarted)
            {
                CancelInvoke("RotatePlayer");
            }
            else
            {
                InvokeRepeating("RotatePlayer", 0f, Time.fixedDeltaTime);
            }
        }
    }

    public static bool GameOver
    {
        get => gameOver;
        set
        {
            gameOver = value;
            _startAnimation.SetBool("GameOver", gameOver);
        }
    }

    public static bool SecondLifeUsed
    {
        get => secondLifeUsed;
        set
        {
            secondLifeUsed = value;
            _startAnimation.SetBool("SecondLifeUsed", secondLifeUsed);
        }
    }

    public static bool InShop
    {
        get => inShop;
        set
        {
            inShop = value;
            _startAnimation.SetBool("inShop", inShop);
        }
    }
    public static bool InRank
    {
        get => inRank;
        set
        {
            inRank = value;
            _startAnimation.SetBool("inRank", inRank);
        }
    }
    public static bool InSettings
    {
        get => inSettings;
        set
        {
            inSettings = value;
            _startAnimation.SetBool("inSettings", inSettings);
        }
    }

    public static float platformDistance;

    public GameObject player;
    public static GameObject _player;

    public static Camera cam;
    public static bool freeMove = false;

    private void Awake()
    {
        _player = player;
        cam = GetComponent<Camera>();

        pc = _player.GetComponent<PlayerController>();
        pc.game = this;

        spawn = transform.GetComponent<Spawner>();
        _startAnimation = startAnimation;

        InvokeRepeating("RotatePlayer", 0f, Time.fixedDeltaTime);

        _coinsText = coinsText;
        _currentCoinsText = currentCoinsText;
        _scoresText = scoresText;
        _maxScoresText = maxScoresText;
    }

    void Update()
    {
        if (freeMove)
        {
            cam.transform.position = new Vector3(pc.transform.position.x + platformDistance, cam.transform.position.y, cam.transform.position.z);
        }
    }

    public void StartGame()
    {
        Debug.Log("Game started");
        FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLevelStart);
        _totalGamesPlayed++;
        CheckGamesCount();

        curScores = _maxScores / 3;

        pc.transform.parent = null;
        platformDistance = basicSize * (Camera.main.aspect / basicAspect);

        camTargetPosition = new Vector3(platformDistance, 0f, -10f);
        targetSize = basicSize * 3f;

        GameStarted = true;
        pc.enabled = true;
        UpdatePlatforms();

        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }
        if (sizeCoroutine != null)
        {
            StopCoroutine(sizeCoroutine);
        }

        moveCoroutine = StartCoroutine(MoveTo(cam.transform, camTargetPosition, camSpeed));
        freeMove = false;
        sizeCoroutine = StartCoroutine(ChangeCameraSize(cam, targetSize, zoomSpeed));
    }

    public void RestartGame()
    {
        Debug.Log("Game restarted");
        FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLevelStart);
        FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLevelEnd);
        _totalGamesPlayed++;
        CheckGamesCount();

        pc.transform.parent = null;
        curScores = _maxScores / 3;
        curCoins = 0;
        UpdateScores();

        camTargetPosition = Vector3.forward * -10f + Vector3.right * platformDistance;
        ClearPlatforms();
        UpdatePlatforms();
        _player.transform.position = Vector3.zero;
        _player.transform.rotation = Quaternion.identity;
        pc.rb.velocity = Vector2.zero;
        pc.rb.angularVelocity = 0f;

        pc.enabled = true;

        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }
        moveCoroutine = StartCoroutine(MoveTo(cam.transform, camTargetPosition, camSpeed));
        freeMove = false;

        TryToSaveData();

        gamesToAd -= 1;
        CheckInterstitialAd();
    }

    public void TryToReplay()
    {
        TryToReplayAd();
    }

    public static void ReplayGame()
    {
        Debug.Log("Game replayed");
        pc.transform.parent = null;
        _player.transform.position = pc.lastTouchedPlatform.transform.position + Vector3.up;
        _player.transform.rotation = Quaternion.identity;
        pc.rb.velocity = Vector2.zero;
        pc.rb.angularVelocity = 0f;
        pc.enabled = true;
    }

    public void ExitToMenu()
    {
        Debug.Log("Game exit");
        FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLevelEnd);

        pc.transform.parent = null;
        ClearPlatforms();
        GameStarted = false;
        curScores = _maxScores / 3;
        curCoins = 0;
        UpdateScores();

        _player.transform.position = Vector3.zero;
        _player.transform.localScale = Vector3.one;
        pc.jumpAnim.enabled = false;
        pc.rb.velocity = Vector2.zero;
        pc.rb.angularVelocity = 0f;

        camTargetPosition = Vector3.forward * -10f;
        targetSize = basicSize;

        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }
        if (sizeCoroutine != null)
        {
            StopCoroutine(sizeCoroutine);
        }
        moveCoroutine = StartCoroutine(MoveTo(cam.transform, camTargetPosition, camSpeed));
        freeMove = false;
        sizeCoroutine = StartCoroutine(ChangeCameraSize(cam, targetSize, zoomSpeed));

        TryToSaveData();

        gamesToAd -= 1;
        CheckInterstitialAd();
    }

    public static void UpdateScores()
    {
        _scoresText.SetText(curScores.ToString());
        if(curScores > _maxScores)
        {
            _maxScores = curScores;
        }

        _maxScoresText.SetText("Your record: " + _maxScores.ToString());
        _coinsText.SetText(curCoins.ToString());
        _currentCoinsText.SetText(_currentCoins.ToString());
    }

    public void RotatePlayer()
    {
        _player.transform.Rotate(Mathf.Abs(Mathf.Sin(Time.time)) * rotateSpeed * Time.deltaTime, Mathf.Abs(Mathf.Cos(Time.time) * 2) * rotateSpeed * Time.deltaTime, 0f);
    }
}
