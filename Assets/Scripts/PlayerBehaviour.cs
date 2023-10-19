using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    [Header("Player Movement Properties")]
    public float horizontalForce;
    public float maxSpeed;
    public float verticalForce;
    public float airFactor;
    public Transform groundPoint;
    public float groundRadius;
    public LayerMask groundLayerMask;
    public bool isGrounded;

    [Header("Screen Shake Properties")]
    public CinemachineVirtualCamera virtualCamera;
    public CinemachineBasicMultiChannelPerlin perlin;
    public float shakeIntensity;
    public float shakeDuration;
    public float shakeTimer;
    public bool isCameraShaking;

    [Header("Animation Properties")]
    public PlayerAnimationState animationState;


    [Header("Collision Response")]
    public float bounceForce;

    private Animator animator;
    private Rigidbody2D rigidbody2D;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();

        // camera shake
        isCameraShaking = false;
        shakeTimer = shakeDuration;
        virtualCamera = GameObject.Find("Player Camera").GetComponent<CinemachineVirtualCamera>();
        perlin = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    void Update()
    {
        var y = Convert.ToInt32(Input.GetKeyDown(KeyCode.Space));
        Jump(y);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var x = Input.GetAxisRaw("Horizontal");
        Move(x);
        Flip(x);
        //jump
        
        //AirCheck();
        isGrounded = Physics2D.OverlapCircle(groundPoint.position, groundRadius, groundLayerMask);

        if (isCameraShaking)
        {
            if (shakeTimer > 0)
            {
                shakeTimer -= Time.deltaTime;
            }
            else
            {
                shakeTimer = shakeDuration;
                isCameraShaking = false;
                perlin.m_AmplitudeGain = 0;
            }
        }
    }

    private void Move(float x)
    {
        rigidbody2D.AddForce(Vector2.right * x * horizontalForce * ((isGrounded) ? 1 : airFactor));

        rigidbody2D.velocity = new Vector2(Mathf.Clamp(rigidbody2D.velocity.x, -maxSpeed, maxSpeed), rigidbody2D.velocity.y);

        if (isGrounded)
        {
            if (x != 0.0f)
            {
                animationState = PlayerAnimationState.RUN;
                animator.SetInteger("AnimationState", (int)animationState);
            }
            else
            {
                animationState = PlayerAnimationState.IDLE;
                animator.SetInteger("AnimationState", (int)animationState);
            }

        }
    }

    private void Jump(int y)
    {
        if ((isGrounded) && (y > 0.0f))
        {
            rigidbody2D.AddForce(Vector2.up * verticalForce, ForceMode2D.Impulse);
        }

    }

    //private void AirCheck()
    //{
    //    if (!isGrounded)
    //    {
    //        animationState = PlayerAnimationState.JUMP;
    //        animator.SetInteger("AnimationState", (int)animationState);
    //    }
    //}

    private void Flip(float x)
    {
        if (x != 0)
        {
            transform.localScale = new Vector3((x > 0) ? 1 : -1, 1, 1);
        }
    }

    private void ShakeCamera()
    {
        perlin.m_AmplitudeGain = shakeIntensity;
        isCameraShaking = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(groundPoint.position, groundRadius);
    }

}
