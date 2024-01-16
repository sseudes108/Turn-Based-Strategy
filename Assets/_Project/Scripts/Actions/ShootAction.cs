using System;
using System.Collections.Generic;
using UnityEngine;

public class ShootAction : BaseAction{

    [SerializeField] private Transform _bulletProjectilePrefab;
    [SerializeField] private Transform _firePoint;

    private enum State{
        Aiming, Shooting, CoolOff,
    }

    private State _state;
    private float _stateTimer;
    private int _maxShootDistance = 7;
    private Unit _targetUnit;
    private bool _canShootBullet;

    private void Update() {
        if(!_isActive)return;

        _stateTimer -= Time.deltaTime;

        switch (_state){
            case State.Aiming:
                Rotate();
                if(_stateTimer <= 0){
                    _state = State.Shooting;
                    float shootingTime = 0.1f;
                    _stateTimer = shootingTime;
                }
                break;
            case State.Shooting:

                if(_canShootBullet){
                    PlayShootAnimation();
                    Shoot();
                    _canShootBullet = false;
                }

                if(_stateTimer <= 0){
                    _state = State.CoolOff;
                    float coolOfStateTime = 0.1f;
                    _stateTimer = coolOfStateTime;
                }

                break;
            case State.CoolOff:
                if(_stateTimer <= 0){
                    PlayIdleAnimation();
                    ActionComplete();
                }
                break;
            default:
            break;
        }
    }

    private void Shoot(){
        Transform newBullet = Instantiate(_bulletProjectilePrefab, _firePoint.position, Quaternion.identity);
        Vector3 shotDirection = _targetUnit.transform.position;
        shotDirection.y = _firePoint.transform.position.y;

        newBullet.GetComponent<BulletProjectile>().Init(shotDirection);

        _targetUnit.Damage(40);
    }

    private void PlayShootAnimation(){
        _animator.ChangeAnimationState(_animator.FIRING_RIFLE);
    }

    private void Rotate(){
        float rotationSpeed = 15f;
        Vector3 aimDirection = (_targetUnit.transform.position - transform.position).normalized;

        transform.forward = Vector3.Lerp(transform.forward, aimDirection, rotationSpeed * Time.deltaTime);
    }

    public override List<GridPosition> GetValidActionGridPositionList(){
        List<GridPosition> validActionGridPositionList = new();

        GridPosition unitGridPosition = _unit.GetGridPosition();

        for (int x = -_maxShootDistance; x <= _maxShootDistance; x++){
            for (int z = -_maxShootDistance; z <= _maxShootDistance; z++){
                GridPosition offSetGridPosition = new(x,z);
                GridPosition testGridPosition = unitGridPosition + offSetGridPosition;

                if(!LevelGrid.Instance.IsValidGridPosition(testGridPosition)){continue;}

                int aimDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if(aimDistance > _maxShootDistance){continue;}

                //Se não estiver ocupada, continue
                if(!LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition)){continue;}

                Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);
                //Both units are in the same "Team"
                if(targetUnit.IsEnemy() == _unit.IsEnemy()){continue;}

                validActionGridPositionList.Add(testGridPosition);
            }
        }
        return validActionGridPositionList;
    }

    public override void TakeAction(GridPosition gridposition, Action<bool> onActionComplete_SetBusy){
        ActionStart(onActionComplete_SetBusy);

        _targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridposition);
        
        _state = State.Aiming;
        float aimingTime = 1f;
        _stateTimer = aimingTime;

        _canShootBullet = true;
    }

    public override string GetActionName(){
        return "Shoot";
    }
}