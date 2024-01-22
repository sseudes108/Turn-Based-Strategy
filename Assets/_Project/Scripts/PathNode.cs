using UnityEngine;

public class PathNode{
    private GridPosition _gridPosition;

    private int _gCost, _hCost, _fCost;
    private PathNode cameFromPathNode;

    public PathNode(GridPosition gridPosition){
        _gridPosition = gridPosition;
    }

    public override string ToString(){
        return _gridPosition.ToString();
    }

    public int GetGCost() => _gCost;
    public int GetHCost() => _hCost;
    public int GetFCost() => _fCost;

}

