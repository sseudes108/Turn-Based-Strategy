using UnityEngine;

public class Testings : MonoBehaviour{
    [SerializeField] private Unit unit;
    
    private void Update() {
        if(Input.GetKeyDown(KeyCode.T)){
            unit.GetMoveAction().GetValidActionGridPositionList();
        }
    }
}
