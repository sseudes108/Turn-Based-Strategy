using System;
using UnityEngine;

public class BulletProjectile : MonoBehaviour{
    [SerializeField] private TrailRenderer _trailRenderer;
    [SerializeField] private Transform _impactVFX;

    private Vector3 _shotTarget;

    public void Init(Vector3 shotTarget){
        _shotTarget = shotTarget;
    }

    public void Update(){    
        float moveSpeed = 50f;
        transform.position = Vector3.MoveTowards(transform.position, _shotTarget, moveSpeed);

        if(Vector3.Distance(transform.position, _shotTarget) < 0.1f){
            transform.position = _shotTarget;
            _trailRenderer.transform.parent = null;
            Instantiate(_impactVFX, _shotTarget, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}