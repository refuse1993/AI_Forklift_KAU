using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;


/**
 * A general labourer class.
 * You should subclass this for specific Labourer classes and implement
 * the createGoalState() method that will populate the goal for the GOAP
 * planner.
 */
public abstract class forklift : MonoBehaviour, IGoap
{
    public float moveSpeed = 1;

    public Camera cam;
    public NavMeshAgent agent;

    private Transform tr;

    private bool inRange = false;

    private float turnSmoothing = 15f;
    private float turnSpeedThreshold = 0.5f;
    private const float stopDistanceProportion = 0.1f;
    private const float navMeshSampleDistance = 4f;
    public CheckComponent Check;



    HashSet<KeyValuePair<string, object>> worldData = new HashSet<KeyValuePair<string, object>>();

    void Start()
    {
        if (Check == null)
        {
            Check = gameObject.AddComponent<CheckComponent>() as CheckComponent;
        }
    }


    void Update()
    {

    }

    /**
	 * Key-Value data that will feed the GOAP actions and system while planning.
	 */
    public HashSet<KeyValuePair<string, object>> getWorldState()
    {
        HashSet<KeyValuePair<string, object>> worldData = new HashSet<KeyValuePair<string, object>>();

        worldData.Add(new KeyValuePair<string, object>("fault", Check.fault == 1));
        worldData.Add(new KeyValuePair<string, object>("state1", Check.num == 0 ));
        worldData.Add(new KeyValuePair<string, object>("state2", Check.num == 1 || Check.num == 4 ));
        worldData.Add(new KeyValuePair<string, object>("state3", Check.num == 2 || Check.num == 5));
        worldData.Add(new KeyValuePair<string, object>("state4", Check.num == 3 || Check.num == 6));
        worldData.Add(new KeyValuePair<string, object>("boxon", Check.boxon == 0));


        return worldData;
    }

    /**
	 * Implement in subclasses
	 */
    public abstract HashSet<KeyValuePair<string, object>> createGoalState();



    public void planFailed(HashSet<KeyValuePair<string, object>> failedGoal)
    {
        // Not handling this here since we are making sure our goals will always succeed.
        // But normally you want to make sure the world state has changed before running
        // the same goal again, or else it will just fail.


    }

    public void planFound(HashSet<KeyValuePair<string, object>> goal, Queue<GoapAction> actions)
    {
        // Yay we found a plan for our goal
        Debug.Log("<color=green>Plan found</color> " + GoapAgent.prettyPrint(actions));
    }

    public void actionsFinished()
    {
        // Everything is done, we completed our actions for this gool. Hooray!
        Debug.Log("<color=blue>Actions completed</color>");
    }

    public void planAborted(GoapAction aborter)
    {
        // An action bailed out of the plan. State has been reset to plan again.
        // Take note of what happened and make sure if you run the same goal again
        // that it can succeed.
        Debug.Log("<color=red>Plan Aborted</color> " + GoapAgent.prettyPrint(aborter));
    }

    /*public bool isInRange()
    {
        if(agent.remainingDistance <= 0.2f && agent.velocity.magnitude >= 0.2f) inRange = true;
       
        this.inRange = inRange;

        return inRange;
    }*/

    public bool moveAgent(GoapAction nextAction)
    {
        agent.SetDestination(nextAction.target.transform.position);

        if (agent.pathPending)
            return false;

        if (agent.remainingDistance <= 0.3f)//근처에 들어오고
        {
            // we are at the target location, we are done
            Quaternion rot = Quaternion.LookRotation(nextAction.target.transform.position - transform.position);
            if (agent.isStopped)
            {
                nextAction.setInRange(true);
                agent.isStopped = false;
                return true;
            }
            else
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, 3f);
            }
            Debug.Log(Quaternion.Angle(nextAction.target.transform.rotation, transform.rotation));

            if (Quaternion.Angle(nextAction.target.transform.rotation, transform.rotation) >= 2 && Quaternion.Angle(nextAction.target.transform.rotation, transform.rotation) <= 3.2)
                agent.isStopped = true;

        }

        return false;
    }

}

