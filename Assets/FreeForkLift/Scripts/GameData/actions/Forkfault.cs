using System;
using UnityEngine;
using System.Collections;

public class Forkfault : GoapAction
{

    private bool fault = false;


    public Forkfault()
    {
        addPrecondition("fault", true); // if we have ore we don't want more
        addEffect("complete", true);
    }


    public override void reset()
    {
        fault = false;
    }

    public override bool isDone()
    {
        return fault;
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
        float j = Input.GetAxis("Horizontal");
        if (j != 0)
        {
            CheckComponent check = (CheckComponent)agent.GetComponent(typeof(CheckComponent));
            check.fault = 0;
            fault = true;
        }
        Debug.Log("*****fault****");
        return true;
    }

}