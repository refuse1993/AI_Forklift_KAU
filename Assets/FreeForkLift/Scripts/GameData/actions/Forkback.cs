using System;
using UnityEngine;
using System.Collections;

public class Forkback : GoapAction
{
    public Transform fork;
    public Transform mast;
    public float speedTranslate; //Platform travel speed
    public Vector3 maxY; //The maximum height of the platform
    public Vector3 minY; //The minimum height of the platform
    public Vector3 maxYmast; //The maximum height of the mast
    public Vector3 minYmast; //The minimum height of the mast

    private int flag = 0;
    private bool parent = false;
    private bool mastMoveTrue = false; //Activate or deactivate the movement of the mast

    private bool reached = false;


    public Forkback()
    {
        addPrecondition("state4", true); // if we have ore we don't want more
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
        if (parent == false)
        {
            ptransform.transform.parent = transform;
            parent = true;
        }
        
        float step = 0.03f;
        Debug.Log("im in");
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, step);
        flag += 1;
        if (flag > 160)
        {
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
            if (fork.transform.position.y < minY.y + 0.01 && fork.transform.position.y > minY.y - 0.01)
            {
                CheckComponent check = (CheckComponent)agent.GetComponent(typeof(CheckComponent));
                check.num += 1;
                if(check.num == 4)
                {
                    check.boxon = 1;
                }
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

            fork.transform.Translate(Vector3.down * speedTranslate);
            if (mastMoveTrue)
            {
                mast.transform.Translate(Vector3.down * speedTranslate);
            }


            float h = Input.GetAxis("Jump");
            if (h != 0)
            {
                CheckComponent check = (CheckComponent)agent.GetComponent(typeof(CheckComponent));
                check.fault = 1;
                reached = true;
            }
        }
        Debug.Log("why not?");
        return true;
    }

}