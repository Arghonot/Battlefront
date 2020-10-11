using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKTest : MonoBehaviour
{

    protected Animator animator;

    public bool ikActive = false;
    //public Transform rightHandObj = null;
    //public Transform lookObj = null;

    //public Transform spine;
    public Transform headTransform;
    public Transform target;
    public Vector3 offset;
    Quaternion initialRotation;

    void Start()
    {
        animator = GetComponent<Animator>();
        initialRotation = headTransform.rotation;
    }

    //void LateUpdate()
    //{
    //    var lookRotation = Quaternion.LookRotation(headTransform.position - target.position);
    //    headTransform.rotation = lookRotation * Quaternion.Euler(
    //        (initialRotation.eulerAngles - offset).x,
    //        (initialRotation.eulerAngles - offset).y,
    //        (initialRotation.eulerAngles - offset).z);
    //}

    void OnAnimatorIK()
    {

        if (ikActive)
        {
            animator.SetLookAtWeight(0, 0, 0, 0, 0);
        }
        else
        {
            animator.SetLookAtWeight(1, 1, 1, 1, 1);
            animator.SetLookAtPosition(target.position);
        }
    }


    ////a callback for calculating IK
    //void OnAnimatorIK()
    //{
    //    return;
    //    if (animator)
    //    {

    //        //if the IK is active, set the position and rotation directly to the goal. 
    //        if (ikActive)
    //        {
    //            // Set the look __target position__, if one has been assigned
    //            if (lookObj != null)
    //            {
    //                Vector3 v3 = Quaternion.LookRotation(lookObj.position - spine.position).eulerAngles;

    //                animator.SetBoneLocalRotation(
    //                    HumanBodyBones.Spine,
    //                    Quaternion.Euler(v3.x, -v3.z, v3.y));//, transform.up));

    //                //animator.SetLookAtWeight(1);
    //                //animator.SetLookAtPosition(lookObj.position);
    //            }

    //            //// Set the right hand target position and rotation, if one has been assigned
    //            //if (rightHandObj != null)
    //            //{
    //            //    animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
    //            //    animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
    //            //    animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandObj.position);
    //            //    animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandObj.rotation);
    //            //}

    //        }

    //        //if the IK is not active, set the position and rotation of the hand and head back to the original position
    //        else
    //        {
    //            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
    //            animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0);
    //            animator.SetLookAtWeight(0);
    //        }
    //    }
    //}

    public Quaternion LookAt(Vector3 sourcePoint, Vector3 destPoint)
    {
        Vector3 forwardVector = Vector3.Normalize(destPoint - sourcePoint);

        float dot = Vector3.Dot(Vector3.forward, forwardVector);

        if (Mathf.Abs(dot - (-1.0f)) < 0.000001f)
        {
            return new Quaternion(Vector3.up.x, Vector3.up.y, Vector3.up.z, 3.1415926535897932f);
        }
        if (Mathf.Abs(dot - (1.0f)) < 0.000001f)
        {
            return Quaternion.identity;
        }

        float rotAngle = (float)Mathf.Acos(dot);
        Vector3 rotAxis = Vector3.Cross(Vector3.forward, forwardVector);
        rotAxis = Vector3.Normalize(rotAxis);
        return CreateFromAxisAngle(rotAxis, rotAngle);
    }

    // just in case you need that function also
    public Quaternion CreateFromAxisAngle(Vector3 axis, float angle)
    {
        float halfAngle = angle * .5f;
        float s = (float)System.Math.Sin(halfAngle);
        Quaternion q;
        q.x = axis.x * s;
        q.y = axis.y * s;
        q.z = axis.z * s;
        q.w = (float)System.Math.Cos(halfAngle);
        return q;
    }
}
