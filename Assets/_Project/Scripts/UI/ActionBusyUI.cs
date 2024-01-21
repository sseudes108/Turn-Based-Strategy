using UnityEngine;

public class ActionBusyUI : MonoBehaviour{
    [SerializeField] private GameObject _busyUI;

    private void OnEnable() {
        UnitActionSystem.Instance.OnBusyChanged += UnitActionSystem_OnBusyChanged;
    }

    private void OnDisable() {
        UnitActionSystem.Instance.OnBusyChanged -= UnitActionSystem_OnBusyChanged;
    }

    private void Start() {
        Hide();
    }

    private void Show(){
        _busyUI.gameObject.SetActive(true);
    }

    private void Hide(){
        _busyUI.gameObject.SetActive(false);
    }

    private void UnitActionSystem_OnBusyChanged(object sender, bool isBusy){
        if(isBusy){
            Show();
        }else{
            Hide();
        }
    }
}
