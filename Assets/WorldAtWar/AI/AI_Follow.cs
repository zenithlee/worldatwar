using UnityEngine;
using System.Collections;


public class AI_Follow : AI_Agent {	
	
	public Vector3 Offset = new Vector3( 0,0,-2); //right, up, forward	
	//private Vector3 TinyVector = new Vector3(0.00001f, 0.00001f, 0.00001f );
	public bool Instant = false;
	
	// Use this for initialization
	new void Start () {
		 base.Start();
		/*if (( ani ) && ( ForwardClip ))
		{
			ani.AddClip( ForwardClip, ForwardClip.name );	
			ani.playAutomatically = true;
			ForwardClip.wrapMode = WrapMode.Loop;				
		}*/
		
	}
	
	new void OnDrawGizmosSelected( )
	{	
		base.OnDrawGizmosSelected( );
		//base.OnDrawGizmosSelected( );
		Gizmos.color = new Color( 0.2f,0.1f,0.2f, 0.5f );
		Gizmos.DrawLine( TargetWorldPosition, TargetWorldPosition + new Vector3( 0,1,0));
		
		Gizmos.color = new Color(0.1f,0.1f,0.7f, 0.5f );
			Gizmos.DrawLine ( transform.position, transform.position + transform.forward * TriggerDistance );
	}
	
	
	void FixedUpdate () {
		
		if ( !Target ) return;
		

		
		Vector3 WorldOffset = Target.rotation * Offset;		
		TargetWorldPosition = Target.position + WorldOffset ;
		
		
		//if ( GetDistanceToTargetPosition() > TriggerDistance )
		{
		//	TargetWorldRotation = Quaternion.LookRotation(TargetWorldPosition - transform.position + TinyVector);
		}
		//else
		//{//
			TargetWorldRotation = Quaternion.LookRotation(Target.position - transform.position);
		//}
		
		if ( Instant ) 
		{
			transform.position = TargetWorldPosition;
			transform.rotation = TargetWorldRotation;
		}
		else
		UpdatePositionAndRotation();
	}
}
