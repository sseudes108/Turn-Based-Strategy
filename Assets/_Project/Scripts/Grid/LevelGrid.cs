using System;
using UnityEngine;

public class LevelGrid : MonoBehaviour{
    public static LevelGrid Instance {get; private set;}
    [SerializeField] private Transform _gridObjectDebug;

    private GridSystem _gridSystem;

    private void Awake() {
        if(Instance != null){
            Debug.Log("There's more than one LevelGrid" + transform + "-" + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start(){
        _gridSystem = new GridSystem(10, 10, 2f);
        _gridSystem.CreateDebugObjets(_gridObjectDebug);
    }

    public void AddUnitAtGridPosition(GridPosition gridPosition, Unit unit){
        GridObject gridObject = _gridSystem.GetGridObjet(gridPosition);
        gridObject.AddUnit(unit);
    }

    public void RemoveUnitAtGridPosition(GridPosition gridPosition, Unit unit){
        GridObject gridObject = _gridSystem.GetGridObjet(gridPosition);
        gridObject.RemoveUnit(unit);
    }

    public void UnitMovedGridPosition(Unit unit, GridPosition fromGridPosition, GridPosition toGridPosition){
        RemoveUnitAtGridPosition(fromGridPosition, unit);
        AddUnitAtGridPosition(toGridPosition, unit);
    }

    public int GetWidth() => _gridSystem.GetWidth();
    public int GetHeight() => _gridSystem.GetHeight();

    public GridPosition GetGridPosition(Vector3 worldPosition) => _gridSystem.GetGridPosition(worldPosition);

    public bool IsValidGridPosition(GridPosition gridPosition) => _gridSystem.IsValidGridPosition(gridPosition);

    public Vector3 GetWorldPosition(GridPosition gridPosition) => _gridSystem.GetWorldPosition(gridPosition);

    public bool HasAnyUnitOnGridPosition(GridPosition gridPosition){
        GridObject gridObject = _gridSystem.GetGridObjet(gridPosition);
        return gridObject.HasAnyUnit();
    }
    public Unit GetUnitAtGridPosition(GridPosition gridPosition){
        GridObject gridObject = _gridSystem.GetGridObjet(gridPosition);
        return gridObject.GetUnit();
    }
}
