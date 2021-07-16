using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Global;
using static Spawner;
using static SaveSystem;
using static PlayerData;

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
    public TextMeshProUGUI totalCoinsText;

    public static bool gameStarted = false;
    public static bool gameOver = false;
    public static bool secondLifeUsed = false;

    public static Vector3 lastPosition;
    public static Quaternion lastRotation;

    public static Vector3 camTargetPosition;
    public static float targetSize;

    public static TextMeshProUGUI _scoresText;
    public static TextMeshProUGUI _maxScoresText;
    public static TextMeshProUGUI _coinsText;
    public static TextMeshProUGUI _totalCoinsText;

    public static Coroutine moveCoroutine;
    public static Coroutine sizeCoroutine;

    PlayerController pc;
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
    public static GameObject player;

    public static Camera cam;

    private void Awake()
    {
        cam = GetComponent<Camera>();
        player = GameObject.Find("Player");
        pc = player.GetComponent<PlayerController>();
        pc.game = this;

        spawn = transform.GetComponent<Spawner>();
        _startAnimation = startAnimation;

        InvokeRepeating("RotatePlayer", 0f, Time.fixedDeltaTime);

        _coinsText = coinsText;
        _totalCoinsText = totalCoinsText;
        _scoresText = scoresText;
        _maxScoresText = maxScoresText;
    }

    void Start()
    {
        
    }

    void Update()
    {

    }

    public void StartGame()
    {
        platformDistance = basicSize * (Camera.main.aspect / basicAspect);

        camTargetPosition = new Vector3(platformDistance, 0f, -10f);
        targetSize = basicSize * 3f;

        GameStarted = true;
        player.GetComponent<PlayerController>().enabled = true;
        StartCoroutine(UpdatePlatforms());

        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }
        if (sizeCoroutine != null)
        {
            StopCoroutine(sizeCoroutine);
        }

        moveCoroutine = StartCoroutine(MoveTo(cam.transform, camTargetPosition, camSpeed));
        sizeCoroutine = StartCoroutine(ChangeCameraSize(cam, targetSize, zoomSpeed));
    }

    public void RestartGame()
    {
        curScores = 0;
        curCoins = 0;
        UpdateScores();

        camTargetPosition = Vector3.forward * -10f;
        ClearPlatforms();
        StartCoroutine(UpdatePlatforms());
        player.transform.position = Vector3.zero;
        player.transform.rotation = Quaternion.identity;
        pc.rb.velocity = Vector2.zero;
        pc.rb.angularVelocity = 0f;

        pc.enabled = true;

        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }
        moveCoroutine = StartCoroutine(MoveTo(cam.transform, camTargetPosition, camSpeed));

        TryToSaveData();
    }

    public void ReplayGame()
    {
        player.transform.position = lastPosition;
        player.transform.rotation = lastRotation;
        pc.rb.velocity = Vector2.zero;
        pc.rb.angularVelocity = 0f;
        pc.enabled = true;
    }

    public void ExitToMenu()
    {
        ClearPlatforms();
        GameStarted = false;
        curScores = 0;
        curCoins = 0;
        UpdateScores();

        player.transform.position = Vector3.zero;
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
        sizeCoroutine = StartCoroutine(ChangeCameraSize(cam, targetSize, zoomSpeed));

        TryToSaveData();
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
        _totalCoinsText.SetText(_totalCoins.ToString());
    }

    public void RotatePlayer()
    {
        player.transform.Rotate(Mathf.Abs(Mathf.Sin(Time.time)) * rotateSpeed * Time.deltaTime, Mathf.Abs(Mathf.Cos(Time.time) * 2) * rotateSpeed * Time.deltaTime, 0f);
    }
}
