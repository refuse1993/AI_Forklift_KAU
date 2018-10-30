//---------------------------------------------------------------- Unity Engine
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;


public class Character : MonoBehaviour
{
    private NavMeshAgent agent;
    private Vector3 destinationPosition;

    // Nav mesh agent
    private float turnSmoothing = 15f;
    private float turnSpeedThreshold = 0.5f;
    private const float stopDistanceProportion = 0.1f;
    private const float navMeshSampleDistance = 4f;
    

    GameObject des;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.gameObject.SetActive(false);
        destinationPosition = transform.position;
        agent.gameObject.SetActive(true);
        agent.updateRotation = false;
        des = GameObject.FindGameObjectWithTag("Finish");
        agent.SetDestination(des.transform.position - new Vector3(0,0,3f) );
        agent.isStopped = false;
    }
    

    //-----------------------------------------------------------------------------
    // Update
    //-----------------------------------------------------------------------------
    private void Update()
    {
        if (agent.pathPending)
            return;

        if (agent.isStopped) 
            transform.rotation = Quaternion.Slerp(transform.rotation, des.transform.rotation, Time.deltaTime * 2f );
            
        if (agent.remainingDistance <= agent.stoppingDistance * stopDistanceProportion)
            Stopping();
            
            
            
        
        else if (agent.desiredVelocity.magnitude > turnSpeedThreshold)
            Heading();
    }

    //-----------------------------------------------------------------------------
    // Stopping
    //-----------------------------------------------------------------------------
   

    private void Stopping()
    {
        if (agent.isStopped)
            return;

        agent.isStopped = true;
    }

    //-----------------------------------------------------------------------------
    // Heading
    //-----------------------------------------------------------------------------
    private void Heading()
    {
        Quaternion targetRotation;
        targetRotation = Quaternion.LookRotation(agent.desiredVelocity);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, turnSmoothing * Time.deltaTime);
    }

    //-----------------------------------------------------------------------------
    // OnGroundClick
    //-----------------------------------------------------------------------------
    
    
    
    /*
     * public void OnGroundClick(BaseEventData data)
    {
        PointerEventData pData = (PointerEventData)data;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(pData.pointerCurrentRaycast.worldPosition, out hit, navMeshSampleDistance, NavMesh.AllAreas))
            destinationPosition = hit.position;
        else
            destinationPosition = pData.pointerCurrentRaycast.worldPosition;

        agent.SetDestination(destinationPosition);
        agent.isStopped = false;
    }
    */
}


