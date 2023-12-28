using UnityEngine;

public class AnimationPlayer : MonoBehaviour{ 
    public readonly int RIFLE_RUN = Animator.StringToHash("Rifle Run");
    public readonly int RIFLE_AIMING_IDLE = Animator.StringToHash("Rifle Aiming Idle");
    private Animator _unitAnimator;
    
    private void Awake() {
        _unitAnimator = GetComponentInChildren<Animator>();
    }

    public void ChangeAnimationState(int AnimationHASH){
        _unitAnimator.Play(AnimationHASH);
    }
}
