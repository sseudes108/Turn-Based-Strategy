using UnityEngine;

public class UnitRagdoll : MonoBehaviour{
    [SerializeField] private Transform _ragdoolRootBone;
    private void Start() {
        Test(_ragdoolRootBone);
    }
    
    private void Test(Transform ragdolRoot){
        foreach(Transform child in _ragdoolRootBone){
            if(child.TryGetComponent<Rigidbody>(out Rigidbody childRigidbody)){
                childRigidbody.angularVelocity = Vector3.zero;
                childRigidbody.linearVelocity = Vector3.zero;
            }
            Test(child);
        }
    }

    public void Setup(Transform originalRootBone){
        MatchAllChildTransforms(originalRootBone, _ragdoolRootBone);
        ApplyExplosionForceToRagdoll(_ragdoolRootBone, 300, transform.position, 10f);
    }

    private void MatchAllChildTransforms(Transform unitRoot, Transform ragdolRoot){
        foreach (Transform child in unitRoot){
            Transform cloneChild = ragdolRoot.Find(child.name);
            if(cloneChild != null){
                cloneChild.SetPositionAndRotation(child.position, child.rotation);

                MatchAllChildTransforms(child, cloneChild);
            }
        }
    }

    private void ApplyExplosionForceToRagdoll(Transform root, float explosionForce, Vector3 explosionPosition, float explosionRange){
        foreach(Transform child in root){
            if(child.TryGetComponent<Rigidbody>(out Rigidbody childRigidbody)){
                childRigidbody.AddExplosionForce(explosionForce, explosionPosition, explosionRange);
            }

            ApplyExplosionForceToRagdoll(child, explosionForce, explosionPosition, explosionRange);
        }
    }
}