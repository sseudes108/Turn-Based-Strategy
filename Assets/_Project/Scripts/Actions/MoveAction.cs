using System;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : BaseAction {
    [SerializeField] private int maxMoveDistance;

    private Vector3 _targetPosition;

    protected override void Awake(){
        base.Awake();
        _targetPosition = transform.position;
    }

    private void Update(){
        if(!_isActive) return;
        HandleMovement();
    }

    private void HandleMovement(){
        float stoppingDistance = 0.1f;
        if (Vector3.Distance(transform.position, _targetPosition) > stoppingDistance){
            float moveSpeed = 4f;
            transform.position += moveSpeed * Time.deltaTime * Direction();
            PlayRunAnimation();
        }else{
            PlayIdleAnimation();
            OnActionComplete();
        }
        float rotateSpeed = 33f;
        transform.forward = Vector3.Lerp(transform.forward, Direction(), rotateSpeed * Time.deltaTime);
    }

    private Vector3 Direction(){
        return (_targetPosition - transform.position).normalized;
    }

    public override void TakeAction(GridPosition targetPosition, Action<bool> onActionComplete_SetBusy){
        OnActionStart(onActionComplete_SetBusy);
        _targetPosition = LevelGrid.Instance.GetWorldPosition(targetPosition);
    }

    private void PlayRunAnimation(){
        _animator.ChangeAnimationState(_animator.RIFLE_RUN);
    }
    private void PlayIdleAnimation(){
        _animator.ChangeAnimationState(_animator.RIFLE_AIMING_IDLE);
    }

    public override List<GridPosition> GetValidActionGridPositionList(){
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

    public override string GetActionName(){
        return "Move";
    }
}
