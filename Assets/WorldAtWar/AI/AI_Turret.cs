using UnityEngine;
using System.Collections;

//"Turrent that searches for Target. Use FieldOfView and ScanMin/Max to define scan region"
public class AI_Turret : AI_Agent {
	
	
	public bool Scanning = true;
	public float ScanMin = 180;
	public float ScanMax = 359;
	public float FieldOfView = 40;
	
	public bool RandomScan = false;
	
	private float ScanningTime = 1;
	private float ScanDirection = 1;
	private float ScanAmt = 0;	
	private float ScanMultiplier = 10 ; //otherwise values aren't similar to values with other scripts
	
	// Use this for initialization
	new void Start () {
		base.Start ( );
		ScanAmt= ScanMin;
	}
	
	new void OnDrawGizmosSelected( )
	{		
			if ( ScanMax < ScanMin ) Gizmos.color = new Color( Random.value, 0, Random.value );
			else Gizmos.color = Color.white;
			
			//Scan min/max
			Vector3 v2 = Quaternion.Euler( 0, ScanMin , 0 ) * Vector3.forward * TriggerDistance*0.9f + transform.position;
			Gizmos.DrawLine( transform.position, v2);
			Vector3 v2b = Quaternion.Euler( 0, ScanMin+5 , 0 ) * Vector3.forward * TriggerDistance*0.9f + transform.position;
			Gizmos.DrawLine( v2, v2b);			
			
			Vector3 v3 = Quaternion.Euler( 0, ScanMax , 0 ) * Vector3.forward * TriggerDistance*0.9f + transform.position;
			Gizmos.DrawLine( transform.position, v3);
			Vector3 v3b = Quaternion.Euler( 0, ScanMax-5 , 0 ) * Vector3.forward * TriggerDistance*0.9f + transform.position;
			Gizmos.DrawLine( v3, v3b);
			
			//field of view
			Gizmos.color = Color.green;
			Vector3 v4 = Quaternion.Euler( 0, -FieldOfView/2 , 0 ) * transform.forward * TriggerDistance + transform.position;
			Gizmos.DrawLine( transform.position, v4);			
			Vector3 v5 = Quaternion.Euler( 0, FieldOfView/2 , 0 ) * transform.forward * TriggerDistance + transform.position;
			Gizmos.DrawLine( transform.position, v5);
			
			base.OnDrawGizmosSelected( );			
		
	}
	
	/*
	public static void RotateY( this Vector3 v, float angle )
    {
        float sin = Mathf.Sin( angle );
        float cos = Mathf.Cos( angle );
       

        float tx = v.x;
        float tz = v.z;
        v.x = (cos * tx) + (sin * tz);
        v.z = (cos * tz) - (sin * tx);
    }
    */	
	
	new void Update () {
		
		base.Update ( );
		if (( GetDistanceToTarget() < TriggerDistance ) && ( CanSeeTarget() ) && IsTargetInFieldOfView( FieldOfView ) )
		{		
				TargetWorldPosition = transform.position;
				Vector3 delta = Target.position - transform.position;
			
				TargetWorldRotation = Quaternion.LookRotation( delta );										
				State = States.Attack;		
		}
		else if ( Scanning )
		{
				if ( RandomScan )
				{
					ScanningTime -= Time.deltaTime * Speed * Time.deltaTime; 
					if (ScanningTime <0 ) 
					{
						ScanningTime = 1;
						TargetWorldRotation = Quaternion.Euler( 0, ScanMin + Random.value*(ScanMax-ScanMin), 0 );
					}
				}
				else
				{
					ScanAmt+=ScanDirection * TurnSpeed * Time.deltaTime * ScanMultiplier;
					if ( ScanAmt > ScanMax ) {ScanDirection = -1; ScanAmt = ScanMax; }
					if ( ScanAmt < ScanMin ) {ScanDirection = 1; ScanAmt = ScanMin;}
					TargetWorldRotation = Quaternion.Euler( 0, ScanAmt, 0 );
				}
			
			State = States.Patrol;
		}
		else
		{
			State = States.Idle;
		}
		
		UpdateRotation ( );		
	}
	
}
