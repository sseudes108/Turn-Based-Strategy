using System;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : BaseAction {
    [SerializeField] private int maxMoveDistance;

    private List<Vector3> _positionlist;
    private int _currentPositionIndex;

    private void Update(){
        if(!_isActive) return;
        HandleMovement();
    }

    private void HandleMovement(){
        float stoppingDistance = 0.1f;
        Vector3 targetPosition = _positionlist[_currentPositionIndex];
        Vector3 direction = (targetPosition - transform.position).normalized;

        float rotateSpeed = 33f;
        transform.forward = Vector3.Lerp(transform.forward, direction, rotateSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) > stoppingDistance){
            float moveSpeed = 4f;
            transform.position += moveSpeed * Time.deltaTime * direction;
            PlayRunAnimation();
        }else{
            _currentPositionIndex++;
            if(_currentPositionIndex >= _positionlist.Count){
                PlayIdleAnimation();
                OnActionCompleted();
            }
        }   
    }

    public override void TakeAction(GridPosition gridPosition, Action OnActionComplete){
        this.onActionComplete = OnActionComplete;

        List<GridPosition> pathGridPositionList = PathFinding.Instance.FindPath(_unit.GetGridPosition(), gridPosition, out int pathLenght);

        _currentPositionIndex = 0;
        _positionlist = new List<Vector3>();

        foreach(var pathGridPosition in pathGridPositionList){
            _positionlist.Add(LevelGrid.Instance.GetWorldPosition(pathGridPosition));
        }

        _isActive = true;
    }

    private void PlayRunAnimation(){
        _animator.ChangeAnimationState(_animator.RIFLE_RUN);
    }

    public override List<GridPosition> GetValidActionGridPositionList(){
        List<GridPosition> validActionGridPositionList = new();

        GridPosition unitGridPosition = _unit.GetGridPosition();

        for (int x = -maxMoveDistance; x <= maxMoveDistance; x++){

            for (int z = -maxMoveDistance; z <= maxMoveDistance; z++){
                
                GridPosition offSetGridPosition = new(x,z);
                GridPosition testGridPosition = unitGridPosition + offSetGridPosition;

                if(!LevelGrid.Instance.IsValidGridPosition(testGridPosition)){continue;}

                //if where the unit is
                if(unitGridPosition == testGridPosition){continue;}

                //ocupied
                if(LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition)){continue;}

                //Not walkable
                if(!PathFinding.Instance.IsWalkableGridPosition(testGridPosition)){continue;}

                //Any path to destination
                if(!PathFinding.Instance.HasPath(unitGridPosition, testGridPosition)){continue;}

                int pathFindingDistanceMultiplier = 10;
                if(PathFinding.Instance.GetPathLenght(unitGridPosition, testGridPosition) > maxMoveDistance * pathFindingDistanceMultiplier){
                    //too far
                    continue;
                }

                validActionGridPositionList.Add(testGridPosition);
            }
        }
        return validActionGridPositionList;
    }

    public override string GetActionName(){
        return "Move";
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition){
        int targetCountAtGridPosition = _unit.GetAction<ShootAction>().GetTargetCountAtGridPosition(gridPosition);
        return new EnemyAIAction{
            _gridPosition = gridPosition,
            _actionValue = targetCountAtGridPosition * 10,
        };
    }
}
