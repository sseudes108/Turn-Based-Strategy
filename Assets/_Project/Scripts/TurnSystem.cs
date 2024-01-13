using System;
using UnityEngine;

public class TurnSystem : MonoBehaviour{
    public static TurnSystem Instance {get; private set;}
    public static Action OnTurnEnd;
    private int _turnNumber = 1;
    private bool _isPlayerTurn = true;

    private void Awake() {
        if(Instance != null){Debug.Log("Error: More than one TurnSystem Instance!" + transform + Instance); Destroy(gameObject);}
        Instance = this;
    }

    public void NextTurn(){
        _turnNumber++;
        _isPlayerTurn =! _isPlayerTurn;
        OnTurnEnd.Invoke();
    }

    public int GetTurnNumber(){
        return _turnNumber;
    }

    public bool IsPlayerTurn(){
        return _isPlayerTurn;
    }
}