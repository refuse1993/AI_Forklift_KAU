using System;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class GetObject : GoapAction
{
    public NavMeshAgent agent;
    
    public Transform fork;
    public Transform mast;
    public float speedTranslate; //Platform travel speed
    public Vector3 maxY; //The maximum height of the platform
    public Vector3 minY; //The minimum height of the platform
    public Vector3 maxYmast; //The maximum height of the mast
    public Vector3 minYmast; //The minimum height of the mast
    public Transform goalComponents;

    private bool mastMoveTrue = false; //Activate or deactivate the movement of the mast

    private bool arrived = false;

    public GetObject()
    {
        addPrecondition("stop", true);
        addPrecondition("isObject", false); // if we have ore we don't want more
        addEffect("isObject", true);
        addEffect("stop", false);
    }


    public override void reset()
    {
        arrived = false;
    }

    public override bool isDone()
    {
        return arrived;
    }

    public override bool requiresInRange()
    {
        return true; // yes we need to be near a rock
    }

    public override bool isInRange()
    {
        return inRange;
    }

    public override void setInRange(bool inRange)
    {
        this.inRange = inRange;
    }

    public override bool checkProceduralPrecondition(GameObject agent)
    {
        return true;
    }

    public override bool perform(GameObject agent)
    {

        if (fork.transform.position.y >= maxYmast.y)
        {
            mastMoveTrue = true;
        }
        if (fork.transform.position.y <= maxYmast.y)
        {
            mastMoveTrue = false;
        }
        if (fork.transform.position.y >= maxY.y)
        {
            fork.transform.position = new Vector3(fork.transform.position.x, maxY.y, fork.transform.position.z);
        }

        if (fork.transform.position.y <= minY.y)
        {
            fork.transform.position = new Vector3(fork.transform.position.x, minY.y, fork.transform.position.z);
        }
        if (mast.transform.position.y >= maxYmast.y)
        {
            mast.transform.position = new Vector3(mast.transform.position.x, maxYmast.y, mast.transform.position.z);
        }

        if (mast.transform.position.y <= minYmast.y)
        {
            mast.transform.position = new Vector3(mast.transform.position.x, minYmast.y, mast.transform.position.z);
        }

        fork.Translate(Vector3.up * speedTranslate * Time.deltaTime);
        if (mastMoveTrue)
        {
            mast.Translate(Vector3.up * speedTranslate * Time.deltaTime);
        }
        //Debug.Log(fork.transform.position.y);
        //Debug.Log(goalComponents.position.y);

        if (fork.transform.position.y >= goalComponents.position.y) arrived = true;

        return true;
    }
}
