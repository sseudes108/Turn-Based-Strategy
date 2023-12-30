using System;
using UnityEngine;

public class Unit : MonoBehaviour{

    public event EventHandler OnUnitPositionChanged;
    private GridPosition _currentGridposition;
    private MoveAction _moveAction;

    private void Awake() {
        _moveAction = GetComponent<MoveAction>();
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

    public GridPosition GetGridPosition(){
        return _currentGridposition;
    }
}
