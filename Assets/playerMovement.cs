using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public float speed = 4;
    public float runMultiplier = 2.0f;
    public float jumpForce = 6;
    public float fallMultiplier = 4;
    public float minJumpTime = 0.3f;
    public float airMobility = 0.25f;
    public float acceleration = 0.5f;
    public float deceleration = 0.1f;

    float startOfJump;
    bool isGrounded = false;
    bool isFastFalling = false;

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.GetContact(0).normal == new Vector2(0.0f, 1.0f))
        {
            isGrounded = true;
        }
    }


    void OnCollisionExit2D(Collision2D coll)
    {
        isGrounded = false;
    }

    void Move()
    {
        float current = rb.velocity.x;
        float moveBy = speed * Input.GetAxisRaw("Horizontal");
        float acc = acceleration;
        float dec = deceleration;
        if (!isGrounded)
        {
            acc *= airMobility;
            dec *= airMobility;
        }

        if (Input.GetKey(KeyCode.LeftShift) && isGrounded)
        {
            moveBy *= runMultiplier;
        }
        if (current < moveBy)
        {
            if (Mathf.Abs(current) < Mathf.Abs(moveBy))
            {
                current += acc;
            }
            else
            {
                current += dec;
            }
        }
        else if (current > moveBy)
        {
            if (Mathf.Abs(current) < Mathf.Abs(moveBy))
            {
                current -= acc;
            }
            else
            {
                current -= dec;
            }
        }
        rb.velocity = new Vector2(current, rb.velocity.y);
    }

    void Jump()
    {
        if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space)) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            isFastFalling = false;
            startOfJump = Time.time;
        }
    }

    void FastFall()
    {
        if (Time.time - startOfJump >= minJumpTime && (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.LeftShift)))
        {
            isFastFalling = true;
            rb.velocity = new Vector2(rb.velocity.x, 0);
        }
        if (isFastFalling)
        {
            rb.velocity += Vector2.up * Physics2D.gravity * (fallMultiplier - 1) * Time.deltaTime;
        }
    }
    // Update is called once per frame
    void Update()
    {
        Move();
        Jump();
        FastFall();
    }
}
