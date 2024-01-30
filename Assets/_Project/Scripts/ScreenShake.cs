using Cinemachine;
using UnityEngine;
using UnityEngine.Video;

public class ScreenShake : MonoBehaviour{
    public static ScreenShake Instance;
    private CinemachineImpulseSource _impulseSource;

    private void Awake() {
        _impulseSource = GetComponent<CinemachineImpulseSource>();
        
        if(Instance != null){
            Debug.Log("There's more than one UnitActionSystem" + transform + "-" + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void Shake(){
        _impulseSource.GenerateImpulse();
    }
}
