using System;
using UnityEngine;

public class Unit : MonoBehaviour{
    private const int ACTION_POINTS_MAX = 2;
    public static event EventHandler OnAnyActionPointsChanged;
    public static event EventHandler OnAnyUnitSpawn;
    public static event EventHandler OnAnyUnitDead;

    public event EventHandler OnUnitPositionChanged;
    private GridPosition _currentGridposition;
    
    private HealthSystem _healthSystem;
    private BaseAction[] _baseActionArray;
    private int _actionPoints = ACTION_POINTS_MAX;

    [SerializeField] private bool _isEnemy;

    private void OnEnable() {
        TurnSystem.OnTurnChanged += TurnSystem_OnTurnEnd;
        _healthSystem.OnDead += HealhSystem_OnDead;
    }
    private void OnDisable() {
        TurnSystem.OnTurnChanged -= TurnSystem_OnTurnEnd;
        _healthSystem.OnDead -= HealhSystem_OnDead;
    }

    private void Awake() {
        _healthSystem = GetComponent<HealthSystem>();
        _baseActionArray = GetComponents<BaseAction>();
    }

    private void Start() {
        _currentGridposition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(_currentGridposition, this);
        OnAnyUnitSpawn?.Invoke(this, EventArgs.Empty);
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

    public T GetAction<T>() where T : BaseAction{
        foreach(BaseAction baseAction in _baseActionArray){
            if (baseAction is T){
                return (T)baseAction;
            }
        }
        return null;
    }

    public GridPosition GetGridPosition() => _currentGridposition;
    public BaseAction[] GetBaseActionArray() => _baseActionArray;

    public bool IsEnemy(){
        return _isEnemy;
    }

    public Vector3 GetWorldPosition(){
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
        OnAnyUnitDead?.Invoke(this, EventArgs.Empty);
    }
}