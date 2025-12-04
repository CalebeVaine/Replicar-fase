using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 8f;
    public float jumpVelocity = 13f;
    public float holdUpwardForce = 30f;
    public float maxHoldTime = 0.25f;
    public LayerMask groundLayer;
    public Transform feet;
    public float feetRadius = 0.1f;

    Rigidbody2D rb;
    float holdTimer;
    bool holdingJump;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector2(h * moveSpeed, rb.linearVelocity.y);

        bool grounded = Physics2D.OverlapCircle(feet.position, feetRadius, groundLayer);

        if (Input.GetButtonDown("Jump") && grounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpVelocity);
            holdingJump = true;
            holdTimer = maxHoldTime;
        }

        if (Input.GetButton("Jump") && holdingJump)
        {
            if (holdTimer > 0f)
            {
                rb.AddForce(Vector2.up * holdUpwardForce * Time.deltaTime, ForceMode2D.Force);
                holdTimer -= Time.deltaTime;
            }
            else
            {
                holdingJump = false;
            }
        }

        if (Input.GetButtonUp("Jump"))
        {
            holdingJump = false;
            if (rb.linearVelocity.y > 0f)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.6f);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (feet != null)
            Gizmos.DrawWireSphere(feet.position, feetRadius);
    }
}