using System;
using UnityEngine;

public class UnitRagdollSpawner : MonoBehaviour{

    [SerializeField] private Transform  _ragdollPrefab;
    private HealthSystem _healthSystem;

    private void OnEnable() {
        _healthSystem.OnDead += HealhSystem_OnDead;
    }
    private void OnDisable() {
        _healthSystem.OnDead -= HealhSystem_OnDead;
    }

    private void Awake(){
        _healthSystem = GetComponent<HealthSystem>();
    }

    private void HealhSystem_OnDead(object sender, EventArgs e){
        Instantiate(_ragdollPrefab, transform.position, transform.rotation);
    }

}
