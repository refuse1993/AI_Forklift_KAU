using System;
using UnityEngine;
using System.Collections;

public class Forklittleup : GoapAction
{
    public Transform fork;
    public Transform mast;
    public float speedTranslate; //Platform travel speed
    public Vector3 maxY; //The maximum height of the platform
    public Vector3 minY; //The minimum height of the platform
    public Vector3 maxYmast; //The maximum height of the mast
    public Vector3 minYmast; //The minimum height of the mast
    public Transform goalComponents;

    private bool mastMoveTrue = false; //Activate or deactivate the movement of the mast

    private bool reached = false;


    public Forklittleup()
    {
        addPrecondition("state2-1", true); // if we have ore we don't want more
        addPrecondition("boxon", true);
        addEffect("complete", true);
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
        CheckComponent check = (CheckComponent)agent.GetComponent(typeof(CheckComponent));
        if (check.boxon == 0)
        {
            target = forklift.Target[check.tcount].targ1;
        }
        else
        {
            target = forklift.Target[check.tcount].GoalT1;
        }
        return true;
    }

    public override bool perform(GameObject agent)
    {
        CheckComponent check = (CheckComponent)agent.GetComponent(typeof(CheckComponent));
       
        fork.Translate(Vector3.up * speedTranslate);
        Debug.Log("littleup");
        check.updown += 1;
 
        if(check.updown == 15)
        {
            check.num += 1;
            reached = true;
        }
        return true;
    }

}