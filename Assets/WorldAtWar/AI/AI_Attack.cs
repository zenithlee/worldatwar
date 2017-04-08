using UnityEngine;
using System.Collections;

public class AI_Attack : AI_Agent {
	
	public Object MissileObject;
	public Vector3 Offset = new Vector3(0,1,0);
	public float FireTime = 3;
	public float AttackForce = 1000;
	float FireCounter = 0;
	public bool FireAtTarget = true;
	// Use this for initialization
	new void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
	
		if ( Target == null ) return;		
		
		CheckForFiring( );
	}
	
	void CheckForFiring( )
	{
		FireCounter += Time.deltaTime ;
		if (( FireCounter >= FireTime ) && (GetDistanceToTarget() < TriggerDistance ))
		{
			FireCounter = 0;
			if ( MissileObject )
			{
				GameObject o ; 
				if ( FireAtTarget )
				{
					Quaternion rel = Quaternion.LookRotation( Target.position- transform.position );
					o = Instantiate( MissileObject, this.transform.position + rel * Offset,rel  ) as GameObject;				
				}
				else
				{
					o = Instantiate( MissileObject, this.transform.position + transform.rotation * Offset, this.transform.rotation ) as GameObject;
				}
				if ( o )
				{
					AI_Missile aim = o.GetComponent<AI_Missile>( );
					aim.Owner = this;
					aim.Target = Target;
					if ( aim ) aim.Fire( AttackForce );
				}
			}
		}
	}
}
