using System.Threading;
using UnityEngine;

public class EnemyAI : MonoBehaviour{
    private float _timer;

    private void OnEnable() {
        TurnSystem.OnTurnEnd += TurnSystem_OnTurnEnd;
    }

    private void OnDisable() {
        TurnSystem.OnTurnEnd -= TurnSystem_OnTurnEnd;
    }

    private void Update(){
        if (TurnSystem.Instance.IsPlayerTurn()){
            return;
        }

        _timer -= Time.deltaTime;
        if(_timer <=0f){
            TurnSystem.Instance.NextTurn();
        }
    }

    private void TurnSystem_OnTurnEnd(){
        _timer = 2f;
    }
}
