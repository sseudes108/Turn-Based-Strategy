using System;
using UnityEngine;

public class HealthSystem : MonoBehaviour{
    public event EventHandler OnDead;

    [SerializeField] private int _currentHealth = 100;

    public void Damage(int damageAmount){
        _currentHealth -= damageAmount;
        if(_currentHealth <= 0){
            _currentHealth = 0;
            Die();
        }
        Debug.Log(_currentHealth);
    }

    private void Die(){
        OnDead?.Invoke(this, EventArgs.Empty);
    }
}
