using UnityEngine;

public class Unit : MonoBehaviour{
    private Vector3 _targetPosition;
    private AnimationPlayer _animator;

    private void Awake() {
        _animator = GetComponent<AnimationPlayer>();
        _targetPosition = transform.position;
    }

    private void Update(){
        HandleMovement();
    }

    private void HandleMovement(){
        float stoppingDistance = 0.1f;
        if (Vector3.Distance(transform.position, _targetPosition) > stoppingDistance)
        {
            Vector3 direction = (_targetPosition - transform.position).normalized;
            float moveSpeed = 4f;
            transform.position += moveSpeed * Time.deltaTime * direction;

            float rotateSpeed = 7f;
            transform.forward = Vector3.Lerp(transform.forward, direction, rotateSpeed * Time.deltaTime);

            PlayRunAnimation();
        }else{
            PlayIdleAnimation();
        }
    }

    public void Move(Vector3 targetPosition){
        _targetPosition = targetPosition;
    }

    private void PlayRunAnimation(){
        _animator.ChangeAnimationState(_animator.RIFLE_RUN);
    }
    private void PlayIdleAnimation(){
        _animator.ChangeAnimationState(_animator.RIFLE_AIMING_IDLE);
    }
}
