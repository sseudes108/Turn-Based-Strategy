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
            OnActionComplete();
            // _isActive = false;
            // _onActionComplete_SetBusy(false);
        }
    }
    public override void TakeAction(GridPosition gridposition, Action<bool> onActionComplete_SetBusy){
        OnActionStart(onActionComplete_SetBusy);
        // this._onActionComplete_SetBusy = _onActionComplete_SetBusy;
        // _isActive = true;
        _totalSpinAmount = 0;
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
}