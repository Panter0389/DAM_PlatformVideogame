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
    public float fallSpeedMultiplier = 2f;

    [Header("Ground Check")]
    public Transform groundCheckTransform;
    public Vector2 groundCheckSize = new Vector2(0.5f, 0.1f);
    public LayerMask groundLayer;

    [Header("Components")]
    public Animator playerAnimator;
    public SpriteRenderer playerRenderer;


    Rigidbody2D body;
    bool isGrounded;
    float horizontalMovement = 0;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }


    public void FixedUpdate()
    {
        body.linearVelocityX = horizontalMovement * playerSpeed;
        GroundCheck();
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
    }

}
