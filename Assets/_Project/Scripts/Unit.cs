using System;
using UnityEngine;

public class Unit : MonoBehaviour{
    private const int ACTION_POINTS_MAX = 2;
    public static event EventHandler OnAnyActionPointsChanged;

    public event EventHandler OnUnitPositionChanged;
    private GridPosition _currentGridposition;
    
    private HealthSystem _healthSystem;
    private MoveAction _moveAction;
    private SpinAction _spinAction;
    private BaseAction[] _baseActionArray;
    private int _actionPoints = ACTION_POINTS_MAX;

    [SerializeField] private bool _isEnemy;

    private void OnEnable() {
        TurnSystem.OnTurnEnd += TurnSystem_OnTurnEnd;
        _healthSystem.OnDead += HealhSystem_OnDead;
    }
    private void OnDisable() {
        TurnSystem.OnTurnEnd -= TurnSystem_OnTurnEnd;
        _healthSystem.OnDead -= HealhSystem_OnDead;
    }

    private void Awake() {
        _healthSystem = GetComponent<HealthSystem>();
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

    public bool IsEnemy(){
        return _isEnemy;
    }

    public Vector3 GetWordPosition(){
        return LevelGrid.Instance.GetWorldPosition(_currentGridposition);
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
        OnAnyActionPointsChanged.Invoke(this, EventArgs.Empty);
    }
    
    //Reset Action points
    private void TurnSystem_OnTurnEnd(){
        if (IsEnemy() && !TurnSystem.Instance.IsPlayerTurn() || !IsEnemy() && TurnSystem.Instance.IsPlayerTurn()){
            _actionPoints = ACTION_POINTS_MAX;
            OnAnyActionPointsChanged.Invoke(this, EventArgs.Empty);
        }
    }

    public int GetActionPoints(){
        return _actionPoints;
    }

    public void Damage(int damageAmount){
        _healthSystem.Damage(damageAmount);
    }

    private void HealhSystem_OnDead(object sender, EventArgs e){
        LevelGrid.Instance.RemoveUnitAtGridPosition(_currentGridposition, this);
        Destroy(gameObject);
    }
}