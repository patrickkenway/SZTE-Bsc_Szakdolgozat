using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator animator;
    bool isFacingRight = true;
    public ParticleSystem smokeFX;

    [Header("Movement")]
    public float moveSpeed = 5f;
    float horizontalMovement;

    [Header("Jumping")]
    public float jumpPower = 7f;
    public int maxJumps = 2;
    int jumpsRemaining;

    [Header("Groundcheck")]
    public Transform groundCheckPos;
    public Vector2 groundCheckSize = new Vector2 (0.5f, 0.05f);
    public LayerMask groundLayer;
    bool isGrounded;

    [Header("WallCheck")]
    public Transform wallCheckPos;
    public Vector2 wallCheckSize = new Vector2(0.5f, 0.05f);
    public LayerMask wallLayer;

    [Header("Gravity")]
    public float baseGravity = 2;
    public float maxFallSpeed = 18f;
    public float fallSpeedMultiplier = 2;

    [Header("WallMoevement")]
    public float wallSlideSpeed = 2;
    bool isWallSliding;

    //Wall jumping
    bool isWallJumping;
    float wallJumpDirection;
    float wallJumpTime = 0.2f;
    float wallJumpTimer;
    public Vector2 wallJumpPower = new Vector2 (4f, 9f);

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Get the animator, which you attach to the GameObject you are intending to animate.
        animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
        GroundCheck();
        Gravity();
        ProcessWallSlide();
        ProcessWallJump();
        //Flip();

        if (!isWallJumping) {
            rb.linearVelocity = new Vector2(horizontalMovement * moveSpeed, rb.linearVelocityY);
            Flip();
        }
        animator.SetFloat("yVelocity", rb.linearVelocityY);
        animator.SetFloat("magnitude", rb.linearVelocity.magnitude);
        animator.SetBool("isWallSliding", isWallSliding);

    }

    private void Gravity()
    {
        if (rb.linearVelocityY < 0)
        {
            rb.gravityScale = baseGravity * fallSpeedMultiplier;
            rb.linearVelocity = new Vector2(rb.linearVelocityX, Mathf.Max(rb.linearVelocityY, -maxFallSpeed));
        }
        else {
            rb.gravityScale = baseGravity;
        }
    }
    private void ProcessWallSlide() {
        if (!isGrounded & WallCheck() & horizontalMovement != 0)
        {
            isWallSliding = true;
            rb.linearVelocity = new Vector2(rb.linearVelocityX, Mathf.Max(rb.linearVelocityY, -wallSlideSpeed)); //caps fall speed
        }
        else {
            isWallSliding = false;
        }
    }
    private void ProcessWallJump() 
    {
        if (isWallSliding)
        {
            isWallJumping = false;
            wallJumpDirection = -transform.localScale.x;
            wallJumpTimer = wallJumpTime;

            CancelInvoke(nameof(CancelWallJump));
        }
        else {
            wallJumpTimer -= Time.deltaTime;
        }
    }

    private void CancelWallJump() {
        isWallJumping = false;
    }

    public void Move(InputAction.CallbackContext context) {
        horizontalMovement = context.ReadValue<Vector2>().x;
    }
    public void Jump(InputAction.CallbackContext context)
    {
        if (jumpsRemaining > 0)
        {

            if (context.performed)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
                jumpsRemaining--;
                JumpFX();


            }
            else if (context.canceled)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
                jumpsRemaining--;
                JumpFX();
            }

        }
        //Wall jump
        if (context.performed && wallJumpTimer > 0f) {
            isWallJumping = true;
            rb.linearVelocity = new Vector2(wallJumpDirection * wallJumpPower.x, wallJumpPower.y); //Jump away from wall
            wallJumpTimer = 0;
            JumpFX();

            //Force flip
            if (transform.localScale.x != wallJumpDirection) {
                isFacingRight = !isFacingRight;
                Vector3 ls = transform.localScale;
                ls.x *= -1;
                transform.localScale = ls;
            }

            Invoke(nameof(CancelWallJump), wallJumpTime + 0.1f); //Wall jump = 0.5f -- jump again = 0.6f
        }
    }
    private void JumpFX() {
        animator.SetTrigger("jump");
        smokeFX.Play();
    }

    private void GroundCheck() {
        if (Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 0, groundLayer))
        {
            jumpsRemaining = maxJumps;
            isGrounded = true;
        }
        else {
            isGrounded = false;
        }
    }

    private bool WallCheck() {
        return Physics2D.OverlapBox(wallCheckPos.position, wallCheckSize, 0, wallLayer);
    }

    private void Flip() {
        if (isFacingRight && horizontalMovement < 0 || !isFacingRight && horizontalMovement > 0) 
        {
            isFacingRight = !isFacingRight;
            Vector3 ls = transform.localScale;
            ls.x *= -1;
            transform.localScale = ls;

            if(rb.linearVelocityY == 0)
            {
                smokeFX.Play();
            }
            
        }
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(groundCheckPos.position,groundCheckSize);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(wallCheckPos.position, wallCheckSize);
    }
}
