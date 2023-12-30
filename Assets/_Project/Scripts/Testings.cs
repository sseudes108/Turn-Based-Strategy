using System;
using UnityEngine;

public class Testings : MonoBehaviour{
    [SerializeField] private Unit unit;
    private GridSystemVisual _gridvisual;

    private void OnEnable() {
        unit.OnUnitPositionChanged += Unit_UpdateGridVisuals;
        UnitActionSystem.Instance.OnUnitSelectedChanged += Unit_UpdateGridVisuals;
    }

    private void OnDisable() {
        unit.OnUnitPositionChanged -= Unit_UpdateGridVisuals;
        UnitActionSystem.Instance.OnUnitSelectedChanged -= Unit_UpdateGridVisuals;
    }
    
    private void Update() {
        if(Input.GetKeyDown(KeyCode.T)){
            
        }
    }
    private void Unit_UpdateGridVisuals(object sender, EventArgs empty){
        UpdateGridVisuals();
    }

    private void UpdateGridVisuals(){
        GridSystemVisual.Instance.HideAllGridPosition();
        GridSystemVisual.Instance.ShowGridPositionList(UnitActionSystem.Instance.GetSelectedUnit().GetMoveAction().GetValidActionGridPositionList());
    }
}
