using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using UnityEngine;

public class PathFindingGridDebugObject : GridDebugObject{
    [SerializeField] private TextMeshPro gCostText, hCostText, fCostText;
    [SerializeField] private SpriteRenderer isWalkableSpriteRenderer;

    private PathNode pathNode;
    public override void SetGridObject(object gridObject)
    {
        base.SetGridObject(gridObject);
        pathNode = (PathNode)gridObject;
    }

    protected override void Update()
    {
        base.Update();
        gCostText.text = pathNode.GetGCost().ToString();
        hCostText.text = pathNode.GetFCost().ToString();
        fCostText.text = pathNode.GetGCost().ToString();
        isWalkableSpriteRenderer.color = pathNode.IsWalkable() ? Color.green : Color.red;
    }

}
