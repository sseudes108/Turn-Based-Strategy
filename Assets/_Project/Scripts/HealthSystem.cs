using System;
using TMPro;
using UnityEngine;

public class HealthSystem : MonoBehaviour{
    public event EventHandler OnDead;
    public event EventHandler OnDamage;

    [SerializeField] private int _currentHealth;
    [SerializeField] private int _MaxHealth;

    private void Awake() {
        _currentHealth = _MaxHealth;
    }

    public void Damage(int damageAmount){
        _currentHealth -= damageAmount;
        OnDamage?.Invoke(this, EventArgs.Empty);
        
        if(_currentHealth <= 0){
            _currentHealth = 0;
            Die();
        }
    }

    private void Die(){
        OnDead?.Invoke(this, EventArgs.Empty);
    }

    public float GetHealthPointsNormalized(){
        return _currentHealth / (float)_MaxHealth;
    }
}
