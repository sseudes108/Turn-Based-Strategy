using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurnSystemUI : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI _turnNumberText;
    [SerializeField] private Button _endTurnButton;
    [SerializeField] private GameObject _enemyTurnVisual;

    private void OnEnable() {
        TurnSystem.OnTurnEnd += TurnSystem_OnTurnEnd;
    }
    private void OnDisable() {
        TurnSystem.OnTurnEnd -= TurnSystem_OnTurnEnd;
    }

    private void Start() {
        _endTurnButton.onClick.AddListener(TurnSystem.Instance.NextTurn);
        
        UpdateTurnNumberTextUI();
        UpdateEnemyTurnVisual();
        UpdateEndTurnButtonVisibility();
    }

    private void TurnSystem_OnTurnEnd(){
        UpdateTurnNumberTextUI();
        UpdateEnemyTurnVisual();
        UpdateEndTurnButtonVisibility();
    }

    private void UpdateTurnNumberTextUI(){
        _turnNumberText.text = "Turn : " + TurnSystem.Instance.GetTurnNumber();
    }

    private void UpdateEnemyTurnVisual(){
        _enemyTurnVisual.SetActive(!TurnSystem.Instance.IsPlayerTurn());
    }

    private void UpdateEndTurnButtonVisibility(){
        _endTurnButton.gameObject.SetActive(TurnSystem.Instance.IsPlayerTurn());
    }
}