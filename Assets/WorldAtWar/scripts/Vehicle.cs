using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Vehicle : MonoBehaviour {

  public Vector3 Target;
  NavMeshAgent Agent;
  Animator ani;
  public float Speed = 10;
  public float AniSpeedMultiplier = 0.5f;  

  public enum States { Idle, Moving, Attacking };
  public States State = States.Idle;

  TerrainHug Hugger;
  public bool AlignWithTerrain = true;

  Quaternion TargetWorldRotation;

  public void DoSetTarget(Vector3 t)
  {
    Debug.Log("SetTarget:");
    Target = t;
    State = States.Moving;
  }

  public void DoSelect()
  {
    Debug.Log("Select");  
  }

	// Use this for initialization
	void Start () {
    Agent = GetComponent<NavMeshAgent>();
    //Agent.Stop();
    Hugger = GetComponentInChildren<TerrainHug>();
    ani = GetComponentInChildren<Animator>();
  }

  void OnDrawGizmos()
  {
   // Debug.DrawLine(transform.position, Target);
  }

  protected bool LookingAtTarget()
  {
    float Dot = Vector3.Angle(transform.forward, new Vector3(Agent.steeringTarget.x, 0, Agent.steeringTarget.z) - new Vector3(transform.position.x, 0, transform.position.z));   
    if (Mathf.Abs(Dot) < 25) return true;
    return false;
  }

  // Update is called once per frame
  void Update () {
    if ( State == States.Moving) {
      if ( Agent ) { 
        Agent.SetDestination(Target);
      }

      Debug.DrawRay(Agent.steeringTarget, new Vector3(0, 10, 0));

      if ( AlignWithTerrain && Hugger!=null ) { 
        NavMeshHit hit;
        if (NavMesh.SamplePosition(this.transform.position + this.transform.forward*5, out hit, 5.0f, NavMesh.AllAreas))
        {
//          Debug.DrawRay(hit.position, new Vector3(0,10,0));
          Hugger.transform.LookAt(hit.position);
        }  
      }

      if ( !LookingAtTarget())
      {
        Vector3 delta = new Vector3(Agent.steeringTarget.x, 0, Agent.steeringTarget.z) - new Vector3(transform.position.x, 0, transform.position.z);
        if ( delta.magnitude  > 0) { 
          TargetWorldRotation = Quaternion.LookRotation(delta);
        }
        transform.rotation = Quaternion.RotateTowards(transform.rotation, TargetWorldRotation, Agent.angularSpeed* Time.deltaTime);
        Agent.speed = 0;
      }
      else
      {
        Agent.speed = Speed;
      }

      

      //Agent.transform.rotation = Quaternion.Euler(up.x, Agent.transform.rotation.eulerAngles.y, up.z);      
      if (Hugger) {
        Vector3 up = Hugger.upDir;
        Hugger.upDir = Agent.transform.rotation.eulerAngles;
      }
    }

    //Debug.Log(Agent.velocity.magnitude);
    if ( Agent.velocity.magnitude > 0.2 )
    {      
      ani.SetBool("moving", true);
      ani.SetBool("idling", false);
      ani.speed = Agent.velocity.magnitude * AniSpeedMultiplier;      
    }
    else
    {     
      ani.SetBool("moving", false);
      ani.SetBool("idling", true);
      ani.speed = 0.9f+Random.value;
    }
    
  }
}

