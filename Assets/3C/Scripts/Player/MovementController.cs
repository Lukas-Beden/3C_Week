using Unity.Cinemachine;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [Header("Scripts")]
    [SerializeField] InputHandler _inputHandler;
    [SerializeField] CinemachineInputAxisController _axisController;

    [Header("Movement")]
    int _moveSpeed = 10;

    [Header("Camera")]
    //[SerializeField] CinemachineCamera _mainCamera;
    float rotationSpeed = 360f;

    [Header("Jump")]
    [SerializeField] LayerMask groundLayer;
    int jumpForce = 5;

    [Header("ZZZ")]
    [SerializeField] CinemachineCamera _cinemachineCam;
    [SerializeField] CinemachineTargetGroup _targetGroup;
    [SerializeField] GameObject _obstacle;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        Move(); 
        Jump();
        LockOn();
    }

    void Move()
    {
        Vector2 move = _inputHandler.Move;

        if (move != Vector2.zero)
        {
            Vector3 camForward = Camera.main.transform.forward;
            Vector3 camRight = Camera.main.transform.right;
            camForward.y = 0f;
            camRight.y = 0f;
            camForward.Normalize();
            camRight.Normalize();

            Vector3 moveDirection = camForward * move.y + camRight * move.x;

            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );

            transform.position += moveDirection * (_moveSpeed * Time.deltaTime);
        }
    }

    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 0.5f, groundLayer);
    }

    void Jump()
    {
        if (_inputHandler.Jump && IsGrounded())
        {
            Rigidbody rb = gameObject.GetComponent<Rigidbody>();
            rb.linearVelocity = new Vector3(0, jumpForce, 0);
        }
    }

    void LockOn()
    {
        if (_inputHandler.LockOn)
        {
            _targetGroup.AddMember(_obstacle.transform, 1, 5);
            _axisController.enabled = false;
        } else
        {
            _targetGroup.RemoveMember(_obstacle.transform);
            _axisController.enabled = true;
        }
    }
}
