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
        BaseAction.OnAnyActionStarted += BaseAction_OnAnyActionStarted;
        TurnSystem.OnTurnChanged += TurnSystem_OnTurnEnd;
        Unit.OnAnyActionPointsChanged += Unit_OnAnyActionPointsChanged;
    }
    private void OnDisable() {
        UnitActionSystem.Instance.OnUnitSelectedChanged -= UnitActionSystem_OnUnitSelectedChanged;
        UnitActionSystem.Instance.OnUnitActionChanged -= UnitActionSystem_OnUnitActionChanged;
        BaseAction.OnAnyActionStarted -= BaseAction_OnAnyActionStarted;
        TurnSystem.OnTurnChanged -= TurnSystem_OnTurnEnd;
        Unit.OnAnyActionPointsChanged -= Unit_OnAnyActionPointsChanged;
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

    private void BaseAction_OnAnyActionStarted(object sender, EventArgs e){
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

    private void TurnSystem_OnTurnEnd(){
        UpdateActionPoints();
    }

    private void Unit_OnAnyActionPointsChanged(object sender, EventArgs e){
        UpdateActionPoints();
    }
}
