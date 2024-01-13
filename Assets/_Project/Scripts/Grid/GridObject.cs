using System.Collections.Generic;

public class GridObject{
    public readonly GridSystem _gridSystem;
    public readonly GridPosition _gridPosition;
    private List<Unit> _unitList;

    public GridObject(GridSystem gridSystem, GridPosition gridPosition){
        this._gridSystem = gridSystem;
        this._gridPosition = gridPosition;
        this._unitList = new();
    }

    public void AddUnit(Unit unit){
        _unitList.Add(unit);
    }

    public void RemoveUnit(Unit unit){
        _unitList.Remove(unit);
    }

    public List<Unit> GetUnitList(){
        return _unitList;
    }

    public override string ToString(){
        string unitString = " ";
        foreach(Unit unit in _unitList){
            unitString += unit + "\n";
        }
        return _gridPosition.ToString() + "\n" + unitString;
    }

    public bool HasAnyUnit(){
        return _unitList.Count > 0;
    }

    public Unit GetUnit(){
        if(HasAnyUnit()){
            return _unitList[0];
        }else{
            return null;
        }
    }
}