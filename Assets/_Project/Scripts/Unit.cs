using System;
using UnityEngine;

public class Unit : MonoBehaviour{

    public event EventHandler OnUnitPositionChanged;
    private GridPosition _currentGridposition;
    private MoveAction _moveAction;
    private SpinAction _spinAction;
    private BaseAction[] _baseActionArray;
    private int _actionPoints = 2;

    private void Awake() {
        _moveAction = GetComponent<MoveAction>();
        _spinAction = GetComponent<SpinAction>();
        _baseActionArray = GetComponents<BaseAction>();
    }

    private void Start() {
        _currentGridposition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(_currentGridposition, this);
    }

    private void Update(){
        HandleGridPosition();
    }

    private void HandleGridPosition(){
        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        if (newGridPosition != _currentGridposition){
            LevelGrid.Instance.UnitMovedGridPosition(this, _currentGridposition, newGridPosition);
            OnUnitPositionChanged?.Invoke(this, EventArgs.Empty);
        }
        _currentGridposition = newGridPosition;
    }

    public MoveAction GetMoveAction(){
        return _moveAction;
    }

    public SpinAction GetSpinAction(){
        return _spinAction;
    }

    public GridPosition GetGridPosition(){
        return _currentGridposition;
    }

    public BaseAction[] GetBaseActionArray(){
        return _baseActionArray;
    }

    public bool TrySpendActionPointsToTakeAction(BaseAction baseAction){
        if(CanSpendActionPointsToTakeAction(baseAction)){
            SpendActionPoints(baseAction.GetActionPointsCost());
            return true;
        }else{
            return false;
        }
    }
    
    public bool CanSpendActionPointsToTakeAction(BaseAction baseAction){
        return _actionPoints >= baseAction.GetActionPointsCost();
    }
    private void SpendActionPoints(int amount){
        _actionPoints -= amount;
    }
    public int GetActionPoints(){
        return _actionPoints;
    }
}
