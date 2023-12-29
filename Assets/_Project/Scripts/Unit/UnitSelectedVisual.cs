using System;
using UnityEngine;

public class UnitSelectedVisual : MonoBehaviour{
    [SerializeField] private Unit _unit;

    private MeshRenderer _meshRenderer;

    private void Awake() {
        _meshRenderer =  GetComponent<MeshRenderer>();
    }

    private void OnEnable() {
        UnitActionSystem.Instance.OnUnitSelectedChanged += UnitActionSystem_OnUnitSelectedChanged;
    }
    private void OnDisable() {
        UnitActionSystem.Instance.OnUnitSelectedChanged -= UnitActionSystem_OnUnitSelectedChanged;
    }

    private void Start() {
        UpdateVisual();
    }

    private void UnitActionSystem_OnUnitSelectedChanged(object sender, EventArgs empty){
        UpdateVisual();
    }

    private void UpdateVisual(){
        if(_unit == UnitActionSystem.Instance.GetSelectedUnit()){
            _meshRenderer.enabled = true;
        }else{
            _meshRenderer.enabled = false;
        }
    }
}
