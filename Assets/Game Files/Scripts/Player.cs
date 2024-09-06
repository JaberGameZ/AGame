using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int Coins;
    [SerializeField] float Speed;
    [SerializeField] float JumpForce;
    [SerializeField] float GroundDistance;

    [SerializeField] Transform GroundCheck;
    [SerializeField] LayerMask GroundMask;

    CharacterController controller;
    Vector3 Velocity;
    bool isGrounded;
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Jump();
        ApplyGravity();
        JumpAndFallMechanic();
    }

    void Move()
    {
        float XAxis = Input.GetAxis("Horizontal") * Speed * Time.deltaTime;
        float ZAxis = Input.GetAxis("Vertical") * Speed * Time.deltaTime;
        Vector3 Movement = new Vector3(XAxis, 0, ZAxis);
        controller.Move(Movement);
    }

    void Jump()
    {
        //Note: Note fix the unlimited jumps
        if(Input.GetKeyDown(KeyCode.Space) && controller.isGrounded)
        {
            Vector3 JumpVector = new Vector3(0, JumpForce, 0) * Time.deltaTime;
            controller.Move(JumpVector * Time.deltaTime);
        }
    }

    void JumpAndFallMechanic()
    {
        isGrounded = Physics.CheckSphere(GroundCheck.position, GroundDistance, GroundMask);
        if(isGrounded && Velocity.y < 0)
        {
            Velocity.y = -2f;
        }

        if(Input.GetKey(KeyCode.Space))
        {
            Velocity.y = Mathf.Sqrt(-2f * JumpForce * -9.81f);
        }
    }

    void ApplyGravity()
    {
        //Note: the player is falling too slow idk why
        Velocity.y += -80 * Time.deltaTime;
        controller.Move(Velocity * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("Ground"))
        {
            //Flag so the player loses when touches the blue platform
            Debug.Log("Player Lost");
        }
    }
}
