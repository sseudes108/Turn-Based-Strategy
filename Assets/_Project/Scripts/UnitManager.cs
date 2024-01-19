using System;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour{
    public static UnitManager Instance;

    private List<Unit> _unitList;
    private List<Unit> _friendlyUnitList;
    private List<Unit> _enemyUnitList;

    private void OnEnable() {
        Unit.OnAnyUnitSpawn += Unit_OnAnyUnitSpawn;
        Unit.OnAnyUnitDead += Unit_OnAnyUnitDead;
    }

    private void OnDisable() {
        Unit.OnAnyUnitSpawn -= Unit_OnAnyUnitSpawn;
        Unit.OnAnyUnitDead -= Unit_OnAnyUnitDead;
    }

    private void Awake() {
        if(Instance != null){
            Debug.Log("There's more than one UnitManager" + transform + "-" + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;

        _unitList = new();
        _friendlyUnitList = new();
        _enemyUnitList = new();
    }

    private void Unit_OnAnyUnitSpawn(object sender, EventArgs e){
        Unit unit = sender as Unit;

        _unitList.Add(unit);
        if(unit.IsEnemy()){
            _enemyUnitList.Add(unit);
        }else{
            _friendlyUnitList.Add(unit);
        }
    }

    private void Unit_OnAnyUnitDead(object sender, EventArgs e){
        Unit unit = sender as Unit;

        _unitList.Remove(unit);
        if(unit.IsEnemy()){
            _enemyUnitList.Remove(unit);
        }else{
            _friendlyUnitList.Remove(unit);
        }
    }

    public List<Unit> GetUnitList() => _unitList;
    public List<Unit> GetFriendlyUnitList() => _friendlyUnitList;
    public List<Unit> GetEnemyUnitList() => _enemyUnitList;
}
