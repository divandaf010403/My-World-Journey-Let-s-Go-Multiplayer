using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingZombie : MonoBehaviour
{
    public Transform leftFootTarget;
    public Transform rightFootTarget;
    public AnimationCurve horizontalCurve;
    public AnimationCurve verticalCurve;

    private Vector3 leftTargetOffset;
    private Vector3 rightTargetOffset;

    private float leftLegLast = 0;
    private float rightLegLast = 0;

    // Start is called before the first frame update
    void Start()
    {
        leftTargetOffset = leftFootTarget.localPosition;
        rightTargetOffset = rightFootTarget.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        float leftLegForwardMovement = horizontalCurve.Evaluate(Time.time);
        float rightLegForwardMovement = horizontalCurve.Evaluate(Time.time - 1);


        leftFootTarget.localPosition = leftTargetOffset + 
                    this.transform.InverseTransformVector(leftFootTarget.forward) * horizontalCurve.Evaluate(Time.time) +
                    this.transform.InverseTransformVector(leftFootTarget.up) * verticalCurve.Evaluate(Time.time + 0.5f);
        rightFootTarget.localPosition = rightTargetOffset + 
                    this.transform.InverseTransformVector(rightFootTarget.forward) * horizontalCurve.Evaluate(Time.time - 1) +
                    this.transform.InverseTransformVector(rightFootTarget.up) * verticalCurve.Evaluate(Time.time - 0.5f);

        float leftLegDirection = leftLegForwardMovement - leftLegLast;
        float rightLegDirection = rightLegForwardMovement - rightLegLast;
                    
        RaycastHit hit;
        if(Physics.Raycast(leftFootTarget.position + leftFootTarget.up, -leftFootTarget.up, out hit, Mathf.Infinity) && leftLegDirection < 0)
        {
            leftFootTarget.position = hit.point;
            this.transform.position += this.transform.forward * Mathf.Abs(leftLegDirection);
        }

        if(Physics.Raycast(rightFootTarget.position + rightFootTarget.up, -rightFootTarget.up, out hit, Mathf.Infinity) && rightLegDirection < 0)
        {
            rightFootTarget.position = hit.point;
            this.transform.position += this.transform.forward * Mathf.Abs(rightLegDirection);
        }

        leftLegLast = leftLegForwardMovement;
        rightLegLast = rightLegForwardMovement;
    }
}
