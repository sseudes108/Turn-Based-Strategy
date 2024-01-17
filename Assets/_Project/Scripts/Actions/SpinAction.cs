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
            ActionComplete();
        }
    }
    public override void TakeAction(GridPosition gridposition, Action<bool> onActionComplete_SetBusy){
        _totalSpinAmount = 0;
        ActionStart(onActionComplete_SetBusy);
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
        return 2;
    }
}