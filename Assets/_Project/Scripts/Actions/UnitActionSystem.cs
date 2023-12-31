using System;
using UnityEngine;
using UnityEngine.Rendering;

public class UnitActionSystem : MonoBehaviour{

    public static UnitActionSystem Instance {get; private set;}
    public event EventHandler OnUnitSelectedChanged;
    
    [SerializeField] private Unit _selectedUnit;
    private BaseAction _selectedAction;
    [SerializeField] private LayerMask _unitLayerMask;

    private bool _isBusy;

    private void Awake() {
        if(Instance != null){
            Debug.Log("There's more than one UnitActionSystem" + transform + "-" + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start() {
        SetSelectedUnit(_selectedUnit);
    }
    
    private void Update() {
        if(_isBusy) return;

        if (Input.GetMouseButtonDown(0)){
            if(TryHandleUnitSelection()) return;

            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());

            if(_selectedUnit.GetMoveAction().IsValidActionGridPosition(mouseGridPosition)){
                SetBusy(true);
                _selectedUnit.GetMoveAction().Move(mouseGridPosition, SetBusy);
            }
        }

        if(Input.GetMouseButtonDown(1)){
            SetBusy(true);
            _selectedUnit.GetSpintAction().Spin(SetBusy);
        }     
    }

    private void HandleSelectedAction(){
        // if(Input.GetMouseButtonDown(0)){
        //     _selectedAction
        // }
    }

    private void SetBusy(bool busy){
        _isBusy = busy;
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
        SetSelectedAction(unit.GetMoveAction());
        OnUnitSelectedChanged?.Invoke(this, EventArgs.Empty);
    }

    public void SetSelectedAction(BaseAction baseAction){
        _selectedAction = baseAction;
    }

    public Unit GetSelectedUnit(){
        return _selectedUnit;
    }
}
