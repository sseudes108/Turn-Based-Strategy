using System;
using UnityEngine;

public class EnemyAI : MonoBehaviour{

    public enum EnemyState{
        Waiting, TakingTurn, Busy
    }

    private EnemyState _state;

    private float _timer;

    private void OnEnable() {
        TurnSystem.OnTurnEnd += TurnSystem_OnTurnEnd;
    }

    private void OnDisable() {
        TurnSystem.OnTurnEnd -= TurnSystem_OnTurnEnd;
    }

    private void Awake() {
        _state = EnemyState.Waiting;
    }

    private void Update(){
        if (TurnSystem.Instance.IsPlayerTurn()){
            return;
        }


        switch(_state){
            case EnemyState.Waiting:

            break;
            case EnemyState.TakingTurn:
                _timer -= Time.deltaTime;
                if(_timer <= 0f){
                    _state = EnemyState.Busy;
                    //TakeEnemyAIAction(SetStateTakingTurn);
                    TakeEnemyAIAction();
                    TurnSystem.Instance.NextTurn();
                }
            break;
            case EnemyState.Busy:
                
            break;
        
        }

        _timer -= Time.deltaTime;
        if(_timer <=0f){
            TurnSystem.Instance.NextTurn();
        }
    }

    private void SetStateTakingTurn(){
        _timer = 0.5f;
        _state = EnemyState.TakingTurn;
    }

    private void TurnSystem_OnTurnEnd(){
        if(!TurnSystem.Instance.IsPlayerTurn()){
            _state = EnemyState.TakingTurn;
            _timer = 2f;    
        }
    }

    // private void TakeEnemyAIAction(Action onEnemyActionComplete){
    //     foreach(Unit enemyUnit in UnitManager.Instance.GetEnemyUnitList()){
    //         TakeEnemyAIAction(enemyUnit, onEnemyActionComplete);
    //     }
    // }

    // private void TakeEnemyAIAction(Unit enemyUnit, Action onEnemyActionComplete){
    //     SpinAction spinAction = enemyUnit.GetSpinAction();

    //     GridPosition actionGridPosition = enemyUnit.GetGridPosition();

    //         if(spinAction.IsValidActionGridPosition(actionGridPosition)){

    //         if(!enemyUnit.TrySpendActionPointsToTakeAction(spinAction)){return;}
            
    //         //SetBusy(true);
    //         spinAction.TakeAction(actionGridPosition, null);
    //         //OnActionStart?.Invoke();
    //     }
    // }

    private void TakeEnemyAIAction(){
        foreach(Unit enemyUnit in UnitManager.Instance.GetEnemyUnitList()){
            TakeEnemyAIAction(enemyUnit);
        }
    }

    private void TakeEnemyAIAction(Unit enemyUnit){
        SpinAction spinAction = enemyUnit.GetSpinAction();

        GridPosition actionGridPosition = enemyUnit.GetGridPosition();

            if(spinAction.IsValidActionGridPosition(actionGridPosition)){

            if(!enemyUnit.TrySpendActionPointsToTakeAction(spinAction)){return;}
            
            //SetBusy(true);
            spinAction.TakeAction(actionGridPosition, null);
            //OnActionStart?.Invoke();
            SetStateTakingTurn();
        }
    }
}
