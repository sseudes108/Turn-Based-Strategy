using UnityEngine;
using TMPro;

public class GridDebugObject : MonoBehaviour{

    private GridObject _gridObject;
    [SerializeField] private TMP_Text _gridDebugText;

    public void SetGridObject(GridObject gridObject){
        this._gridObject = gridObject;
    }

    private void Update() {
        UpdateUI();
    }

    private void UpdateUI(){
        _gridDebugText.text = _gridObject.ToString();
    }
}
