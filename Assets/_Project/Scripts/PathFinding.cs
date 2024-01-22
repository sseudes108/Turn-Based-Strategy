using UnityEngine;

public class PathFinding : MonoBehaviour{
    [SerializeField] private Transform _gridObjectDebugPrefab;

    private int _width, height, cellsize;
    private GridSystem<PathNode> _gridSystem;

    private void Awake() {
        _gridSystem = new GridSystem<PathNode>(10, 10, 2f, (GridSystem<PathNode> g, GridPosition gridPosition) => new PathNode(gridPosition));
        _gridSystem.CreateDebugObjets(_gridObjectDebugPrefab);
    }
}
