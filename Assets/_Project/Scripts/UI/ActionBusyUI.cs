using UnityEngine;

public class ActionBusyUI : MonoBehaviour{
    [SerializeField] private GameObject _busyUI;

    private void OnEnable() {
        UnitActionSystem.Instance.OnBusyChanged += ActiveDeactiveBusyImageUI;
    }

    private void OnDisable() {
        UnitActionSystem.Instance.OnBusyChanged -= ActiveDeactiveBusyImageUI;
    }

    private void ActiveDeactiveBusyImageUI(){
        bool currenStatus = _busyUI.activeSelf;

        currenStatus = !currenStatus;
        _busyUI.SetActive(currenStatus);
    }
}
