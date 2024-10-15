using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderConstraintController : MonoBehaviour
{
    Vector3 originalPosition;
    public GameObject moveCobe;
    public float legMoveSpeed = 7f;
    public float moveDistance = 0.7f;
    public float moveStoppingDistance = 0.4f;
    public SpiderConstraintController oppositeLeg;
    bool isMoving = false;
    bool moving = false;

    void Start()
    {
        originalPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // transform.position = originalPosition;
        float distanceToMoveCubes = Vector3.Distance(transform.position, moveCobe.transform.position);
        if ((distanceToMoveCubes >= moveDistance && !oppositeLeg.isItMoving()) || moving)
        {
            moving = true;
            transform.position = Vector3.Lerp(transform.position, moveCobe.transform.position + new Vector3(0f, 0.3f, 0f), Time.deltaTime * legMoveSpeed);
            originalPosition = transform.position;
            isMoving = true;
            if(distanceToMoveCubes < moveStoppingDistance)
            {
                moving = false;
            }
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, originalPosition + new Vector3(0f, -0.3f, 0f), Time.deltaTime * legMoveSpeed * 3f);
            isMoving = false;
        }
    }

    public bool isItMoving() 
    {
        return isMoving;
    }
}
