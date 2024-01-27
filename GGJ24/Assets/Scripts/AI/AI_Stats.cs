using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI_Stats : MonoBehaviour
{
    NavMeshAgent agent;
    public float minSpeed;
    public float maxSpeed;
    private float speed;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        speed = Random.Range(minSpeed, maxSpeed);
        Debug.LogWarning(speed);
    }

    private void Update()
    {
        agent.speed = speed;
    }
}
