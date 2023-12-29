using System.Collections.Generic;
using UnityEngine;

public class MoveAction : MonoBehaviour{
    [SerializeField] private int maxMoveDistance;

    private Vector3 _targetPosition;
    private AnimationPlayer _animator;
    private Unit _unit;

    private void Awake() {
        _unit = GetComponent<Unit>();
        _animator = GetComponent<AnimationPlayer>();
        _targetPosition = transform.position;
    }

    private void Update(){
        HandleMovement();
    }

    private void HandleMovement(){
        float stoppingDistance = 0.1f;
        if (Vector3.Distance(transform.position, _targetPosition) > stoppingDistance){
            Vector3 direction = (_targetPosition - transform.position).normalized;
            float moveSpeed = 4f;
            transform.position += moveSpeed * Time.deltaTime * direction;

            float rotateSpeed = 7f;
            transform.forward = Vector3.Lerp(transform.forward, direction, rotateSpeed * Time.deltaTime);

            PlayRunAnimation();
        }else{
            PlayIdleAnimation();
        }
    }

    public void Move(GridPosition targetPosition){
        _targetPosition = LevelGrid.Instance.GetWorldPosition(targetPosition);
    }

    private void PlayRunAnimation(){
        _animator.ChangeAnimationState(_animator.RIFLE_RUN);
    }
    private void PlayIdleAnimation(){
        _animator.ChangeAnimationState(_animator.RIFLE_AIMING_IDLE);
    }

    public bool IsValidActionGridPosition(GridPosition gridPosition){
        List<GridPosition> validGridPositionList = GetValidActionGridPositionList();
        return validGridPositionList.Contains(gridPosition);
    }

    public List<GridPosition> GetValidActionGridPositionList(){
        List<GridPosition> validActionGridPositionList = new();

        GridPosition unitGridPosition = _unit.GetGridPosition();

        for (int x = -maxMoveDistance; x <= maxMoveDistance; x++){

            for (int z = -maxMoveDistance; z <= maxMoveDistance; z++){
                
                GridPosition offSetGridPosition = new(x,z);
                GridPosition testGridPosition = unitGridPosition + offSetGridPosition;

                if(!LevelGrid.Instance.IsValidGridPosition(testGridPosition)){continue;}
                if(unitGridPosition == testGridPosition){continue;}
                if(LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition)){continue;}

                validActionGridPositionList.Add(testGridPosition);
            }
        }
        return validActionGridPositionList;
    }
}
