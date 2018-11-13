using System;
using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class Forkreach : GoapAction
{
    public Transform fork;
    public Transform mast;
    public float speedTranslate; //Platform travel speed
    public Vector3 maxY; //The maximum height of the platform
    public Vector3 minY; //The minimum height of the platform
    public Vector3 maxYmast; //The maximum height of the mast
    public Vector3 minYmast; //The minimum height of the mast
    public Transform goalComponents;
    public Transform GoalTransform;

    private bool mastMoveTrue = false; //Activate or deactivate the movement of the mast

    private bool reached = false;
    

    public Forkreach()
    {
        addPrecondition("state2", true); // if we have ore we don't want more
        addEffect("state2-1", true);
        
    }


    public override void reset()
    {
        reached = false;
    }

    public override bool isDone()
    {
        return reached;
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
        TargetComponent tar = (TargetComponent)agent.GetComponent(typeof(TargetComponent));
        CheckComponent check = (CheckComponent)agent.GetComponent(typeof(CheckComponent));
        if (check.boxon == 0) { 
            target = tar.targ2;
        }
        else
        {
            target = tar.GoalT2;
        }
        return true;
    }
    
    public override bool perform(GameObject agent)
    {
        CheckComponent check = (CheckComponent)agent.GetComponent(typeof(CheckComponent));
        if ( check.boxon == 1)
        {
            goalComponents = GoalTransform;
        }

        if (fork.transform.position.y >= maxYmast.y)
        {
            mastMoveTrue = true;
        }
        if (fork.transform.position.y <= maxYmast.y)
        {
            mastMoveTrue = false;
        }
        if (fork.transform.position.y > maxY.y)
        {
            fork.transform.position = new Vector3(fork.transform.position.x, maxY.y, fork.transform.position.z);
        }

        if (fork.transform.position.y < goalComponents.transform.position.y + 0.02 && fork.transform.position.y > goalComponents.transform.position.y - 0.02)
        {
            reached = true;
        }

        if (fork.transform.position.y < minY.y)
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

        fork.Translate(Vector3.up * speedTranslate);
        if (mastMoveTrue)
            {
                mast.Translate(Vector3.up * speedTranslate);
            }
        Debug.Log("llll");
        return true;
    }

}