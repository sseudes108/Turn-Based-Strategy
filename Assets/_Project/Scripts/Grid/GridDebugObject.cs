using UnityEngine;
using TMPro;

public class GridDebugObject : MonoBehaviour{
    [SerializeField] private TextMeshPro _gridDebugText;

    private object _gridObject;

    public virtual void SetGridObject(object gridObject){
        this._gridObject = gridObject;
    }

    protected virtual void Update() {
        UpdateUI();
    }

    private void UpdateUI(){
        _gridDebugText.text = _gridObject.ToString();
    }
}
