using Unity.Cinemachine;
using UnityEngine;

public class Movements : MonoBehaviour
{
    [SerializeField] InputHandler _inputHandler;
    [SerializeField] CinemachineCamera _cameraAC;
    [SerializeField] GameObject _cameraLookAtAC;

    [SerializeField] int movementType = 1;
    float _moveSpeed = 10f;
    float rotationSpeed = 10f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (movementType == 1)
        {
            MoveAC();
        }
    }

    void MoveAC()
    {
        Vector2 move = _inputHandler.Move;

        Vector3 moveDirection = _cameraAC.transform.forward * move.y + _cameraAC.transform.right * move.x;
        moveDirection.y = 0;

        if (moveDirection != Vector3.zero)
        {
            _cameraLookAtAC.transform.position = new Vector3(moveDirection.x + transform.position.x, 0f, moveDirection.z + transform.position.z);
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            transform.position += moveDirection.normalized * (_moveSpeed * Time.deltaTime);
        } else
        {
            _cameraLookAtAC.transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);
        }

    }
}
