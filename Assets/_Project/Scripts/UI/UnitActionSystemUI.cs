using System;
using TMPro;
using UnityEngine;

public class UnitActionSystemUI : MonoBehaviour{

    [SerializeField] private Transform  _actionButtonPrefab;
    [SerializeField] private Transform  _actionButtonConteinerTransform;
    [SerializeField] private TextMeshProUGUI _actionPointsText;

    private void OnEnable() {
        UnitActionSystem.Instance.OnUnitSelectedChanged += UnitActionSystem_OnUnitSelectedChanged;
        UnitActionSystem.Instance.OnUnitActionChanged += UnitActionSystem_OnUnitActionChanged;
        UnitActionSystem.Instance.OnActionStart += UnitActionSystem_OnActionStart;
    }
    private void OnDisable() {
        UnitActionSystem.Instance.OnUnitSelectedChanged -= UnitActionSystem_OnUnitSelectedChanged;
        UnitActionSystem.Instance.OnUnitActionChanged -= UnitActionSystem_OnUnitActionChanged;
        UnitActionSystem.Instance.OnActionStart -= UnitActionSystem_OnActionStart;
    }

    private void Start() {
        CreateUnitActionButtons();
        UpdateSelectedVisuals();
        UpdateActionPoints();
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
        UpdateActionPoints();
    }

    private void UnitActionSystem_OnUnitActionChanged(object sender, EventArgs e){
        UpdateSelectedVisuals();
    }

    private void UnitActionSystem_OnActionStart(){
        UpdateActionPoints();
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

    private void UpdateActionPoints(){
        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();
        _actionPointsText.text = "Action Points: " +  selectedUnit.GetActionPoints();
    }
}
