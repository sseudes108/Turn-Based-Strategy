using Cinemachine;
using UnityEngine;

public class CameraMovement : MonoBehaviour{
    private const float MIN_FOLLOW_Y_OFFSET = 2f;
    private const float MAX_FOLLOW_Y_OFFSET = 12f;
    
    [SerializeField] private float _moveSpeed = 10f;
    [SerializeField] private float _rotationSpeed = 5f;
    [SerializeField] private float _zoomSpeed = 5f;
    [SerializeField] private CinemachineVirtualCamera _cinemachine;

    private CinemachineTransposer _cinemachineTransposer;
    private Vector3 _targetFollowOffset;

    private void Start() {
        _cinemachineTransposer = _cinemachine.GetCinemachineComponent<CinemachineTransposer>();
        _targetFollowOffset = _cinemachineTransposer.m_FollowOffset;
    }

    private void Update(){
        HandleMovement();
        HandleRotation();
        HandleZoom();
    }

    private void HandleMovement(){
        Vector3 moveInputDirection = Vector3.zero;

        if (Input.GetKey(KeyCode.W)){
            moveInputDirection.z = +1;
        };

        if (Input.GetKey(KeyCode.S)){
            moveInputDirection.z = -1;
        };

        if (Input.GetKey(KeyCode.A)){
            moveInputDirection.x = -1;
        }

        if (Input.GetKey(KeyCode.D)){
            moveInputDirection.x = +1;
        }

        Vector3 move = transform.forward * moveInputDirection.z + transform.right * moveInputDirection.x;
        transform.position += _moveSpeed * Time.deltaTime * move;
    }

    private void HandleRotation(){
        Vector3 rotation = Vector3.zero;

        if (Input.GetKey(KeyCode.Q)){
            rotation.y = +1;
        };

        if (Input.GetKey(KeyCode.E)){
            rotation.y = -1;
        };

        transform.eulerAngles += _rotationSpeed * Time.deltaTime * rotation;
    }
    
    private void HandleZoom(){
        float zoomAmount = 1;

        if (Input.mouseScrollDelta.y > 0){
            _targetFollowOffset.y -= zoomAmount;
        }

        if (Input.mouseScrollDelta.y < 0){
            _targetFollowOffset.y += zoomAmount;
        }

        _targetFollowOffset.y = Mathf.Clamp(_targetFollowOffset.y, MIN_FOLLOW_Y_OFFSET, MAX_FOLLOW_Y_OFFSET);
        _cinemachineTransposer.m_FollowOffset = Vector3.Lerp(_cinemachineTransposer.m_FollowOffset, _targetFollowOffset, Time.deltaTime * _zoomSpeed);
    }

    
}

