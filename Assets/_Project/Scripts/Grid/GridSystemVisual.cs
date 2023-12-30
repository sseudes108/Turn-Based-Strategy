using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class GridSystemVisual : MonoBehaviour{
    public static GridSystemVisual Instance {get; private set;}
    [SerializeField] Transform _gridVisualSinglePrefab;
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

        HideAllGridPosition();
    }

    public void HideAllGridPosition(){
        for(int x = 0; x < LevelGrid.Instance.GetWidth(); x++){
            for (int z = 0; z < LevelGrid.Instance.GetHeight(); z++){
                gridSystemVisualSingleArray[x, z].Hide();
            }
        }
    }

    public void ShowGridPositionList(List<GridPosition> gridPositionList){
        foreach(GridPosition grid in gridPositionList){
            gridSystemVisualSingleArray[grid._x, grid._z].Show();
        }
    }
}
