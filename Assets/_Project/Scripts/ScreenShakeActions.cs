using UnityEngine;

public class ScreenShakeActions : MonoBehaviour{
    private void OnEnable() {
        ShootAction.OnShoot += ShootAction_OnShoot;
    }

    private void OnDisable() {
        ShootAction.OnShoot -= ShootAction_OnShoot;
    }

    private void ShootAction_OnShoot(object sender){
        ScreenShake.Instance.Shake();
    }
}
