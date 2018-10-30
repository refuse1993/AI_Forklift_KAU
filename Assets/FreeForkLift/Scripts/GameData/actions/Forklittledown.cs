using System;
using UnityEngine;
using System.Collections;

public class Forklittledown : GoapAction
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


    public Forklittledown()
    {
        addPrecondition("state2-1", true); // if we have ore we don't want more
        addPrecondition("boxon", false);
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
        return false; // yes we need to be near a rock
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

        CheckComponent check = (CheckComponent)agent.GetComponent(typeof(CheckComponent));

        fork.Translate(Vector3.down * speedTranslate);
        Debug.Log("littledown");
        check.updown -= 1;

        if (check.updown == 0)
        {
            check.num += 1;
            check.boxon = 0;
            reached = true;
        }
        return true;
    }

}