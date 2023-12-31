using UnityEngine;
using System;

public abstract class BaseAction : MonoBehaviour{
    protected Action<bool> _onActionComplete_SetBusy;
    protected Unit _unit;
    protected AnimationPlayer _animator;
    protected bool _isActive;
    
    protected virtual void Awake() {
        _unit = GetComponent<Unit>();
        _animator = GetComponent<AnimationPlayer>();
    }

    protected void OnActionComplete(){
        _isActive = false;
        _onActionComplete_SetBusy(false);
    }
    protected void OnActionStart(Action<bool> onActionComplete_SetBusy){
        this._onActionComplete_SetBusy = onActionComplete_SetBusy;
        _isActive = true;
    }

    public abstract string GetActionName();
}
