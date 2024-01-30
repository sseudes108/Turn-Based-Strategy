using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GranadeAction : BaseAction
{
    private int _maxThrowDistance = 7;

    [SerializeField] private Transform _grandeProjectilePrefab;

    void Update()
    {
        if(!_isActive){
            return;
        }

        OnActionCompleted();
    }

    public override string GetActionName()
    {
        return "Granade";
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction{
            _gridPosition = gridPosition,
            _actionValue = 0,
        };
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validActionGridPositionList = new();
        var unitGridPosition = _unit.GetGridPosition();

        for (int x = -_maxThrowDistance; x <= _maxThrowDistance; x++){
            for (int z = -_maxThrowDistance; z <= _maxThrowDistance; z++){
                GridPosition offSetGridPosition = new(x,z);
                GridPosition testGridPosition = unitGridPosition + offSetGridPosition;

                if(!LevelGrid.Instance.IsValidGridPosition(testGridPosition)){continue;}

                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if(testDistance > _maxThrowDistance){continue;}

                validActionGridPositionList.Add(testGridPosition);
            }
        }
        return validActionGridPositionList;
    }

    public override void TakeAction(GridPosition gridposition, Action onActionComplete)
    {
        var n = _unit.transform.position;
        var n2 = _unit.GetWorldPosition();
        this.onActionComplete = onActionComplete;
        var granadeProjectileTransform = Instantiate(_grandeProjectilePrefab);
        var granade = granadeProjectileTransform.GetComponent<GranadeProjectile>();
        granade.SetUp(gridposition, _unit.GetWorldPosition());
        OnActionStarted();
    }
}
