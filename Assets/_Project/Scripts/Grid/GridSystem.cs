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
        return new Vector3(gridPosition._x, 0, gridPosition._z) * _cellSize;
    }

    public GridPosition GetGridPosition(Vector3 wordlPosition){
        return new GridPosition(){
            _x = Mathf.RoundToInt(wordlPosition.x / _cellSize),
            _z = Mathf.RoundToInt(wordlPosition.z / _cellSize),
        };
    }

    public void CreateDebugObjets(Transform debugPrefab){
        for(int x = 0; x < _width; x ++){
            for (int z = 0; z < _height; z++){
                GridPosition gridPosition = new GridPosition(x, z);
                Transform debugObjectTransform = GameObject.Instantiate(debugPrefab, GetWorldPosition(gridPosition), Quaternion.identity);
                GridDebugObject gridDebugObject = debugObjectTransform.GetComponent<GridDebugObject>();
                gridDebugObject.SetGridObject(GetGridObjet(gridPosition));
            }
        }
    }

    public TGridObject GetGridObjet(GridPosition gridPosition){
        return _gridObjectsArray[gridPosition._x, gridPosition._z];
    }

    public bool IsValidGridPosition(GridPosition gridPosition){
        return  gridPosition._x >= 0 && 
                gridPosition._z >= 0 && 
                gridPosition._x < _width && 
                gridPosition._z < _height;
    }

    public int GetWidth(){
        return _width;
    }
    public int GetHeight(){
        return _height;
    }
}
