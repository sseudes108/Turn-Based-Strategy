using System;
using UnityEngine;

public class UnitActionSystem : MonoBehaviour{

    public static UnitActionSystem Instance {get; private set;}
    public event EventHandler OnUnitSelectedChanged;
    
    [SerializeField] private Unit _selectedUnit;
    [SerializeField] private LayerMask _unitLayerMask;


    private void Awake() {
        if(Instance != null){
            Debug.Log("There's more than one UnitActionSystem" + transform + "-" + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    private void Update() {
        if (Input.GetMouseButtonDown(0)){
            if(TryHandleUnitSelection()) return;
            _selectedUnit?.Move(MouseWorld.GetPosition());
        }        
    }

    private bool TryHandleUnitSelection(){
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, _unitLayerMask)){
            if(raycastHit.collider.TryGetComponent<Unit>(out Unit unit)){
                SetSelectedUnit(unit);
                return true;
            }
        }
        return false;
    }

    private void SetSelectedUnit(Unit unit){
        _selectedUnit = unit;
        OnUnitSelectedChanged?.Invoke(this, EventArgs.Empty);
    }

    public Unit GetSelectedUnit(){
        return _selectedUnit;
    }
}
