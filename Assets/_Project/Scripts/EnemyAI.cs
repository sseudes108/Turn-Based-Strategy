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
        foreach(var enemyUnit in UnitManager.Instance.GetEnemyUnitList()){
            if(TryTakeEnemyAIAction(enemyUnit, onEnemyAIActionComplete)){
                return true;
            };
        }
        return false;
    }

    private bool TryTakeEnemyAIAction(Unit enemyUnit, Action onEnemyAIActionComplete){

        EnemyAIAction bestEnemyAiAction = null;
        BaseAction bestBaseAction = null;

        foreach(BaseAction baseAction in enemyUnit.GetBaseActionArray()){

            if(!enemyUnit.CanSpendActionPointsToTakeAction(baseAction)){
                continue;
            }

            if(bestEnemyAiAction == null){
                bestEnemyAiAction = baseAction.GetBestEnemyAIAction();
                bestBaseAction = baseAction;
            }else{
                EnemyAIAction testEnemyAIAction = baseAction.GetBestEnemyAIAction();
                if(testEnemyAIAction != null && testEnemyAIAction._actionValue > bestEnemyAiAction._actionValue){
                    bestEnemyAiAction = testEnemyAIAction;
                    bestBaseAction = baseAction;
                }
            }
        }

        if(bestEnemyAiAction != null && enemyUnit.TrySpendActionPointsToTakeAction(bestBaseAction)){
            bestBaseAction.TakeAction(bestEnemyAiAction._gridPosition, onEnemyAIActionComplete);
            return true;
        }else{
            return false;
        }
    }
}
