using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UnitActionSystemUI : MonoBehaviour{

    [SerializeField] private Transform  _actionButtonPrefab;
    [SerializeField] private Transform  _actionButtonConteinerTransform;

    private void OnEnable() {
        UnitActionSystem.Instance.OnUnitSelectedChanged += UnitActionSystem_OnUnitSelectedChanged;
        UnitActionSystem.Instance.OnUnitActionChanged += UnitActionSystem_OnUnitActionChanged;
    }
    private void OnDisable() {
        UnitActionSystem.Instance.OnUnitSelectedChanged -= UnitActionSystem_OnUnitSelectedChanged;
        UnitActionSystem.Instance.OnUnitActionChanged -= UnitActionSystem_OnUnitActionChanged;
    }

    private void Start() {
        CreateUnitActionButtons();
        UpdateSelectedVisuals();
    }

    private void CreateUnitActionButtons(){
        DestroyActionButtos();
        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();
        BaseAction[] selectedUnitBaseActionArray = selectedUnit.GetBaseActionArray();
        foreach(BaseAction baseAction in selectedUnitBaseActionArray){
            Transform actionButtonPrefabUITransform = Instantiate(_actionButtonPrefab, _actionButtonConteinerTransform);
            ActionButtonUI actionButtonUI = actionButtonPrefabUITransform.GetComponent<ActionButtonUI>();
            actionButtonUI.SetBaseAction(baseAction);
        }
    }

    private void DestroyActionButtos(){
        foreach(Transform actionButtonTransform in _actionButtonConteinerTransform){
            Destroy(actionButtonTransform.gameObject);
        }
    }

    private void UnitActionSystem_OnUnitSelectedChanged(object sender, EventArgs e){
        CreateUnitActionButtons();
    }

    private void UnitActionSystem_OnUnitActionChanged(object sender, EventArgs e){
        UpdateSelectedVisuals();
    }

    private void UpdateSelectedVisuals(){
        BaseAction selectedAction = UnitActionSystem.Instance.GetSelectedAction();
        foreach(Transform button in _actionButtonConteinerTransform){
            ActionButtonUI actionButtonUI = button.GetComponent<ActionButtonUI>();
            if(actionButtonUI.GetActionName() == selectedAction.GetActionName().ToUpper()){
                actionButtonUI.EnableSelectedVisual(true);
            }else{
                actionButtonUI.EnableSelectedVisual(false);
            }
        }
    }
}
