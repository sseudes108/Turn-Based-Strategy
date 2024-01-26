using System;
using System.Collections.Generic;
using UnityEngine;

public class Testings : MonoBehaviour{
    [SerializeField] private Unit unit;

    private void Update() {
        // if(Input.GetKeyDown(KeyCode.T)){
        //     GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());
        //     GridPosition startGridPosition = new(0, 0);

        //     List<GridPosition> gridPositionList = PathFinding.Instance.FindPath(startGridPosition, mouseGridPosition, out int pathLenght);
            
        //     for(int i= 0; i < gridPositionList.Count - 1; i++){
        //         Debug.DrawLine(
        //             LevelGrid.Instance.GetWorldPosition(gridPositionList[i]),
        //             LevelGrid.Instance.GetWorldPosition(gridPositionList[i + 1]),
        //             Color.red,
        //             10f
        //         );
        //     }
        // }
    }
}
