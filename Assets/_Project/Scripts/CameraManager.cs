using System;
using System.Collections;
using UnityEngine;

public class CameraManager : MonoBehaviour{
    [SerializeField] private GameObject _actionCameraGameObject;

    private void OnEnable() {
        BaseAction.OnAnyActionStarted += BaseAction_OnAnyActionStarted;
        BaseAction.OnAnyActionCompleted += BaseAction_OnAnyActionCompleted;
    }

    private void OnDisable() {
        BaseAction.OnAnyActionStarted -= BaseAction_OnAnyActionStarted;
        BaseAction.OnAnyActionCompleted -= BaseAction_OnAnyActionCompleted;
    }

    private void ShowActionCamera(){
        _actionCameraGameObject.SetActive(true);
    }
    private void HideActionCamera(){
        _actionCameraGameObject.SetActive(false);
    }

    private void BaseAction_OnAnyActionStarted(object sender, EventArgs e){
        switch (sender){
            case ShootAction shootAction:
                Unit shooterUnit = shootAction.GetUnit();
                Unit targetUnit = shootAction.GetTargetUnit();
                Vector3 cameraCharacterHeight = Vector3.up * 1.7f;

                Vector3 shootDir = (targetUnit.GetWordPosition() - shooterUnit.GetWordPosition()).normalized;

                float shoulderOffsetAmout = 0.5f;
                Vector3 shoulderOffset = Quaternion.Euler(0,90,0)* shootDir * shoulderOffsetAmout;

                Vector3 actionCameraPos = shooterUnit.GetWordPosition() + cameraCharacterHeight + shoulderOffset + shootDir * -1;

                _actionCameraGameObject.transform.position = actionCameraPos;
                _actionCameraGameObject.transform.LookAt(targetUnit.GetWordPosition() + cameraCharacterHeight);

                ShowActionCamera();
            break;
        }
    }

    private void BaseAction_OnAnyActionCompleted(object sender, EventArgs e){
        switch (sender){
            case ShootAction:
                HideActionCamera();
            break;
        }
    }
}
