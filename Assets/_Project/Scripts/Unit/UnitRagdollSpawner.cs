using System;
using UnityEngine;

public class UnitRagdollSpawner : MonoBehaviour{

    [SerializeField] private Transform  _ragdollPrefab;
    [SerializeField] private Transform _originalRootBone;
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
        Transform ragDollTransform = Instantiate(_ragdollPrefab, transform.position, transform.rotation);
        UnitRagdoll unitRagdoll = ragDollTransform.GetComponent<UnitRagdoll>();
        unitRagdoll.Setup(_originalRootBone);
    }

}
