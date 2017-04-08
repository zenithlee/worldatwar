using UnityEngine;
using System.Collections;

public class AI_Avoid : AI_Agent {
		
	private float LazyTimer = 0.5f;
	
	
	private Vector3 RandVector ;
	// Use this for initialization
	new void Start () {
		RandVector = new Vector3(Random.value, 0 , Random.value );
		TargetWorldPosition = transform.position;
		TargetWorldRotation = transform.rotation;
	}
	
	// Update is called once per frame
	new void LateUpdate () {
		
		//are we too close?
		if ( GetDistanceToTarget()  < TriggerDistance )
		{			
			//this lazy timer reduces jerky movement when small movements are made towards me
			LazyTimer -= Time.deltaTime;
			if ( LazyTimer < 0 ) 
			{
				LazyTimer = 0.5f ;
				if ( Mood == Moods.Uncertain ) AvoidUncertain(); else AvoidNormal();
			}
		}		
		UpdatePositionAndRotation();
	}
	
	//draw our agent gizmo
	new void OnDrawGizmosSelected() 
	{			
		base.OnDrawGizmosSelected();						
		Gizmos.DrawWireCube( TargetWorldPosition, new Vector3(0.1f,0.1f,0.1f ));		
    }
	
	void AvoidNormal( )
	{
		Vector3 v1 = Target.position;
		v1.y = transform.position.y;			
		TargetWorldRotation = Quaternion.LookRotation(v1 - transform.position );
		//TargetWorldPosition = transform.position + (TargetWorldRotation * transform.forward) * -AvoidDistance; //step back
		//TargetWorldPosition = transform.position - transform.forward;
		TargetWorldPosition = transform.position + (transform.position-v1).normalized * TriggerDistance;
	}
	
	//usually get out the way, or sometimes forget, sometimes confront
	void AvoidUncertain( )
	{
		TargetWorldRotation = Quaternion.LookRotation(Target.position - transform.position);
		TargetWorldPosition = transform.position + transform.forward * -TriggerDistance + RandVector;
		if ( MaintainHeight ) TargetWorldPosition.y = Target.position.y; //don't affect height
	}
}
