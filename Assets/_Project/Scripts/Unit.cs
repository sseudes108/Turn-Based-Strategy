using UnityEngine;

public class Unit : MonoBehaviour{
    private Vector3 _targetPosition;

    private void Update() {
        float stoppingDistance = 0.1f;
        if(Vector3.Distance(transform.position, _targetPosition) > stoppingDistance){
            Vector3 direction = (_targetPosition - transform.position).normalized;
            float moveSpeed = 4f;
            transform.position += direction * moveSpeed * Time.deltaTime;
        }

        if(Input.GetKeyDown(KeyCode.T)){
            Move(new Vector3(4, 0, 4));
        }
    }

    private void Move(Vector3 targetPosition){
        _targetPosition = targetPosition;
    }
}
