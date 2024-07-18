using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Controls the lower body animation IK system of the Animator component in Unity.
// This script is designed to be attached to a GameObject that has an Animator component.

public class LowerBodyAnimation : MonoBehaviour
{
    [SerializeField] private Animator animator;

    // The [Range(0, 1)] attribute limits these variables to a range between 0 and 1, where 0 means no influence and 1 means full influence.
    [SerializeField] [Range(0, 1)] private float leftFootPosWeight;
    [SerializeField] [Range(0, 1)] private float rightFootPosWeight;

    [SerializeField] [Range(0, 1)] private float leftFootRotWeight;
    [SerializeField] [Range(0, 1)] private float rightFootRotWeight;

    // footOffset, raycastOffsetLeft, and raycastOffsRight are vectors used for fine-tuning the position where the raycasts (invisible lines used to detect surfaces) are cast and the position where the feet should land.
    [SerializeField] private Vector3 footOffset;
    [SerializeField] private Vector3 raycastOffsetLeft;
    [SerializeField] private Vector3 raycastOffsRight;



    // The OnAnimatorIK(int layerIndex) method is a special Unity function that runs during the IK calculations of the Animator.It is here that the position and rotation of the character's feet are adjusted.

    private void OnAnimatorIK(int layerIndex)
    {
        Vector3 leftFootPos = this.animator.GetIKPosition(AvatarIKGoal.LeftFoot);
        Vector3 rightFootPos = this.animator.GetIKPosition(AvatarIKGoal.RightFoot);

        RaycastHit hitLeftFoot;
        RaycastHit hitRightFoot;

        bool isLeftFootDown = Physics.Raycast(leftFootPos + this.raycastOffsetLeft, Vector3.down, out hitLeftFoot);
        bool isRightFootDown = Physics.Raycast(rightFootPos + this.raycastOffsRight, Vector3.down, out hitRightFoot);

        if (isLeftFootDown)
        {
            this.animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, this.leftFootPosWeight);
            this.animator.SetIKPosition(AvatarIKGoal.LeftFoot, hitLeftFoot.point + this.footOffset);

            Quaternion leftFootRotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(transform.forward, hitLeftFoot.normal), hitLeftFoot.normal);
            this.animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, this.leftFootRotWeight);
            this.animator.SetIKRotation(AvatarIKGoal.LeftFoot, leftFootRotation);
        }
        else
        {
            this.animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 0);
        }

        if (isRightFootDown)
        {
            this.animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, this.rightFootPosWeight);
            this.animator.SetIKPosition(AvatarIKGoal.RightFoot, hitRightFoot.point + this.footOffset);

            Quaternion rightFootRotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(transform.forward, hitRightFoot.normal), hitRightFoot.normal);
            this.animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, this.rightFootRotWeight);
            this.animator.SetIKRotation(AvatarIKGoal.RightFoot, rightFootRotation);
        }
        else
        {
            this.animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 0);
        }
    }
}
