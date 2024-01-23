using System;
using UnityEngine;

public class GridSystem<TGridObject>{
    private readonly int _width, _height;
    private readonly float _cellSize;
    private readonly TGridObject[,] _gridObjectsArray;

    public GridSystem(int width, int height, float cellSize, Func<GridSystem<TGridObject>, GridPosition, TGridObject> createGridObject){
        this._width = width;
        this._height = height;
        this._cellSize = cellSize;

        _gridObjectsArray = new TGridObject[_width, _height];

        for(int x = 0; x < _width; x ++){
            for (int z = 0; z < _height; z ++){
                GridPosition gridPosition = new GridPosition(x,z);
                _gridObjectsArray[x,z] = createGridObject(this, gridPosition);
            }
        }
    }

    public Vector3 GetWorldPosition(GridPosition gridPosition){
        return new Vector3(gridPosition.x, 0, gridPosition.z) * _cellSize;
    }

    public GridPosition GetGridPosition(Vector3 wordlPosition){
        return new GridPosition(){
            x = Mathf.RoundToInt(wordlPosition.x / _cellSize),
            z = Mathf.RoundToInt(wordlPosition.z / _cellSize),
        };
    }

    public void CreateDebugObjets(Transform debugPrefab){
        for(int x = 0; x < _width; x ++){
            for (int z = 0; z < _height; z++){
                GridPosition gridPosition = new GridPosition(x, z);
                Transform debugObjectTransform = GameObject.Instantiate(debugPrefab, GetWorldPosition(gridPosition), Quaternion.identity);
                GridDebugObject gridDebugObject = debugObjectTransform.GetComponent<GridDebugObject>();
                gridDebugObject.SetGridObject(GetGridObject(gridPosition));
            }
        }
    }

    public TGridObject GetGridObject(GridPosition gridPosition){
        return _gridObjectsArray[gridPosition.x, gridPosition.z];
    }

    public bool IsValidGridPosition(GridPosition gridPosition){
        return  gridPosition.x >= 0 && 
                gridPosition.z >= 0 && 
                gridPosition.x < _width && 
                gridPosition.z < _height;
    }

    public int GetWidth(){
        return _width;
    }
    public int GetHeight(){
        return _height;
    }
}
