using UnityEngine;
using System.Collections;


/**
 * Simple move controller to test AI 
 * Press arrows to move ( or ASWD )
 * Press E to activate nearby switches, adjust the trigger radius with the TriggerRadius
 * 
 * 
 * */
public class Controller_Basic : MonoBehaviour {
	
	public float NormalSpeed = 1;
	public float ShiftSpeed = 2;	
	private float m_Speed = 1;
	
	public float TriggerRadius = 2;  //when the trigger button is pressed (e) then press all triggers within this radius
	public KeyCode TriggerKey = KeyCode.E;	
	
	void Start () {
	
	}
	
	void OnDrawGizmosSelected( )
	{
		DrawCircleGizmo( transform.position, TriggerRadius );
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
	
	
	void Update () 
	{		
		Transform t = Camera.main.transform;
		Vector3 forward = t.TransformDirection(Vector3.forward);
		forward.y = 0;
		forward = forward.normalized;
		Vector3 right = new Vector3(forward.z, 0, -forward.x);	
		float v = Input.GetAxisRaw("Vertical");
		float h = Input.GetAxisRaw("Horizontal");						
		Vector3 targetDirection = h * right + v * forward;
		if ( Input.GetKey ( KeyCode.LeftShift )) m_Speed = ShiftSpeed; else m_Speed = NormalSpeed; 		
		transform.position += targetDirection * Time.deltaTime * m_Speed;		
		//move if a direction is pressed
		if ( (v!=0) ||  (h !=0)  ) 
		{
			transform.rotation = Quaternion.LookRotation( targetDirection );
			Forward ( );
		}
		
		if ( Input.GetKeyDown( TriggerKey ))
		{
			CheckTriggers();
		}
		
	}
	
	void Forward( )
	{
		Animation ani = GetComponent<Animation>();
		if ( ani )
		{
			ani.CrossFade( "Walk" );
		}
	}
		
	//checks for triggers near me
	//Calls ActivateSwitch on them
	void CheckTriggers( )
	{
		Collider[] objects = Physics.OverlapSphere( transform.position, TriggerRadius );
		foreach( Collider c in objects )
		{			
			AI_Switch sw = c.GetComponent<AI_Switch>( );
			if ( sw )
			{
				sw.ActivateSwitch( );
			}
		}
	}

}
