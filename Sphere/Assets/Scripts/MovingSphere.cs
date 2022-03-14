using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingSphere : MonoBehaviour
{
    public bool isGravity; // 因为没有建立新项目 在原有的基础上进行修改 故添加变换按钮

    [SerializeField, Range(0f, 100f)]
    float maxSpeed = 10f;// 最大速度

    [SerializeField, Range(0f, 100f)]
    float maxAcceleration = 10f, maxAirAcceleration = 1f;// 最大加速度

    Vector3 velocity;// 临时变量
    Vector3 desiredVelocity;

    Rigidbody body;
    private void Awake()
    {
        if (isGravity == true)
        {
            body = GetComponent<Rigidbody>();
        }
    }
    private void Update()
    {
        if (isGravity == true)
        {
            isGravityMoveFunction();
        }
        else
        {
            isNotGravityMoveFunction();
        }
    }


    private void FixedUpdate()
    {
        if (isGravity == true)
        {
            // velocity = body.velocity;
            UpdateState();
            float acceleration = onGround ? maxAcceleration : maxAirAcceleration;
            float maxSpeedChange = acceleration * Time.deltaTime;
            velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);
            velocity.z = Mathf.MoveTowards(velocity.z, desiredVelocity.z, maxSpeedChange);
            body.velocity = velocity; // 分配速度
            if (desiredJump)
            {
                desiredJump = false;
                Jump();
            }
            body.velocity = velocity;
            onGround = false;
        }
    }
    void isGravityMoveFunction()
    {
        Vector2 playerInput;
        playerInput.x = Input.GetAxis("Horizontal");
        playerInput.y = Input.GetAxis("Vertical");
        playerInput = Vector2.ClampMagnitude(playerInput, 1f);

        desiredVelocity = new Vector3(playerInput.x, 0f, playerInput.y) * maxSpeed;
        // Jump Part
        desiredJump |= Input.GetButtonDown("Jump");
    }
    bool desiredJump;
    [SerializeField, Range(0f, 10f)]
    float jumpHeight = 2f;
    bool onGround;
    [SerializeField, Range(0, 5)]
    int maxAirJumps = 0;
    int jumpPhase;
    private void OnCollisionEnter(Collision collision)
    {
        EvaluateCollision(collision);
    }
    private void OnCollisionStay(Collision collision)
    {
        EvaluateCollision(collision);

    }
    void EvaluateCollision(Collision collision)
    {
        for (int i = 0; i < collision.contactCount; i++)
        {
            Vector3 normal = collision.GetContact(i).normal;
            onGround |= normal.y > 0.9f;
        }
    }
    void UpdateState()
    {
        velocity = body.velocity;
        if (onGround)
        {
            jumpPhase = 0;
        }
    }
    void Jump()
    {
        if (onGround || jumpPhase < maxAirJumps)
        {
            jumpPhase += 1;
            float jumpSpeed = Mathf.Sqrt(-2f * Physics.gravity.y * jumpHeight);
            if (velocity.y > 0f)
            {
                jumpSpeed = Mathf.Max(jumpSpeed = velocity.y, 0f);
            }
            velocity.y += jumpSpeed;
        }
    }
    void isNotGravityMoveFunction()
    {
        //[SerializeField]
        Rect allowedArea = new Rect(-4.5f, -4.5f, 9f, 9f);

        //[SerializeField, Range(0f, 1f)]
        float bounciness = 0.5f;

        Vector2 playerInput;
        playerInput.x = Input.GetAxis("Horizontal");
        playerInput.y = Input.GetAxis("Vertical");
        playerInput = Vector2.ClampMagnitude(playerInput, 1f);

        Vector3 desiredVelocity = new Vector3(playerInput.x, 0f, playerInput.y) * maxSpeed;
        float maxSpeedChange = maxAcceleration * Time.deltaTime;
        velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);
        velocity.z = Mathf.MoveTowards(velocity.z, desiredVelocity.z, maxSpeedChange);


        Vector3 displacement = velocity * Time.deltaTime;
        Vector3 newPosition = transform.localPosition + displacement;
        // normalGameMoveFunction()
        if (newPosition.x < allowedArea.xMin)
        {
            newPosition.x = allowedArea.xMin;
            velocity.x = -velocity.x * bounciness;
        }
        else if (newPosition.x > allowedArea.xMax)
        {
            newPosition.x = allowedArea.xMax;
            velocity.x = -velocity.x * bounciness;
        }

        if (newPosition.z < allowedArea.yMin)
        {
            newPosition.z = allowedArea.yMin;
            velocity.z = -velocity.z * bounciness;
        }
        else if (newPosition.z > allowedArea.yMax)
        {
            newPosition.z = allowedArea.yMax;
            velocity.z = -velocity.z * bounciness;
        }
        transform.localPosition = newPosition;
    }

    void normalGameMoveFunction()
    {
        // if (!allowedArea.Contains(new Vector2(newPosition.x, newPosition.z)))
        // {
        //     // newPosition = transform.localPosition;
        //     newPosition.x = Mathf.Clamp(newPosition.x, allowedArea.xMin, allowedArea.xMax);
        //     newPosition.z = Mathf.Clamp(newPosition.z, allowedArea.yMin, allowedArea.yMax);
        // }
    }
}


