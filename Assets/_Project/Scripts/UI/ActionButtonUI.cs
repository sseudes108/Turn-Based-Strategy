using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ActionButtonUI : MonoBehaviour{
    [SerializeField] private TextMeshProUGUI _textMeshPro;
    [SerializeField] private Button _button;
    [SerializeField] private Transform _selected;

    public void SetBaseAction(BaseAction baseAction){
        _textMeshPro.text = baseAction.GetActionName().ToUpper();

        _button.onClick.AddListener(()=>{
            UnitActionSystem.Instance.SetSelectedAction(baseAction);
        });
    }

    public string GetActionName(){
        return _textMeshPro.text;
    }

    public void EnableSelectedVisual(bool value){
        _selected.gameObject.SetActive(value);
    }
}
