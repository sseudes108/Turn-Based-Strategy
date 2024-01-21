using System;
using UnityEngine;

public class EnemyAI : MonoBehaviour{

    public enum EnemyState{
        WaitingForEnemyTurn, TakingTurn, Busy
    }

    private EnemyState _state;

    private float _timer;

    private void OnEnable() {
        TurnSystem.OnTurnChanged += TurnSystem_OnTurnChanged;
    }

    private void OnDisable() {
        TurnSystem.OnTurnChanged -= TurnSystem_OnTurnChanged;
    }

    private void Awake() {
        _state = EnemyState.WaitingForEnemyTurn;
    }

    private void Update(){
        if (TurnSystem.Instance.IsPlayerTurn()){
            return;
        }

        switch(_state){
            case EnemyState.WaitingForEnemyTurn:
            break;

            case EnemyState.TakingTurn:
                _timer -= Time.deltaTime;
                if(_timer <= 0f){
                    
                    if(TryTakeEnemyAIAction(SetStateTakingTurn)){
                        _state = EnemyState.Busy;
                    }else{
                        //No more enemies have actions that can take
                        TurnSystem.Instance.NextTurn();
                    }
                }
            break;

            case EnemyState.Busy:
            break;
        }
    }

    private void SetStateTakingTurn(){
        _timer = 0.5f;
        _state = EnemyState.TakingTurn;
    }

    private void TurnSystem_OnTurnChanged(){
        if(!TurnSystem.Instance.IsPlayerTurn()){
            _state = EnemyState.TakingTurn;
            _timer = 2f;    
        }
    }

    private bool TryTakeEnemyAIAction(Action onEnemyAIActionComplete){
        Debug.Log("Enemy Action");
        foreach(var enemyUnit in UnitManager.Instance.GetEnemyUnitList()){
            if(TryTakeEnemyAIAction(enemyUnit, onEnemyAIActionComplete)){
                return true;
            };
        }
        return false;
    }

    private bool TryTakeEnemyAIAction(Unit enemyUnit, Action onEnemyAIActionComplete){
        SpinAction spinAction = enemyUnit.GetSpinAction();

        GridPosition actionGridPosition = enemyUnit.GetGridPosition();
        
        if(!spinAction.IsValidActionGridPosition(actionGridPosition)){return false;}

        if(!enemyUnit.TrySpendActionPointsToTakeAction(spinAction)){return false;}

        Debug.Log("Spin Action");
        spinAction.TakeAction(actionGridPosition, onEnemyAIActionComplete);
        return true;
    }
}
