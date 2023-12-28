using UnityEngine;

public class MouseWorld : MonoBehaviour{
    public static MouseWorld Instance;
    [SerializeField] private LayerMask _mousePlaneLayerMask;

    private void Awake() {
        if(!Instance){Instance = this;}else{Destroy(gameObject);}
    }

    public static Vector3 GetPosition(){
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, Instance._mousePlaneLayerMask);
        return raycastHit.point;
    }
}