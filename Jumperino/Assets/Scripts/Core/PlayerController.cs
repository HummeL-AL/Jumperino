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
    public float MaxTouchTime
    {
        get => maxTouchTime;
        set
        {
            maxTouchTime = value;
            if (jumpAnim)
            {
                jumpAnim.SetFloat("speedMultiplayer", 1f / maxTouchTime);
            }
        }
    }
    public float jumpAngle;

    public PlayerSkin skin;
    public PlayerSkin Skin
    {
        get => skin;
        set
        {
            skin = value;
            GetComponent<MeshFilter>().sharedMesh = skin.mesh;
            GetComponent<MeshRenderer>().materials = skin.materials;
        }
    }

    public AudioClip coinPickupSound;
    public AudioClip platformHitSound;

    bool onGround;
    float touchTime;

    public Rigidbody2D rb;
    public Animator jumpAnim;
    public GameController game;

    public Platform lastTouchedPlatform;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        jumpAnim = GetComponent<Animator>();
        Skin = skin;
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

                        float angle = transform.localEulerAngles.z;
                        if ((angle > 45 && angle < 135) || (angle > 225 && angle < 315))
                        {
                            jumpAnim.SetBool("horizontal", true);
                        }
                        else
                        {
                            jumpAnim.SetBool("horizontal", false);
                        }
                        break;
                    }
                case TouchPhase.Moved:
                    {
                        if (!Physics.Raycast(touch.position, Vector3.forward, 30f, LayerMask.NameToLayer("UI")))
                        {
                            if (touchTime < MaxTouchTime)
                            {
                                touchTime += Time.deltaTime;
                                Mathf.Clamp(touchTime, 0f, MaxTouchTime);
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
                            if (touchTime < MaxTouchTime)
                            {
                                touchTime += Time.deltaTime;
                                Mathf.Clamp(touchTime, 0f, MaxTouchTime);
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
#if (UNITY_EDITOR)
        if (Input.GetMouseButtonDown(0))
        {
            jumpAnim.SetBool("preparingJump", true);

            float angle = transform.localEulerAngles.z;
            if ((angle > 45 && angle < 135) || (angle > 225 && angle < 315))
            {
                jumpAnim.SetBool("horizontal", true);
            }
            else
            {
                jumpAnim.SetBool("horizontal", false);
            }
        }

        if (Input.GetMouseButton(0))
        {
            if (!Physics.Raycast(Input.mousePosition, Vector3.forward, 30f, LayerMask.NameToLayer("UI")))
            {
                if (touchTime < MaxTouchTime)
                {
                    touchTime += Time.deltaTime;
                    Mathf.Clamp(touchTime, 0f, MaxTouchTime);
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

#endif
        if (transform.position.y < -cam.orthographicSize - 1f && !GameOver)
        {
            GameOver = true;
            rb.simulated = false;
            rb.velocity = Vector3.zero;
            touchTime = 0;
            jumpAnim.SetBool("preparingJump", false);

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
        transform.parent = null;
        float jumpForce = basicJumpForce * (touchTime/MaxTouchTime);
        rb.AddForce(new Vector2(jumpForce * Mathf.Cos(jumpAngle * Mathf.Deg2Rad), jumpForce * Mathf.Sin(jumpAngle * Mathf.Deg2Rad)));

        _totalJumps++;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        Platform collidedPlatform = collision.gameObject.GetComponent<Platform>();
        if (collision.gameObject.GetComponent<Platform>() && (!onGround || lastTouchedPlatform != collidedPlatform))
        {
            lastTouchedPlatform = collidedPlatform;
            transform.SetParent(lastTouchedPlatform.transform);

            AudioSource.PlayClipAtPoint(platformHitSound, collision.transform.position, soundVolume);
            if(skin.landParticles)
            {
                Instantiate(skin.landParticles, transform.position, Quaternion.identity);
            }

            onGround = true;

            UpdateCurPlatform(collidedPlatform.transform);
            UpdateScores();

            freeMove = true;
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Coin>())
        {
            collision.gameObject.GetComponent<Coin>().playerTouched = true;
            Destroy(collision.gameObject);
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        transform.parent = null;
    }
}
