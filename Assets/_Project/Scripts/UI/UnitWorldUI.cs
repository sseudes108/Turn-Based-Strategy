using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitWorldUI : MonoBehaviour{
    [SerializeField] private TextMeshProUGUI _actionPointsText;
    [SerializeField] private Unit _unit;
    [SerializeField] private Image _healthBarImage;
    [SerializeField] private HealthSystem _healthSystem;

    private void OnEnable() {
        Unit.OnAnyActionPointsChanged += Unit_OnAnyActionPointsChanged;
        _healthSystem.OnDamage += HealthSystem_OnDamage;
    }

    private void OnDisable() {
        Unit.OnAnyActionPointsChanged -= Unit_OnAnyActionPointsChanged;
        _healthSystem.OnDamage += HealthSystem_OnDamage;
    }

    private void Start() {
        UpdateActionPointsText();
    }

    private void UpdateActionPointsText(){
        _actionPointsText.text = _unit.GetActionPoints().ToString();
    }

    private void Unit_OnAnyActionPointsChanged(object sender, EventArgs e){
        UpdateActionPointsText();
    }

    private void HealthSystem_OnDamage(object sender, EventArgs e){
        UpdateHealthBarImage();
    }

    private void UpdateHealthBarImage(){
        _healthBarImage.fillAmount = _healthSystem.GetHealthPointsNormalized();
        Debug.Log(_healthSystem.GetHealthPointsNormalized());
    }
}