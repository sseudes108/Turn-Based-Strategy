using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEditor;

public abstract class BaseAction : MonoBehaviour{
    public Action<bool> _onActionComplete_SetBusy;
    protected Unit _unit;
    protected AnimationPlayer _animator;
    protected bool _isActive;
    
    protected virtual void Awake() {
        _unit = GetComponent<Unit>();
        _animator = GetComponent<AnimationPlayer>();
    }

    protected void ActionComplete(){
        _isActive = false;
        _onActionComplete_SetBusy(false);
    }
    protected void ActionStart(Action<bool> onActionComplete_SetBusy){
        this._onActionComplete_SetBusy = onActionComplete_SetBusy;
        _isActive = true;
    }

    public abstract string GetActionName();

    public abstract void TakeAction(GridPosition gridposition, Action<bool> onActionComplete);

    public virtual bool IsValidActionGridPosition(GridPosition gridPosition){
        List<GridPosition> validGridPositionList = GetValidActionGridPositionList();
        return validGridPositionList.Contains(gridPosition);
    }
    public abstract List<GridPosition> GetValidActionGridPositionList();

    public virtual int GetActionPointsCost(){
        return 1;
    }

    protected void PlayIdleAnimation(){
        _animator.ChangeAnimationState(_animator.RIFLE_AIMING_IDLE);
    }
}
