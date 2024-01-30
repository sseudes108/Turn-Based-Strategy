using System;
using System.Collections.Generic;
using UnityEngine;

public class Testings : MonoBehaviour{
    [SerializeField] private Unit unit;

    private void Update() {
    if(Input.GetKeyDown(KeyCode.T)){
            ScreenShake.Instance.Shake();
        }
    }
}
