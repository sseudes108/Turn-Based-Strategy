using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurnSystemUI : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI _turnNumberText;
    [SerializeField] private Button _endTurnButton;

    private void OnEnable() {
        TurnSystem.OnTurnEnd += UpdateTurnNumberTextUI;
    }
    private void OnDisable() {
        TurnSystem.OnTurnEnd -= UpdateTurnNumberTextUI;
    }

    private void Start() {
        _endTurnButton.onClick.AddListener(TurnSystem.Instance.NextTurn);
        
        UpdateTurnNumberTextUI();
    }

    private void UpdateTurnNumberTextUI(){
        _turnNumberText.text = "Turn : " + TurnSystem.Instance.GetTurnNumber();
    }

}