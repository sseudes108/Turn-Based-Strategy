using System;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class PathFinding : MonoBehaviour{
    public static PathFinding Instance {get; private set;}

    const int MOVE_STRAIGHT_COST = 10;
    const int MOVE_DIAGONAL_COST = 14;

    [SerializeField] private Transform _gridObjectDebugPrefab;
    //[SerializeField] private LayerMask _obstaclesLayerMask;AQUI

    private int _width, _height, _cellsize;
    private GridSystem<PathNode> _gridSystem;

    private void Awake() {
        if(Instance != null){
            Debug.Log("There's more than one PathFinding" + transform + "-" + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void SetUp(int width, int height, int cellsize){
        this._width = width;
        this._height = height;
        this._cellsize = cellsize;

        _gridSystem = new GridSystem<PathNode>(_width, _height, _cellsize, (GridSystem<PathNode> g, GridPosition gridPosition) => new PathNode(gridPosition));
        //_gridSystem.CreateDebugObjets(_gridObjectDebugPrefab);

        for (int x = 0; x < width; x++){
            for(int z = 0; z < height; z++){
                GridPosition gridPosition = new(x, z);
                Vector3 worldPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);
                float rayCastOffsetDistance = 5.0f;
                bool hasObstacle = Physics.Raycast(worldPosition + Vector3.down * rayCastOffsetDistance, Vector3.up, rayCastOffsetDistance * 2, LayerMask.NameToLayer("Osbtacle"));

                if(hasObstacle){
                    GetNode(x,z).SetIsWalkable(false);
                }
            }
        }
    }

    public List<GridPosition> FindPath(GridPosition startGridPosition, GridPosition endGridPosition, out int pathLenght){
        List<PathNode> openList = new();
        List<PathNode> closedList = new();

        PathNode startNode = _gridSystem.GetGridObject(startGridPosition);

        PathNode endNode = _gridSystem.GetGridObject(endGridPosition);

        openList.Add(startNode);

        for(int x = 0; x < _gridSystem.GetWidth(); x++){

            for(int z = 0; z < _gridSystem.GetHeight(); z++){

                GridPosition gridPosition = new(x,z);
                PathNode pathNode = _gridSystem.GetGridObject(gridPosition);

                pathNode.SetGCost(int.MaxValue);
                pathNode.SetHCost(0);
                pathNode.CalculateFCost();
                pathNode.ResetCameFromPathNode();
            }
        }

        startNode.SetGCost(0);
        startNode.SetHCost(CalculateDistance(startGridPosition, endGridPosition));
        startNode.CalculateFCost();

        while(openList.Count > 0){
            PathNode currentNode = GetLowestFCastPathNode(openList);

            if(currentNode == endNode){
                pathLenght = endNode.GetFCost();
                return CalculatePath(endNode);
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach(PathNode neighbourNode in GetNeighbourList(currentNode)){
                if(closedList.Contains(neighbourNode)) continue;

                if(!neighbourNode.IsWalkable()){
                    closedList.Add(neighbourNode);
                    continue;
                }

                int tentativeGCost = currentNode.GetGCost() + CalculateDistance(currentNode.GetGridPosition(), neighbourNode.GetGridPosition());

                if(tentativeGCost < neighbourNode.GetGCost()){
                    neighbourNode.SetCameFromPathNode(currentNode);
                    neighbourNode.SetGCost((int)tentativeGCost);
                    neighbourNode.SetHCost(CalculateDistance(neighbourNode.GetGridPosition(), endGridPosition));
                    neighbourNode.CalculateFCost();

                    if(!openList.Contains(neighbourNode)){
                        openList.Add(neighbourNode);
                    }
                }
            }
        }

        //No Path found
        pathLenght = 0;
        return null;
    }

    private List<GridPosition> CalculatePath(PathNode endNode){
        List<PathNode> pathNodeList = new();
        pathNodeList.Add(endNode);
        PathNode currentNode = endNode;
        while(currentNode.GetCameFromPathNode() != null){
            pathNodeList.Add(currentNode.GetCameFromPathNode());
            currentNode = currentNode.GetCameFromPathNode();
        }

        pathNodeList.Reverse();

        List<GridPosition> gridPositionList = new();
        foreach(PathNode pathNode in pathNodeList){
            gridPositionList.Add(pathNode.GetGridPosition());
        }

        return gridPositionList;
    }

    public int CalculateDistance(GridPosition gridPositionA, GridPosition gridPositionB){
        GridPosition gridPositionDistance = gridPositionA - gridPositionB;

        int xDistance = Mathf.Abs(gridPositionDistance.x);
        int zDistance = Mathf.Abs(gridPositionDistance.z);
        int remaining = Mathf.Abs(xDistance - zDistance);

        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, zDistance) + MOVE_STRAIGHT_COST * remaining;
    }

    private PathNode GetLowestFCastPathNode(List<PathNode> pathNodeList){
        PathNode lowestFCostPathNode = pathNodeList[0];

        for(int i = 0; i < pathNodeList.Count; i++){
            if(pathNodeList[i].GetFCost() < lowestFCostPathNode.GetFCost()){
                lowestFCostPathNode = pathNodeList[i];
            }
        }
        return lowestFCostPathNode;
    }

    private PathNode GetNode(int x, int z){
        return _gridSystem.GetGridObject(new GridPosition(Mathf.Abs(x),Mathf.Abs(z)));
    }

    private List<PathNode> GetNeighbourList(PathNode currentNode){
        List<PathNode> neighbourList = new();

        GridPosition gridposition = currentNode.GetGridPosition();

        //LEFT
        if(gridposition.x - 1 >= 0){
            //Left Node
            neighbourList.Add(GetNode(gridposition.x - 1, gridposition.z));
            //Left Down
            if(gridposition.z - 1 >= 0){
                neighbourList.Add(GetNode(gridposition.x - 1, gridposition.z - 1));
            }
            //Left Up
            if(gridposition.z + 1 < _gridSystem.GetHeight()){
                neighbourList.Add(GetNode(gridposition.x - 1, gridposition.z + 1));
            }
        }

        //RIGHT
        if(gridposition.x + 1 < _gridSystem.GetWidth()){
            //Right Node
            neighbourList.Add(GetNode(gridposition.x + 1, gridposition.z));
            //Right Down
            if(gridposition.z - 1 >= 0){
                neighbourList.Add(GetNode(gridposition.x + 1, gridposition.z - 1));
            }
            //Right Up
            if(gridposition.z + 1 < _gridSystem.GetHeight()){
                neighbourList.Add(GetNode(gridposition.x + 1, gridposition.z + 1));
            }
        }

        //Up Node
        if(gridposition.z - 1 >= 0){
            neighbourList.Add(GetNode(gridposition.x, gridposition.z + 1));
        }
        //Down Node
        if(gridposition.z + 1 < _gridSystem.GetHeight()){
            neighbourList.Add(GetNode(gridposition.x, gridposition.z - 1));
        }     
        
        return neighbourList;
    }

    public bool IsWalkableGridPosition(GridPosition gridPosition){
        return _gridSystem.GetGridObject(gridPosition).IsWalkable();
    }

    public bool HasPath(GridPosition startGridPosition, GridPosition endGridPosition){
        return FindPath(startGridPosition, endGridPosition, out int pathLenght) != null;
    }

    public int GetPathLenght(GridPosition startGridPosition, GridPosition endGridPosition){
        FindPath(startGridPosition, endGridPosition, out int pathLenght);
        return pathLenght;
    }
}
