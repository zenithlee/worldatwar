using UnityEngine;
using System.Collections;

/**
 * 
 * Base class AI_Agent with no functionality other than distance to the Target
 * To use an Agent drag the appropriate AI script onto an object, 
 * and then drag another object (usually the player) into the Target slot from the hierarchy
 * 
 */

public class AI_Agent : MonoBehaviour {
	
	public int ID = 1;
	public Transform Target;	
	public float DistanceToTarget = 0 ;
	public float TriggerDistance = 5; //trigger the state when under or over this distance (depends on state)	
	public float Speed = 1;	
	public bool ConstantSpeed = false;
	public float TurnSpeed = 2;
	public bool TurnFirst = true;
	public bool ForwardSmooth = false;
	
	public float Health = 1;
	
	public enum Moods {Normal, Uncertain, Aggressive, Cowardly, Hunting};
	public Moods Mood = Moods.Normal; //Normal, Uncertain, Aggressive, Cowardly, Hunting
	public enum States {Idle, Patrol, Follow, Attack, Orbit, Avoid};
	public States State = States.Idle;
	public bool MaintainHeight = false;
	
	private Animation ani ;
	
	public Vector3 TargetWorldPosition; //the position we are aiming for
	public Quaternion TargetWorldRotation; //the rotation we are aiming for
	protected float MaxIdleDistance = 0.05f; // the maximum distance we can move to remain in idle ani, prevents many transitions with small movements
	
	//obstacle avoidance
	public bool ObstructionAvoidance = true;
	public bool Obstructed = false;
	public float ObstructionTimer = 0.5f;
	public Vector3 TemporaryWorldPosition; //the position we are aiming for
	public Quaternion TemporaryWorldRotation; //the rotation we are aiming for
	public float FeelerWidth = 2;
	private Vector3 FeelerA = new Vector3(0,0,0);
	private Vector3 FeelerB = new Vector3( 0,0,0 );
	
	
	
	
	public string ___________________________________________ = ""; //dividor 
	
	// Use this for initialization
	protected void Start () {		
		ani =  GetComponent<Animation>() as Animation;
		if ( ani )
		{
		ani["RelaxedWalk"].wrapMode = WrapMode.Loop;
		if ( ani ) ani.CrossFade( "RelaxedWalk" );
		ani["RelaxedWalk"].speed = 1;
		GetComponent<Animation>().wrapMode = WrapMode.Loop;
		}
		
		TargetWorldPosition = transform.position;
		TargetWorldRotation = transform.rotation;
		TemporaryWorldPosition = TargetWorldPosition;
		TemporaryWorldRotation = TargetWorldRotation;
		
	}
	
	public void Update( )
	{
		//Check if we are obstructed
		if (Target != null  && ObstructionAvoidance )
		{
			ObstructionTimer -= Time.deltaTime;
			if ( ObstructionTimer < 0 )
			{
				if ( Target.GetComponent<Collider>() != null ) //check for null 
				{
					if(CanSeeTarget( ) ) Obstructed = false; 
					else 
					{
						Obstructed = true;
						AvoidObstruction( );
					}
					ObstructionTimer = 0.5f;
				}
			}
		}	
		
	}
	
	protected void LateUpdate( )
	{
        GetDistanceToTarget( );
	}
	
	void AvoidObstruction( )
	{
		if (Target)
		{
			RaycastHit hitinfo;
			FeelerA = transform.position;
			bool result = Physics.Linecast( FeelerA, TargetWorldPosition, out hitinfo) ;			
			if ( result && (hitinfo.transform != null) && ( hitinfo.transform != transform) )
			{								
				TemporaryWorldPosition = Vector3.Lerp( transform.position + hitinfo.normal, hitinfo.point + hitinfo.normal, 0.9f );
				//TemporaryWorldRotation = Quaternion.LookRotation( hitinfo.normal  );
				//print ( hitinfo.normal );
			}
            /*
			else
			{						
				FeelerB = transform.position - transform.right * FeelerWidth;
				bool result2 = Physics.Raycast( FeelerB, Target.position - transform.right, out hitinfo, 1000f) ;
				if ( result2  && (hitinfo.transform != null) && (hitinfo.transform != Target.transform) && ( hitinfo.transform != transform))
				{				
					TemporaryWorldPosition = Vector3.Lerp( transform.position, hitinfo.point + transform.right , 0.9f );
					TemporaryWorldRotation = Quaternion.LookRotation( (hitinfo.point + transform.right )- transform.position  );
				}
			}
			*/

        }

    }
	
	//draw our agent gizmo
	public void OnDrawGizmosSelected() {		
		
			Gizmos.color = new Color( 0.7f, 0.7f,0.7f, 0.7f );
			Gizmos.DrawLine( transform.position, transform.position + transform.forward*TriggerDistance  );
		
		
			
			if (DistanceToTarget < TriggerDistance ) Gizmos.color = new Color(1.0f,0.1f,0.1f, 0.7f );
			else Gizmos.color = new Color(0.1f,0.1f,0.9f, 0.7f );
			DrawCircleGizmo( transform.position, TriggerDistance );		
		
		if ( Obstructed ) 
		{
			Gizmos.DrawLine( transform.position, TemporaryWorldPosition );
			Gizmos.DrawWireCube( TemporaryWorldPosition, new Vector3( 0.1f, 2.1f, 0.1f ) );
		}
		if (Target)
		{
			Gizmos.DrawLine( FeelerA, Target.position );
			Gizmos.DrawLine( FeelerB, Target.position );
		}
    }
	
	protected void DrawCircleGizmo( Vector3 pos, float radius )
	{
		for ( int i=0; i< 360; i+=10 )
		{
			float itor = i * Mathf.PI / 180.0f;
			float intor = (i+10) * Mathf.PI / 180.0f;
			Gizmos.DrawLine( pos+new Vector3( Mathf.Sin( itor ) * radius, 0 , Mathf.Cos( itor )*radius), pos+new Vector3( Mathf.Sin( intor )*radius, 0 , Mathf.Cos( intor )*radius));
		}
	}
	
	protected void DrawPieGizmo( Vector3 pos, float radius, float start, float end )
	{
		for ( float i=start; i< end; i+=10 )
		{
			
			float itor = i * Mathf.PI / 180.0f;
			float intor = (i+10) * Mathf.PI / 180.0f;
			Vector3 t = pos+new Vector3( Mathf.Sin( itor ) * radius, 0 , Mathf.Cos( itor )*radius);
			Gizmos.DrawLine( pos+new Vector3( Mathf.Sin( itor ) * radius, 0 , Mathf.Cos( itor )*radius), pos+new Vector3( Mathf.Sin( intor )*radius, 0 , Mathf.Cos( intor )*radius));
			if ( i == start ) Gizmos.DrawLine( pos, t ); 
			if ( i == end -1 ) Gizmos.DrawLine( pos, t ); 
		}
	}
	
	
	
	protected float GetDistanceToTarget( )
	{
		if (Target)
		{
            DistanceToTarget = Vector3.Distance(Target.transform.position, transform.position);			
		} 
		return DistanceToTarget;
	}
	protected float GetDistanceToTargetPosition( )
	{
		float dtp = 0;
		if (Target)
		{
			dtp = Vector3.Distance(Target.transform.position, transform.position);
		} 
		return dtp;
	}

    /**
	 * Check if I am looking at the Target with a tolerance of +- 2 degrees
	 * */
    protected bool LookingAtTarget( )
	{
		Quaternion dest;
		if ( Obstructed ) dest = TemporaryWorldRotation; else dest = TargetWorldRotation; 
		float Dot = Quaternion.Dot ( dest, transform.rotation );
		if ( Mathf.Abs(Dot) > 0.999 ) return true;		
		return false;
	}

    /**
	 * Cast a ray to the Target and check that it is the first hit
	 * */
    protected bool CanSeeTarget( )
	{
		if (Target)
		{
			RaycastHit hitinfo;
			bool result = Physics.Raycast( transform.position, Target.position - transform.position, out hitinfo, TriggerDistance * 10) ;
			if ( result )
			{
				if ( hitinfo.transform == Target.transform )
				{
					return true;
				}
			}
		}
		return false;
	}

    /**
	 * Is the Target in the field of view
	 * */
    protected bool IsTargetInFieldOfView( float FieldOfView )
	{
		if (Target == null ) return false;
		float angle = Vector3.Angle(Target.position - transform.position, transform.forward );				
		if ( angle < FieldOfView ) return true;
		return false;
	}
	
	protected void UpdatePositionAndRotation( )
	{		
		if (( TurnFirst ) && ( !LookingAtTarget() ) )
		{
			UpdateRotation();
		}
		else		
		{
			UpdateRotation();			
			//transform.position = Vector3.Lerp( transform.position, TargetWorldPosition , Speed*Time.deltaTime );
			if ( MaintainHeight ) 
			{
				TargetWorldPosition.y = transform.position.y; //don't affect height
				TemporaryWorldPosition.y = transform.position.y;
			}
			
			UpdatePosition( );			
		}
	}
	
	protected void UpdatePosition( )
	{		
		
		Vector3 dest ;
		if ( ObstructionAvoidance && Obstructed ) dest = TemporaryWorldPosition;
		else dest = TargetWorldPosition;
		
			float delta = Speed * Time.deltaTime;			
			if ( ForwardSmooth && DistanceToTarget > TriggerDistance ) 
			{
				transform.position += transform.forward * delta; 
			}
			else
			{
				if ( ConstantSpeed )
				{
					transform.position = Vector3.MoveTowards( transform.position, dest, Speed*Time.deltaTime );							
				}
				else
				{				
					transform.position = Vector3.Lerp( transform.position, dest , delta );
				}
		}
		
		
	}
	
	protected void UpdateRotation( ) 
	{		
		Quaternion dest;
		if ( ObstructionAvoidance && Obstructed ) dest = TemporaryWorldRotation ; else dest = TargetWorldRotation;
		
		if ( ConstantSpeed ) transform.rotation = Quaternion.RotateTowards( transform.rotation, dest, TurnSpeed * Time.deltaTime );
		else
		transform.rotation = Quaternion.Slerp(transform.rotation, dest, TurnSpeed * Time.deltaTime * 2);		
		
		/*
		Vector3 lerpedRotation = new Vector3(0,0,0 );
		lerpedRotation.Set ( 
			Mathf.Lerp( transform.rotation.eulerAngles.x, TargetWorldRotation.eulerAngles.x, Time.deltaTime * 2), 
			Mathf.Lerp( transform.rotation.eulerAngles.y, TargetWorldRotation.eulerAngles.y, Time.deltaTime * 2), 
			Mathf.Lerp( transform.rotation.eulerAngles.z, TargetWorldRotation.eulerAngles.z, Time.deltaTime * 2));
		
		transform.rotation = Quaternion.Euler( lerpedRotation );
		*/
			
		
	}
	
	/*
	public void UpdateAnimation () 
	{
		
		if ( ani )
			{	
			Debug.Log ( "Agent Update" );
				//Vector3 dist = transform.position - Target.position;
				
				if ( IdleClip && ( Vector3.Distance( transform.position, TargetWorldPosition ) < MaxIdleDistance) )
				{
					ani.CrossFade( IdleClip.name );
				}
				else			
				if ( ForwardClip )
				{
					ani.CrossFade(ForwardClip.name);				
				}			
			}
	}
	*/
}
