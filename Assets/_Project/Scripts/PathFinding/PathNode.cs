using UnityEngine;

public class PathNode{
    private GridPosition _gridPosition;

    private int _gCost, _hCost, _fCost;
    private PathNode _cameFromPathNode;

    public PathNode(GridPosition gridPosition){
        _gridPosition = gridPosition;
    }

    public override string ToString(){
        return _gridPosition.ToString();
    }

    public void SetGCost(int gCost){
        this._gCost = gCost;
    }
    public void SetHCost(int hCost){
        this._hCost = hCost;
    }
    public void CalculateFCost(){
        _fCost = _gCost + _hCost;
    }
    
    public void ResetCameFromPathNode(){
        _cameFromPathNode = null;
    }
    public void SetCameFromPathNode(PathNode pathNode){
        _cameFromPathNode = pathNode;
    }
    public PathNode GetCameFromPathNode() => _cameFromPathNode;

    public int GetGCost() => _gCost;
    public int GetHCost() => _hCost;
    public int GetFCost() => _fCost;
    public GridPosition GetGridPosition() => _gridPosition;
}
