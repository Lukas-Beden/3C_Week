using System.Collections.Generic;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class MovementAC : MonoBehaviour
{
    [SerializeField] InputHandler _inputHandler;

    [Header("Cinemachine Camera")]
    CinemachineCamera _camera;

    [SerializeField] List<CinemachineCamera> _cameraAC = new();

    [SerializeField] GameObject _cameraLookAtAC;

    float _moveSpeed = 10f;
    float _rotationSpeed = 10f;
    [SerializeField] float _cameraMoveSpeed = 10f;

    int typeCamAC = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _camera = _cameraAC[0];
    }

    // Update is called once per frame
    void Update()
    {
        MoveAC();
        ChangeCamType();
    }

    void MoveAC()
    {
        Vector2 move = _inputHandler.Move;

        Vector3 moveDirection = _camera.transform.forward * move.y + _camera.transform.right * move.x;
        moveDirection.y = 0;

        Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);

        transform.position += moveDirection.normalized * (_moveSpeed * Time.deltaTime);
    }
    void ChangeCamType()
    {
        if (_inputHandler.ChangeCam > 0)
        {
            typeCamAC++;
        }
        else if (_inputHandler.ChangeCam < 0)
        {
            typeCamAC--;
        }
        typeCamAC = (typeCamAC % _cameraAC.Count + _cameraAC.Count) % _cameraAC.Count;
        _camera = _cameraAC[typeCamAC];
        for (int i = 0; i < _cameraAC.Count; i++)
        {
            if (i == typeCamAC)
            {
                _cameraAC[i].Priority = 20;
            }
            else
            {
                _cameraAC[i].Priority = 0;
            }
        }
    }
}
