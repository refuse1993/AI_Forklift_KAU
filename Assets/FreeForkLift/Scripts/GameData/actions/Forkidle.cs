using System;
using UnityEngine;
using System.Collections;

public class Forkidle : GoapAction
{

    private bool wait = false;
    

    public Forkidle()
    {
        addPrecondition("state1", true); // if we have ore we don't want more
        addEffect("complete", true);
    }


    public override void reset()
    {
        wait = false;
    }

    public override bool isDone()
    {
        return wait;
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
        float j = Input.GetAxis("Horizontal");
        if (j != 0)
        {
            CheckComponent check = (CheckComponent)agent.GetComponent(typeof(CheckComponent));
            check.num += 1;
            wait = true;
        }
        Debug.Log("lllsml");
        return true;
    }

}