using System;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemVisual : MonoBehaviour{
    public static GridSystemVisual Instance {get; private set;}
    [SerializeField] Transform _gridVisualSinglePrefab;

    [Serializable]
    public struct GridVisualTypeMaterial{
        public GridVisualType gridVisualType;
        public Material material;
    }

    public enum GridVisualType{
        White, Blue, Red, RedSoft, Yellow
    }
    [SerializeField] private List<GridVisualTypeMaterial> _gridVisualTypeMaterialsList;

    
    private GridSystemVisualSingle[,] gridSystemVisualSingleArray;
     
    private void Awake() {
        if(Instance != null){
            Debug.Log("There's more than one GridSystemVisual" + transform + "-" + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start() {
        gridSystemVisualSingleArray = new GridSystemVisualSingle[LevelGrid.Instance.GetWidth(), LevelGrid.Instance.GetHeight()];

        for(int x = 0; x < LevelGrid.Instance.GetWidth(); x++){
            for (int z = 0; z < LevelGrid.Instance.GetHeight(); z++){
                GridPosition gridPosition = new(x, z);
                Transform gridSystemVisualSingleTransform = Instantiate(_gridVisualSinglePrefab, LevelGrid.Instance.GetWorldPosition(gridPosition), Quaternion.identity);
                gridSystemVisualSingleArray[x, z] = gridSystemVisualSingleTransform.GetComponent<GridSystemVisualSingle>();
            }
        }
    }

    private void Update() {
        UpdateGridVisual();
    }
    
    public void HideAllGridPosition(){
        for(int x = 0; x < LevelGrid.Instance.GetWidth(); x++){
            for (int z = 0; z < LevelGrid.Instance.GetHeight(); z++){
                gridSystemVisualSingleArray[x, z].Hide();
            }
        }
    }

    public void ShowGridPositionList(List<GridPosition> gridPositionList, GridVisualType gridVisualType){
        foreach(GridPosition gridposition in gridPositionList){
            gridSystemVisualSingleArray[gridposition._x, gridposition._z].Show(GetGridVisualTypeMaterial(gridVisualType));
        }
    }

    public void ShowGridPositionRange(GridPosition gridPosition, int range, GridVisualType gridVisualType){
        List<GridPosition> gridPositionList = new();
        
        for(int x = -range; x<= range; x++){
            for(int z = -range; z<= range; z++){
                GridPosition testGridPosition = gridPosition + new GridPosition(x, z);
                if(!LevelGrid.Instance.IsValidGridPosition(testGridPosition)){continue;}

                int aimDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if(aimDistance > range){continue;}

                gridPositionList.Add(testGridPosition);
            }
        }

        ShowGridPositionList(gridPositionList, gridVisualType);
    }

    private void UpdateGridVisual(){
        HideAllGridPosition();
        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();
        BaseAction selectedAction = UnitActionSystem.Instance.GetSelectedAction();

        GridVisualType gridVisualType;

        switch (selectedAction)
        {
            default:
            case MoveAction:
                gridVisualType = GridVisualType.White;
            break;
            case SpinAction:
                gridVisualType = GridVisualType.Blue;
            break;
            case ShootAction shootAction:
                gridVisualType = GridVisualType.Red;
                ShowGridPositionRange(selectedUnit.GetGridPosition(), shootAction.GetMaxShootDistance(), GridVisualType.RedSoft);
            break;
        }

        ShowGridPositionList(selectedAction.GetValidActionGridPositionList(), gridVisualType);
    }

    private Material GetGridVisualTypeMaterial(GridVisualType gridVisualType){
        foreach (GridVisualTypeMaterial gridVisualTypeMaterial in _gridVisualTypeMaterialsList){
            if(gridVisualTypeMaterial.gridVisualType == gridVisualType){
                return gridVisualTypeMaterial.material;
            }
        }
        Debug.Log("Error! Could not find GridVisualTypeMaterial for GridVisualType" + gridVisualType);
        return null;
    }
}
