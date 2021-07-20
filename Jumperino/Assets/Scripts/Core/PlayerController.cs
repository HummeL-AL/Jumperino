using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Global;
using static Spawner;
using static PlayerData;
using static GameController;

public class PlayerController : MonoBehaviour
{
    public float rotationSpeed;
    public float basicJumpForce;
    public float maxTouchTime;
    public float jumpAngle;

    public static PlayerSkin _skin;
    public PlayerSkin skin
    {
        get => _skin;
        set
        {
            _skin = value;
            GetComponent<MeshFilter>().sharedMesh = _skin.mesh;
            GetComponent<MeshRenderer>().materials = _skin.materials;
        }
    }

    bool onGround;
    float touchTime;

    public Rigidbody2D rb;
    public Animator jumpAnim;
    public GameController game;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        jumpAnim = GetComponent<Animator>();
    }

    void OnEnable()
    {
        OnGameStart();
    }

    void Update()
    {
        if (Input.touchCount != 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    {
                        jumpAnim.SetBool("preparingJump", true);
                        break;
                    }
                case TouchPhase.Moved:
                    {
                        if (!Physics.Raycast(touch.position, Vector3.forward, 30f, LayerMask.NameToLayer("UI")))
                        {
                            if (touchTime < maxTouchTime)
                            {
                                touchTime += Time.deltaTime;
                                Mathf.Clamp(touchTime, 0f, maxTouchTime);
                            }
                        }
                        else if (touchTime != 0)
                        {
                            touchTime = 0;
                        }
                        break;
                    }
                case TouchPhase.Stationary:
                    {
                        if(!Physics.Raycast(touch.position, Vector3.forward, 30f, LayerMask.NameToLayer("UI")))
                        {
                            if (touchTime < maxTouchTime)
                            {
                                touchTime += Time.deltaTime;
                                Mathf.Clamp(touchTime, 0f, maxTouchTime);
                            }
                        }
                        else if(touchTime != 0)
                        {
                            touchTime = 0;
                        }
                        break;
                    }
                case TouchPhase.Ended:
                    {
                        jumpAnim.SetBool("preparingJump", false);

                        if (onGround)
                        {
                            DoJump();
                        }

                        touchTime = 0;
                        onGround = false;
                        break;
                    }
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            jumpAnim.SetBool("preparingJump", true);
        }

        if (Input.GetMouseButton(0))
        {
            if (!Physics.Raycast(Input.mousePosition, Vector3.forward, 30f, LayerMask.NameToLayer("UI")))
            {
                if (touchTime < maxTouchTime)
                {
                    touchTime += Time.deltaTime;
                    Mathf.Clamp(touchTime, 0f, maxTouchTime);
                }
            }
            else if (touchTime != 0)
            {
                touchTime = 0;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            jumpAnim.SetBool("preparingJump", false);

            if (onGround)
            {
                DoJump();
            }

            touchTime = 0;
            onGround = false;
        }

        if (transform.position.y < -cam.orthographicSize - 1f && !GameOver)
        {
            GameOver = true;
            rb.simulated = false;
            rb.velocity = Vector3.zero;
            this.enabled = false;
        }
    }

    public void OnGameStart()
    {
        StartCoroutine(RotateTo(transform, Vector3.zero, rotationSpeed));
        rb.simulated = true;
        jumpAnim.enabled = true;
    }

    public void DoJump()
    {
        float jumpForce = basicJumpForce * touchTime;
        rb.AddForce(new Vector2(jumpForce * Mathf.Cos(jumpAngle * Mathf.Deg2Rad), jumpForce * Mathf.Sin(jumpAngle * Mathf.Deg2Rad)));

        _totalJumps++;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.GetComponent<Platform>())
        {
            Platform collidedPlatform = collision.gameObject.GetComponent<Platform>();
            camTargetPosition = new Vector3(collidedPlatform.transform.position.x + platformDistance, 0f, -10f);

            onGround = true;

            UpdateCurPlatform(collidedPlatform.transform);
            UpdateScores();

            lastPosition = collidedPlatform.transform.position + Vector3.up;
            lastRotation = transform.rotation;

            if (moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
            }
            moveCoroutine = StartCoroutine(MoveTo(cam.transform, camTargetPosition, 5f));
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<Coin>())
        {
            Destroy(collision.gameObject);
            curCoins++;
            _currentCoins++;
            _totalCoins++;
            UpdateScores();
        }
    }
}
