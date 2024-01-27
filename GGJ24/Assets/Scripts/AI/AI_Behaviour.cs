using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI_Behaviour : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;

    public float fleeDistance;

    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    public float sightRange;
    public bool playerInSightRange;
    int screamCounter = 0;
    bool scream = false;


    public string screamMale = "event:/Characters/Woman_base/Woman_scream";
    public string screamFemale = "event:/Characters/Man_base/Man_Scream";

    FMOD.Studio.EventInstance MaleScreamEv;
    FMOD.Studio.EventInstance FemaleScreamEv;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        MaleScreamEv = FMODUnity.RuntimeManager.CreateInstance(screamMale);
        FemaleScreamEv = FMODUnity.RuntimeManager.CreateInstance(screamFemale);
    }
    private void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);

        if(!playerInSightRange)
        {
            Patrolling();
        }
        if(playerInSightRange)
        {
            Fleeing();
            scream = true;
        }
    }
    private void Patrolling()
    {
        if(!walkPointSet)
        {
            SearchWalkPoint();
        }
        if(walkPointSet)
        {
            agent.SetDestination(walkPoint);
        }
        Vector3 distanceToWalk = transform.position - walkPoint;

        if(distanceToWalk.magnitude < 1f)
        {
            walkPointSet = false;
        }
        
    }

    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        Vector3 randomOffset = new Vector3(randomX, 0, randomZ);

        if(Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
        {
            walkPointSet = true;
        }
        Ray ray = new Ray(transform.position + randomOffset + Vector3.up * 100f, Vector3.down);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, Mathf.Infinity, whatIsGround))
        {
            walkPoint = hit.point;
            walkPointSet = true;
        }


    }
    private void Fleeing()
    {
       

        if (Vector3.Distance(transform.position, player.position) < fleeDistance)
        {
            Vector3 fleeDirection = transform.position - player.position;
            Vector3 fleeDestination = transform.position + fleeDirection.normalized * fleeDistance;

            agent.SetDestination(fleeDestination);

        }

        if (this.gameObject.tag == "NPCFemale")
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/Characters/Woman_base/Woman_scream", this.transform.position);

        }
        else
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/Characters/Man_base/Man_Scream", this.transform.position);
      
        }
    }
}
