using UnityEngine;

public class GranadeProjectile : MonoBehaviour{
    private Vector3 _targetPosition;
    
    private void Update() {
        Vector3 moveDir = (_targetPosition - transform.position).normalized;
        float moveSpeed = 15f;
        transform.position += moveDir * moveSpeed * Time.deltaTime;

        float reachedTargetDistance = 0.2f;
        if(Vector3.Distance(transform.position, _targetPosition) < reachedTargetDistance){
            Destroy(gameObject);
        }
    }

    public void SetUp(GridPosition targetGridPosition, Vector3 startPosition){
        transform.position = startPosition;
        _targetPosition = LevelGrid.Instance.GetWorldPosition(targetGridPosition);
    }
}
