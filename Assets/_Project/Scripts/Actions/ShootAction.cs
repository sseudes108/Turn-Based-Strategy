using System;
using System.Collections.Generic;
using UnityEngine;

public class ShootAction : BaseAction{

    private enum State{
        Aiming, Shooting, CoolOff,
    }

    private State _state;
    private float _stateTimer;
    
    private float _totalSpinAmount;
    private int _maxShootDistance = 7;

    private void Update() {
        if(!_isActive)return;

        _stateTimer -= Time.deltaTime;

        switch (_state){
            case State.Aiming:

                if(_stateTimer <= 0){
                    _state = State.Shooting;
                    float shootingTime = 0.1f;
                    _stateTimer = shootingTime;
                }
                break;
            case State.Shooting:
                if(_stateTimer <= 0){
                    _state = State.CoolOff;
                    float coolOfStateTime = 0.5f;
                    _stateTimer = coolOfStateTime;
                }
                break;
            case State.CoolOff:
                if(_stateTimer <= 0){
                    OnActionComplete();
                }
                break;
            default:
            break;
        }
    }

    public override string GetActionName(){
        return "Shoot";
    }

    public override List<GridPosition> GetValidActionGridPositionList(){
        List<GridPosition> validActionGridPositionList = new();

        GridPosition unitGridPosition = _unit.GetGridPosition();

        for (int x = -_maxShootDistance; x <= _maxShootDistance; x++){

            for (int z = -_maxShootDistance; z <= _maxShootDistance; z++){
                
                GridPosition offSetGridPosition = new(x,z);
                GridPosition testGridPosition = unitGridPosition + offSetGridPosition;

                if(!LevelGrid.Instance.IsValidGridPosition(testGridPosition)){continue;}

                int aimDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if(aimDistance > _maxShootDistance){
                    continue;
                }

                //Se não estiver ocupada, continue
                if(!LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition)){continue;}

                Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);

                if(targetUnit.IsEnemy() == _unit.IsEnemy()){
                    //Both units are in the same "Team"
                    continue;
                }
                                

                validActionGridPositionList.Add(testGridPosition);
            }
        }
        return validActionGridPositionList;
    }

    public override void TakeAction(GridPosition gridposition, Action<bool> onActionComplete_SetBusy){
        OnActionStart(onActionComplete_SetBusy);

        _state = State.Aiming;
        float aimingTime = 1f;
        _stateTimer = aimingTime;
    }
}
