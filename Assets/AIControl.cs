using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIControl : MonoBehaviour {

    public GameObject [] goalLocations;
    NavMeshAgent agent;
    Animator anim;

    void Start() {

        agent = this.GetComponent<NavMeshAgent>();
        goalLocations = GameObject.FindGameObjectsWithTag("goal");
        int i = Random.Range(0,goalLocations.Length);
        agent.SetDestination(goalLocations[i].transform.position);
      
    }


    void Update() {

    }
}