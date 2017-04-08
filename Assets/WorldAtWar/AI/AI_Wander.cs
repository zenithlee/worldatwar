using UnityEngine;
using System.Collections;

public class AI_Wander : AI_Agent {
		
	public float WanderTime = 2;
	private Vector3 RandVector ;
	private float Counter = 2;
	public float MaxDeviation = 90;
	// Use this for initialization
	new void Start () {		
	}
	
	// Update is called once per frame
	new void LateUpdate() {		
		
		Counter -= Time.deltaTime ;
		if ( Counter < 0 )
		{
			Counter = WanderTime;
			ChooseRandomPosition( );
		}
		 
		UpdatePositionAndRotation();
	}
	
	void ChooseRandomPosition( )
	{
		//Vector3 rv = new Vector3( Random.value -0.5f , 0, Random.value - 0.5f );
		//TargetWorldPosition = transform.position +  rv * TriggerDistance * 2;		
		
		//choose an angle near our own
		//float angle = Vector3.Angle( transform.position, transform.forward );				
		float angle =  0;
		angle = (Random.value * (MaxDeviation )) - (MaxDeviation/2);
		TargetWorldPosition = transform.position + (Quaternion.Euler( 0, angle,0 ) * transform.forward) * TriggerDistance ;
		//TargetWorldRotation = Quaternion.FromToRotation( transform.position, TargetWorldPosition );
		TargetWorldRotation = Quaternion.LookRotation( TargetWorldPosition - transform.position );
	}
	
	//draw our agent gizmo
	new void OnDrawGizmosSelected() 
	{			
		base.OnDrawGizmosSelected();						
		Gizmos.DrawWireCube( TargetWorldPosition, new Vector3(0.1f,0.1f,0.1f ));
    }
}
