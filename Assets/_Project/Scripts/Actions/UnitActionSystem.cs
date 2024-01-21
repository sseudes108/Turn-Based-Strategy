using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitActionSystem : MonoBehaviour{

    public static UnitActionSystem Instance {get; private set;}

    public event EventHandler OnUnitSelectedChanged;
    public event EventHandler OnUnitActionChanged;
    public event EventHandler<bool> OnBusyChanged;
   
    [SerializeField] private Unit _selectedUnit;
    [SerializeField] private LayerMask _unitLayerMask;

    private BaseAction _selectedAction;
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
        if(!TurnSystem.Instance.IsPlayerTurn()) return;
        if(TryHandleUnitSelection()) return;
        if(EventSystem.current.IsPointerOverGameObject()) return;

        HandleSelectedAction();
    }

    private void HandleSelectedAction(){   
        if(Input.GetMouseButtonDown(0)){
            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());

            if(_selectedAction.IsValidActionGridPosition(mouseGridPosition)){

                if(!_selectedUnit.TrySpendActionPointsToTakeAction(_selectedAction)){return;}
                
                SetBusy();
                _selectedAction.TakeAction(mouseGridPosition, ClearBusy);
            }
        }
    }

    private void SetBusy(){
        _isBusy = true;
        OnBusyChanged?.Invoke(this, _isBusy);
    }

    private void ClearBusy(){
        _isBusy = false;
        OnBusyChanged?.Invoke(this, _isBusy);
    }

    private bool TryHandleUnitSelection(){
        if(Input.GetMouseButtonDown(0)){
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, _unitLayerMask)){
                if(raycastHit.collider.TryGetComponent<Unit>(out Unit unit)){
                    if(unit == _selectedUnit) return false;
                    if(unit.IsEnemy()) return false;

                    SetSelectedUnit(unit);
                    return true;
                }
            }
        }
        return false;
    }

    private void SetSelectedUnit(Unit unit){
        _selectedUnit = unit;
        SetSelectedAction(unit.GetMoveAction());
        OnUnitSelectedChanged?.Invoke(this, EventArgs.Empty);
        OnUnitActionChanged?.Invoke(this, EventArgs.Empty);
    }

    public void SetSelectedAction(BaseAction baseAction){
        if(!_isBusy){
            _selectedAction = baseAction;
            OnUnitActionChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public Unit GetSelectedUnit(){
        return _selectedUnit;
    }

    public BaseAction GetSelectedAction(){
        return _selectedAction;
    }
}
