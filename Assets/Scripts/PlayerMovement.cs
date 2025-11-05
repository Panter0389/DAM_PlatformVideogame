using UnityEngine;
using static UnityEngine.InputSystem.InputAction;


public class PlayerMovement : MonoBehaviour
{
    [Header("General Settings")]
    public float playerSpeed = 10;
    public float jumpForce = 10;

    [Header("Gravity Settings")]
    public float baseGravity = 2;
    public float maxFallSpeed = 18f;
    public float wallSlideMaxFallSpeed = 9f;
    public float fallSpeedMultiplier = 2f;

    [Header("Ground Check")]
    public Transform groundCheckTransform;
    public Vector2 groundCheckSize = new Vector2(0.5f, 0.1f);
    public LayerMask groundLayer;

    [Header("Wall Check")]
    public Transform leftWallCheckTransform;
    public Transform rightWallCheckTransform;
    public Vector2 wallCheckSize = new Vector2(0.5f, 0.1f);
    public LayerMask wallLayer;

    [Header("SFX")]
    public AudioClip jumpSFX;

    [Header("Components")]
    public Animator playerAnimator;
    public SpriteRenderer playerRenderer;
    public AudioSource audioSource;


    Rigidbody2D body;
    bool isGrounded;
    bool leftWallCollision;
    bool rightWallCollision;
    float horizontalMovement = 0;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }


    public void FixedUpdate()
    {
        body.linearVelocityX = horizontalMovement * playerSpeed;
        GroundCheck();
        WallCheck();
        SetGravity();
    }

    public void Update()
    {
        playerAnimator.SetFloat("XAbsSpeed", Mathf.Abs(body.linearVelocityX) );
        playerAnimator.SetFloat("YSpeed", body.linearVelocityY);

        if (Mathf.Abs(body.linearVelocityX) > 0.01f)
        {
            bool needFlip = body.linearVelocityX < 0;
            playerRenderer.flipX = needFlip;
        }       
    }

    private void SetGravity()
    {
        if(body.linearVelocityY < 0)
        {
            body.gravityScale = baseGravity * fallSpeedMultiplier;

            if(leftWallCollision || rightWallCollision)
                body.linearVelocityY = Mathf.Max(body.linearVelocityY, -wallSlideMaxFallSpeed);
            else
                body.linearVelocityY = Mathf.Max(body.linearVelocityY, -maxFallSpeed);
        }
        else
        {
            body.gravityScale = baseGravity;
        }
    }

    public void GroundCheck()
    {
        if (Physics2D.OverlapBox(groundCheckTransform.position, groundCheckSize, 0, groundLayer))
            isGrounded = true;
        else
            isGrounded = false;
    }

    public void WallCheck()
    {
        if (Physics2D.OverlapBox(rightWallCheckTransform.position, wallCheckSize, 0, wallLayer))
            rightWallCollision = true;
        else
            rightWallCollision = false;

        if (Physics2D.OverlapBox(leftWallCheckTransform.position, wallCheckSize, 0, wallLayer))
            leftWallCollision = true;
        else
            leftWallCollision = false;
    }


    public void PlayerInput_Move(CallbackContext context)
    {
        horizontalMovement = context.ReadValue<Vector2>().x;
    }

    public void PlayerInput_Jump(CallbackContext context)
    {
        if (isGrounded)
        {
            if (context.performed)
            {
                body.linearVelocityY = jumpForce;
                audioSource.PlayOneShot(jumpSFX);
            }
        }

        if (context.canceled && body.linearVelocityY > 0)
        {
            body.linearVelocityY = body.linearVelocityY/2;
        }
    }


    public void OnDrawGizmos()
    {
        Gizmos.DrawCube(groundCheckTransform.position, groundCheckSize);
        Gizmos.DrawCube(rightWallCheckTransform.position, wallCheckSize);
        Gizmos.DrawCube(leftWallCheckTransform.position, wallCheckSize);
    }

}
