using UnityEngine;
using System.Collections;

//Orbits me around the Target
//Speed determines how quickly I reach my target position, 
//while TurnSpeed determines how quickly I orbit the Target
public class AI_Orbit : AI_Agent {		
		
	public Vector3 RotationOffset = new Vector3( 0, -2, 0);
	private Vector3 Offset = new Vector3(0,0,0);
	
	
	// Use this for initialization
	new void Start () {
		base.Start();
		if ( TurnFirst == true ) Debug.Log ( "AI_ORBIT:WARNING - TurnFirst is on, this may lead to undesirable results" );
	}
	
	new void OnDrawGizmosSelected( )
	{	
		//base.OnDrawGizmos( );
		
		Gizmos.color = Color.green;
		Gizmos.DrawWireCube( TargetWorldPosition, new Vector3(0.1f,0.1f,0.1f) );
		Gizmos.color = Color.white;
		Gizmos.DrawLine ( transform.position, transform.position + transform.forward * TriggerDistance );
		Gizmos.color = Color.red;
		Gizmos.DrawLine ( transform.position, TargetWorldPosition );
		
		Vector3 pos = Target.position;
		if ( MaintainHeight ) pos.y = transform.position.y;
		
		DrawCircleGizmo( pos, TriggerDistance );
		
		
	}
	
	// Update is called once per frame
	new void Update () {
		
	
		if ( !Target ) return;
		
		base.Update ();
		
		float timer = Time.fixedTime * TurnSpeed ;
		Offset.x = Mathf.Sin ( timer ) * TriggerDistance;
		Offset.y = 0;
		Offset.z = Mathf.Cos( timer ) * TriggerDistance;
		TargetWorldPosition = Target.position + Offset;				
		TargetWorldRotation = Quaternion.LookRotation(Target.position - TargetWorldPosition) * Quaternion.Euler( RotationOffset );
		transform.rotation = TargetWorldRotation;
		
		UpdatePositionAndRotation( );
		
		
	}
}
