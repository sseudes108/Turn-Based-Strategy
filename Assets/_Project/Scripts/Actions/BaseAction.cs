using UnityEngine;
using System;
using System.Collections.Generic;

public abstract class BaseAction : MonoBehaviour{
    protected Action onActionComplete;

    public static event EventHandler OnAnyActionStarted;
    public static event EventHandler OnAnyActionCompleted;
    protected AnimationPlayer _animator;

    protected Unit _unit;
    protected bool _isActive;
    
    
    protected virtual void Awake() {
        _unit = GetComponent<Unit>();
        _animator = GetComponent<AnimationPlayer>();
    }

    public abstract string GetActionName();

    public abstract void TakeAction(GridPosition gridposition, Action onActionComplete);

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

    public Unit GetUnit(){
        return _unit;
    }

    protected void OnActionCompleted(){
        onActionComplete();
        _isActive = false;
        OnAnyActionCompleted?.Invoke(this, EventArgs.Empty);
    }

    protected void OnActionStarted(){
        _isActive = true;
        OnAnyActionStarted?.Invoke(this, EventArgs.Empty);
    }
}
