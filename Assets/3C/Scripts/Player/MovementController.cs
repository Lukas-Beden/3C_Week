using Unity.Cinemachine;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [Header("Scripts")]
    [SerializeField] InputHandler _inputHandler;

    [Header("Movement")]
    int _moveSpeed = 5;

    [Header("Camera")]
    [SerializeField] CinemachineCamera _mainCamera;
    float rotationSpeed = 720f;

    [Header("Jump")]
    [SerializeField] LayerMask groundLayer;
    int jumpForce = 5;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        //Look();
        Jump();
    }

    void Move()
    {
        Vector2 move = _inputHandler.Move;

        if (move != Vector2.zero)
        {
            Quaternion targetRotation = Quaternion.Euler(0, _mainCamera.transform.eulerAngles.y, 0);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            Vector3 camForward = _mainCamera.transform.forward;
            Vector3 camRight = _mainCamera.transform.right;
            camForward.y = 0f;
            camRight.y = 0f;
            camForward.Normalize();
            camRight.Normalize();

            Vector3 moveDirection = camForward * move.y + camRight * move.x;
            transform.position += moveDirection * (_moveSpeed * Time.deltaTime);
        }
    }



    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 1.1f, groundLayer);
    }

    void Jump()
    {
        if (_inputHandler.Jump && IsGrounded())
        {
            Rigidbody rb = gameObject.GetComponent<Rigidbody>();
            rb.linearVelocity = new Vector3(0, jumpForce, 0);
        }
    }
}
