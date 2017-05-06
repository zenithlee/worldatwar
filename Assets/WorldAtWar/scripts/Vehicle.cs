using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace WW
{

  public class Vehicle : MonoBehaviour
  {

    public Vector3 Target;
    NavMeshAgent Agent;
    Animator ani;
    public float Speed = 10;
    public float AniSpeedMultiplier = 0.5f;
    public bool TurnBeforeMoving;

    public enum States { Idle, TurnToTarget, Move, Attack, Scout, Die };
    public States State = States.Idle;

    TerrainHug Hugger;
    public bool AlignWithTerrain = true;

    Quaternion TargetWorldRotation;

    public bool IsScout = false;
    public float ScoutTimer = 5;
    public float ScoutTime = 5;
    public float ScoutRadius = 20;

    public float CheckEnemyTimer = 5;
    public float CheckEnemyTime = 5;
    public float CheckEnemyRadius = 5;
    //public Selectable Enemy = null;
    public Projectile MyProjectile;

    Selectable MySelectable;

    // Use this for initialization
    void Start()
    {
      MySelectable = GetComponent<Selectable>();
      Agent = GetComponent<NavMeshAgent>();
      //Agent.Stop();
      Hugger = GetComponentInChildren<TerrainHug>();
      ani = GetComponentInChildren<Animator>();

      if (MySelectable.Data.Type == Types.ConstructionTypes.Sniper)
      {
        ani.SetBool("sniper", true);
      }

      if (MySelectable.Data.Type == Types.ConstructionTypes.Engineer)
      {
        ani.SetBool("engineer", true);
      }
    }

    public void DoSetTarget(Vector3 t)
    {
      //Debug.Log("SetTarget:");
      Target = t;
      //State = States.Move;
      Agent.SetDestination(Target);
      ani.SetBool("attack", false);
      if (State != States.Scout)
      {
        SetState(States.Move);
      }
    }

    public void DoSelect()
    {
      Debug.Log("Select");
    }

    public void DoDie()
    {
      ani.SetBool("attack", false);
      ani.SetBool("idle", false);
      ani.SetBool("move", false);
      ani.SetBool("die", true);
      SetState(States.Die);
    }
    protected bool LookingAtTarget()
    {
      float Dot = Vector3.Angle(transform.forward, new Vector3(Agent.steeringTarget.x, 0, Agent.steeringTarget.z) - new Vector3(transform.position.x, 0, transform.position.z));
      if (Mathf.Abs(Dot) < 25) return true;
      return false;
    }

    void DoMove()
    {
      ani.SetBool("attack", false);
    }

    void DoScout()
    {
      ScoutTimer -= Time.deltaTime;
      if (ScoutTimer < 0)
      {
        ScoutTimer = ScoutTime;
        Vector3 v = transform.position + new Vector3(Random.value * ScoutRadius - ScoutRadius / 2, 0, Random.value * ScoutRadius - ScoutRadius / 2);
        SendMessage("SetTarget", v);
      }
    }

    void FireAt(Vector3 target)
    {
      if (MyProjectile != null)
      {
        GameObject go = Instantiate(MyProjectile.gameObject, this.transform.position + Vector3.up * 5, transform.rotation);
        go.transform.LookAt(target);
        //go.transform.rotation = Quaternion.LookRotation((Enemy.transform.position - transform.position).normalized);
        //go.transform.rotation = Quaternion.Euler(0, 0, 0);

        go.GetComponent<Projectile>().Fire(target);
        SendMessage("DoAction");
        ani.SetBool("attack", true);
        Invoke("StopFire", 0.5f);
      }

      //this.Agent.transform.rotation = Quaternion.Slerp(this.Agent.transform.rotation, Quaternion.LookRotation((target.transform.position - this.transform.position).normalized), Time.deltaTime);                
      //this.Agent.transform.LookAt(target);
    }

    void StopFire()
    {
      ani.SetBool("attack", false);
    }

    void CheckEnemy()
    {
      Debug.DrawLine(this.transform.position, this.transform.position + new Vector3(1, 0, 1) * CheckEnemyRadius);

      if (State == States.Attack)
      {
        Debug.DrawLine(this.transform.position, Target);

        TargetWorldRotation = Quaternion.LookRotation(Target - Agent.transform.position);
        this.Agent.transform.rotation = Quaternion.RotateTowards(transform.rotation, TargetWorldRotation, Agent.angularSpeed * Time.deltaTime);
      }
      CheckEnemyTimer -= Time.deltaTime;
      if (CheckEnemyTimer > 0)
      {
        return;
      }

      CheckEnemyTimer = CheckEnemyTime;

      bool EnemyFound = false;
      Collider[] nearby = Physics.OverlapSphere(this.transform.position, CheckEnemyRadius);
      foreach (Collider c in nearby)
      {
        Selectable s = c.GetComponent<Selectable>();
        if (s == MySelectable) continue;
        if (s != null)
        {
          if (s.Data.Team != MySelectable.Data.Team)
          {
            EnemyFound = true;
            SetState(States.Attack);
            Target = s.transform.position;
          }
        }
      }

      if (EnemyFound == false)
      {
        if (State != States.Scout)
        {
          SetState(States.Idle);
        }
      }
      else
      {
        if (State == States.Attack)
        {
          FireAt(Target);
        }
      }

    }

    void SetState(States newState)
    {
      State = newState;
    }


    void CheckIdle()
    {
      if (Agent.velocity.magnitude < 0.1f)
      {
        ani.SetBool("move", false);
        ani.SetBool("idle", true);
      }
      else
      {
        ani.SetBool("move", true);
        ani.SetBool("idle", false);
      }
    }
    // Update is called once per frame
    void Update()
    {

      CheckIdle();
      if (IsScout && State == States.Idle)
      {
        DoScout();
      }

      CheckEnemy();

      if (State == States.Move)
      {
        if (Agent)
        {
          DoMove();
        }

        Debug.DrawRay(Agent.steeringTarget, new Vector3(0, 10, 0));

        if (AlignWithTerrain && Hugger != null)
        {
          NavMeshHit hit;
          if (NavMesh.SamplePosition(this.transform.position + this.transform.forward * 5, out hit, 5.0f, NavMesh.AllAreas))
          {
            //          Debug.DrawRay(hit.position, new Vector3(0,10,0));
            Hugger.transform.LookAt(hit.position);
          }
        }
        if (State != States.Attack)
        {
          if (!LookingAtTarget())
          {
            Vector3 delta = new Vector3(Agent.steeringTarget.x, 0, Agent.steeringTarget.z) - new Vector3(transform.position.x, 0, transform.position.z);
            if (delta.magnitude > 0)
            {
              TargetWorldRotation = Quaternion.LookRotation(delta);
            }
            transform.rotation = Quaternion.RotateTowards(transform.rotation, TargetWorldRotation, Agent.angularSpeed * Time.deltaTime);
            Agent.speed = 0;
          }
          else
          {
            Agent.speed = Speed;
          }
        }



        //Agent.transform.rotation = Quaternion.Euler(up.x, Agent.transform.rotation.eulerAngles.y, up.z);      
        if (Hugger)
        {          
          Hugger.upDir = Agent.transform.rotation.eulerAngles;
        }
      }

      //Debug.Log(Agent.velocity.magnitude);
      //if ( Agent.velocity.magnitude > 0.2 )
      //{

      // ani.SetBool("attacking", State == States.Attack );     
      // ani.SetBool("moving", State == States.Move);
      // ani.SetBool("idling", State == States.Idle);
      // ani.speed = Agent.velocity.magnitude * AniSpeedMultiplier;      
      //}
      //else
      //{     
      //      ani.SetBool("moving", false);
      //ani.SetBool("idling", true);
      //ani.speed = 0.9f+Random.value;
      //}

    }

    void OnDrawGizmos()
    {
      // Debug.DrawLine(transform.position, Target);
    }

  }



}