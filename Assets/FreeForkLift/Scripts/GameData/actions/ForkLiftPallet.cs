using System;
using UnityEngine;
using System.Collections;


public class ForkLiftPallet : GoapAction
{

    private bool reach = false;
    public Transform fork;
    public Transform pallet;

    public ForkLiftPallet()
    {
        addPrecondition("state3", true); // if we have ore we don't want more
        addEffect("complete", true);
    }


    public override void reset()
    {
        reach = false;
    }

    public override bool isDone()
    {
        return reach;
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
        float h = Input.GetAxis("Jump");
        if (j != 0)
        {
            CheckComponent check = (CheckComponent)agent.GetComponent(typeof(CheckComponent));
            check.num += 1;
            reach = true;
        }
        if (h != 0)
        {
            CheckComponent check = (CheckComponent)agent.GetComponent(typeof(CheckComponent));
            check.fault = 1;
            reach = true;
        }

        Debug.Log("ssss");
        return true;
    }

}