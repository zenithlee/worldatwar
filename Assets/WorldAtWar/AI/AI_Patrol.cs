using UnityEngine;
using System.Collections;

/**
 * AI_Patrol follows a set of waypoints
 * Drag a Waypoint prefab into the scene
 * and then into my Waypoints Array from the hierarchy
 * 
 * */

public class AI_Patrol : AI_Agent {
		
	public GameObject[] WayPoints = new GameObject[0];
	public int WayPointNumber = 0;	
	
	public float WaitCounter = 0;
	// Use this for initialization
	new void Start () {		
	}
	
	// Update is called once per frame
	new void Update () {
		base.Update();
		
		if ( WayPoints.Length == 0 ) return;
		Target = WayPoints[WayPointNumber].transform;
		if ( !Target )  return;
		
		TargetWorldPosition = Target.position;
		TargetWorldRotation = Quaternion.LookRotation( TargetWorldPosition - transform.position );
		
		if ( GetDistanceToTarget() < TriggerDistance )
		{
			GameObject go = WayPoints[WayPointNumber];
			AI_Waypoint wp = go.GetComponent<AI_Waypoint>();
			if ( wp != null )
			{
				WaitCounter += Time.deltaTime;
				if( WaitCounter >= wp.WaitTime ) NextWaypoint( );
			}
			else
				NextWaypoint( );
		}
		
		UpdatePositionAndRotation();
	}
	
	
	void NextWaypoint( )
	{
		if( WayPointNumber < WayPoints.Length-1 ) WayPointNumber++;
			else WayPointNumber = 0;
		WaitCounter = 0;
	}
	
	
	//draw our agent gizmo
	new void OnDrawGizmosSelected() 
	{	
			//base.OnDrawGizmos();
			Gizmos.color = new Color(1f,0.1f,0.7f, 0.5f );
			Gizmos.DrawLine ( transform.position, transform.position + transform.forward * TriggerDistance );
			
			Vector3 v1 = new Vector3();
			Vector3 v2 = new Vector3();
			for ( int i=0; i< WayPoints.Length; i++ )
			{				
				v1 = WayPoints[i].transform.position;
				
				if ( i < WayPoints.Length-1 ) v2 = WayPoints[i+1].transform.position; else v2 = WayPoints[0].transform.position;
				
				if ( i == WayPointNumber-1 ) Gizmos.color = new Color( 0.5f, 0.1f, 0.1f, 0.5f );
				else Gizmos.color = new Color(0.1f,0.1f,0.7f, 0.5f );
				Gizmos.DrawLine ( v1, v2 );
			}
	}    
}
