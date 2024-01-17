using UnityEngine;

public class LookAtCamera : MonoBehaviour{
    private Transform _cameraTransform;

    private void Awake() {
        _cameraTransform = Camera.main.transform;
    }

    private void LateUpdate() {
        transform.LookAt(_cameraTransform);
    }
}
