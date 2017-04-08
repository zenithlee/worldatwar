using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Vehicle : MonoBehaviour {

  public Vector3 Target;
  NavMeshAgent Agent;
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
  }

  void OnDrawGizmos()
  {
   // Debug.DrawLine(transform.position, Target);
  }

  protected bool LookingAtTarget()
  {
    /*TargetWorldRotation = Quaternion.LookRotation(new Vector3(Target.x, 0, Target.z) - new Vector3(transform.position.x,0, transform.position.z));  
    float Dot = Quaternion.Dot(TargetWorldRotation, transform.rotation);
    Debug.Log(Mathf.Abs(Dot));
    if (Mathf.Abs(Dot) > 0.99) return true;
    return false;*/
    float Dot = Vector3.Angle(transform.forward, new Vector3(Agent.steeringTarget.x, 0, Agent.steeringTarget.z) - new Vector3(transform.position.x, 0, transform.position.z));
    //Debug.Log(Dot);
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
        TargetWorldRotation = Quaternion.LookRotation(new Vector3(Agent.steeringTarget.x, 0, Agent.steeringTarget.z) - new Vector3(transform.position.x, 0, transform.position.z));
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
      Animator ani = GetComponent<Animator>();
      ani.SetBool("moving", true);
      ani.SetBool("idling", false);
      ani.speed = Agent.velocity.magnitude * AniSpeedMultiplier;      
    }
    else
    {
      Animator ani = GetComponent<Animator>();      
      ani.SetBool("moving", false);
      ani.SetBool("idling", true);
      ani.speed = 0.9f+Random.value;
    }
    
  }
}

