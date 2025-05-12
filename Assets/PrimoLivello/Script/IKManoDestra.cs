using UnityEngine;

public class IKManoDestra : MonoBehaviour
{
    private Animator animator;
    public Transform targetPomello;
    private bool usaIK = false;
    public float weight = 0.4f; // puoi regolarlo anche dinamicamente

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void AttivaIK(bool attiva)
    {
        usaIK = attiva;
    }

    void OnAnimatorIK(int layerIndex)
    {


        if (animator && usaIK && targetPomello != null)
        {
            
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, weight);
            animator.SetIKRotationWeight(AvatarIKGoal.RightHand, weight);

            animator.SetIKPosition(AvatarIKGoal.RightHand, targetPomello.position);
            animator.SetIKRotation(AvatarIKGoal.RightHand, targetPomello.rotation);
        }
        else
        {
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
            animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0);
        }
    }
}
