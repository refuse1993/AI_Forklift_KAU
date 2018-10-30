using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;

/**
 * A general labourer class.
 * You should subclass this for specific Labourer classes and implement
 * the createGoalState() method that will populate the goal for the GOAP
 * planner.
 */
public abstract class forklift : MonoBehaviour, IGoap
{
    public float moveSpeed = 1;
    public CheckComponent Check;
    
    private Vector3 destinationPosition;

    // Nav mesh agent
    private float turnSmoothing = 15f;
    private float turnSpeedThreshold = 0.5f;
    private const float stopDistanceProportion = 0.5f;
    private const float navMeshSampleDistance = 4f;
    NavMeshAgent agentt;

    void Start()
    {
        if (Check == null)
        {
            Check = gameObject.AddComponent<CheckComponent>() as CheckComponent;
        }
        agentt = GetComponent<NavMeshAgent>();
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
        worldData.Add(new KeyValuePair<string, object>("state1", Check.num == 0));
        worldData.Add(new KeyValuePair<string, object>("state2", Check.num == 1));
        worldData.Add(new KeyValuePair<string, object>("state3", Check.num == 2));
        worldData.Add(new KeyValuePair<string, object>("state4", Check.num == 3));
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

    public bool moveAgent(GoapAction nextAction)
    {
        Debug.Log("i'm in");
        agentt.SetDestination(nextAction.target.transform.position - new Vector3(0, 0, 2));

        if (agentt.isStopped)
        {
            Debug.Log("i'm in222");
            transform.rotation = Quaternion.Slerp(transform.rotation, nextAction.target.transform.rotation, Time.deltaTime * 2f);
            nextAction.setInRange(true);
            return true;
        }

        if (agentt.remainingDistance <= agentt.stoppingDistance)
        {
            agentt.isStopped = true;
            return false;
        }

        else if (agentt.desiredVelocity.magnitude > turnSpeedThreshold)
        {
            Quaternion targetRotation;
            targetRotation = Quaternion.LookRotation(agentt.desiredVelocity);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, turnSmoothing * Time.deltaTime);
            return false;
        }

         return false;
    }
    

}

