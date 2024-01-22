using System;
using System.Collections.Generic;
using UnityEngine;

public class SpinAction : BaseAction{
      
    private float _totalSpinAmount;
    private void Update() {        
        if(!_isActive)return;

        float spinAmount = 360f * Time.deltaTime;
        transform.eulerAngles += new Vector3(0, spinAmount, 0);
        
        _totalSpinAmount += spinAmount;
        if(_totalSpinAmount >= 360){
            OnActionCompleted();
        }
    }
    public override void TakeAction(GridPosition gridposition, Action OnActionComplete){
        this.onActionComplete = OnActionComplete;
        _totalSpinAmount = 0f;
        OnActionStarted();
    }

    public override string GetActionName(){
        return "Spin";
    }

    public override List<GridPosition> GetValidActionGridPositionList(){
        GridPosition unitGridposition = _unit.GetGridPosition();
        return new List<GridPosition>{
            unitGridposition
        };
    }

    public override int GetActionPointsCost(){
        return 1;
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition){
        return new EnemyAIAction{
            _gridPosition = gridPosition,
            _actionValue = 0,
        };
    }
}