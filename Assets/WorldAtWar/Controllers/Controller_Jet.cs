using UnityEngine;
using System.Collections;


/**
 * Non-realistic Jet controller
 * with takeoff assist
 * 
 * 
 * 
 * */

public class Controller_Jet : MonoBehaviour {
		
	public GameObject Missile;
	public Vector3 MissileOffset = new Vector3( 0, 0, 10 );
	public GameObject MissileExplosion;
	
	public AudioClip EngineSound;
	public AudioSource StartupSound;
	
	public WheelCollider FrontWheel;
	
	public ParticleSystem JetParticles;
	
	public float Mass = 1200;
	public float Length = 1;	
	public float Throttle = 0;
	float EngineForce = 1000;	
	float Aeleron = 0;
	public float Speed = 0;	
	float Elevator = 0;
	float ElevatorTrim = -20;
	float ElevatorMultiplier = 0.01f;
	float AeleronMultiplier = 0.05f;
	float Lift = 0;
	float Stabilizer = 0.01f;
	
//	float ailerons = 0.0f;	
	//float elevatorCenterSetting = 0.5f;
	
	Vector3 LocalVelocity;
	
	Vector3 OriginalPosition;
	Quaternion OriginalRotation;
	
	Vector3 drag = new Vector3(2.0f,8.0f,0.05f);
//	Vector3 stabilizingDrag = new Vector3(2.0f,1.0f,0.0f);
	float brakeDrag = 0.0f;
	float brake = 0.0f; 
	
	float Height = 0;
	
	AudioSource Engine;
	
	public bool ShowGUI = false;
	
	// Use this for initialization
	void Start () {
		OriginalPosition = transform.position;
		OriginalRotation = transform.rotation;
		GetComponent<Rigidbody>().mass = Mass;
		GetComponent<Rigidbody>().angularDrag = 0.5f;
		
		Engine = GetComponent<AudioSource>( );
	}
	
	void StartEngine( )
	{		
		if ( StartupSound ) StartupSound.Play();
	}
	
	void FixedUpdate( )
	{
		LocalVelocity = transform.InverseTransformDirection( GetComponent<Rigidbody>().velocity );
		Speed  = LocalVelocity.magnitude;
		
		GetHeight( );
		
		UpdateGroundDriving( );
		
		UpdateForces( );
		if (( Input.GetAxis( "Vertical" ) == 0 ) && ( Input.GetAxis( "Horizontal") ==0 )) Stabilize() ;		
		
		UpdateSound( );
		UpdateParticles( );
		
		Aeleron *=0.98f;
		Elevator *= 0.98f;
	}
		
	// Update is called once per frame
	void Update () {
	
		
		Elevator += Input.GetAxis( "Vertical" ) * ElevatorMultiplier ;
		Aeleron += Input.GetAxis( "Horizontal" ) * AeleronMultiplier;
		
		
		
		if ( Input.GetKey( KeyCode.PageUp ) ) Throttle+=0.5f;
		if ( Input.GetKey( KeyCode.PageDown ) )Throttle--;		
		Throttle = Mathf.Clamp( Throttle, 0, 50 );
		if ( Input.GetKey( KeyCode.LeftShift )) Throttle= 70;
		
		if ( Input.GetKeyDown( KeyCode.O ) ) Lift++;
		if ( Input.GetKeyDown( KeyCode.K ) )Lift--;
		if ( Input.GetKeyDown( KeyCode.Space )  ) FireMissile( );
		if ( Input.GetKeyDown( KeyCode.Escape )  ) Reset( );
		if ( Input.GetKeyDown( KeyCode.Alpha1 )  ) DebugF( );
		
		
		
	}
	
	void UpdateGroundDriving( )
	{
		if ( Height < 10 ) FrontWheel.steerAngle = Aeleron * 100;
	}
	
	void UpdateParticles( )
	{
		if ( JetParticles )
		{
			//if ( Speed < 5 ) JetParticles.enableEmission = false; else JetParticles.enableEmission = true;
			JetParticles.startSize = Throttle / 3;
			
			JetParticles.startSpeed = Throttle / 2;
		}
	}
	
	void UpdateSound( )
	{	
		if ( Engine )
		{
			Engine.pitch =  0.5f + (Speed * 0.03f);
			if ( Engine.pitch > 5 ) Engine.pitch = 5;
		}
	}
	
	void DebugF( )
	{
		GetComponent<Rigidbody>().MovePosition( new Vector3( OriginalPosition.x,150, OriginalRotation.z ));
		GetComponent<Rigidbody>().MoveRotation( OriginalRotation );
	}
	
	void Reset( )
	{
		GetComponent<Rigidbody>().MovePosition( OriginalPosition );
		GetComponent<Rigidbody>().MoveRotation( OriginalRotation );
		GetComponent<Rigidbody>().velocity = Vector3.zero;
		GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
		Throttle = 0;
		Elevator = 0;
		Aeleron = 0;
	}
	
	void OnGUI( )
	{
		if ( !ShowGUI ) return;
		float xx = 10;
		float yy = 10;
		GUI.TextField ( new Rect( xx, yy, 200, 32 ),  "Velocity: " + GetComponent<Rigidbody>().velocity );
		yy += 32;
		GUI.TextField ( new Rect( xx, yy, 200, 32 ),  "LocVelocity: " + LocalVelocity );
		yy += 32;
		GUI.TextField ( new Rect( xx, yy, 200, 32 ),  "LocRotation: " + transform.localRotation );
		yy += 32;
		GUI.TextField( new Rect( xx, yy, 200, 32 ), "Speed: " + Speed );
		yy += 32;
		GUI.TextField( new Rect( xx, yy, 200, 32 ), "Throttle: " + Throttle );
		yy += 32;
		GUI.TextField( new Rect( xx, yy, 200, 32 ), "Aeleron: " + Aeleron );
		yy += 32;
		GUI.TextField( new Rect( xx, yy, 200, 32 ), "Elevator: " + Elevator );
		yy += 32;
		GUI.TextField( new Rect( xx, yy, 200, 32 ), "ElevatorTrim: " + ElevatorTrim );
		yy += 32;
		GUI.TextField( new Rect( xx, yy, 200, 32 ), "Up: " + transform.up );
		yy += 32;
		GUI.TextField( new Rect( xx, yy, 200, 32 ), "Lift: " + Lift );
		yy += 32;
		GUI.TextField( new Rect( xx, yy, 200, 32 ), "Alt: " + Height );
		
		yy += 32;
		GUI.TextField( new Rect( xx, yy, 200, 32 ), "ElevatorTrim: " + ElevatorTrim );
		yy+=32;
		ElevatorTrim = GUI.HorizontalSlider( new Rect( xx,yy ,200, 32), ElevatorTrim, -400f, 400f );
		
		yy += 32;
		GUI.TextField( new Rect( xx, yy, 200, 32 ), "Arrows, P/L, Space, Esc " );
		
	}
	
	void Stabilize( )
	{
		//stabilizer		
		//print ( transform.right.y );
		GetComponent<Rigidbody>().rotation *= Quaternion.Euler( transform.forward.y*Speed * Stabilizer, 0, -transform.right.y * Speed * Stabilizer );
	}
	
	void GetHeight( )
	{
		RaycastHit hit = new RaycastHit( );
		Physics.Raycast( transform.position, -Vector3.up, out hit );
		Height = hit.distance;
		
	} 
	
	void FireMissile( )
	{
		MissileOffset.x = -MissileOffset.x;
		GameObject go = Instantiate( Missile, transform.position + transform.rotation * MissileOffset,transform.rotation  ) as GameObject;
		AI_Missile miss = go.GetComponent<AI_Missile>() as AI_Missile ;
		if ( miss ) 
		{
			miss.Owner = this.GetComponent<AI_Agent>();		
			if ( MissileExplosion ) miss.Explosion = MissileExplosion;
			miss.Fire( 12000) ;
		}		
	}
	
	//quick game physics
	void UpdateForces( )
	{
		//rotation
		Quaternion AddRot = Quaternion.identity;        
        float yaw = 0;        		
        AddRot.eulerAngles = new Vector3(Elevator, yaw, -Aeleron) * Time.deltaTime * Speed;
        GetComponent<Rigidbody>().rotation *= AddRot;	
		GetComponent<Rigidbody>().rotation *= Quaternion.Euler( 0 , -transform.right.y, 0 );
		
		//translation in direction
		Vector3 AddPos = Vector3.forward;
        AddPos = GetComponent<Rigidbody>().rotation * AddPos * Throttle * 0.5f;
		
		//lift
		float IHeight = 100-Height;  //air density up to 100 units
		if ( IHeight < 1 ) IHeight = 1;
		if (( Speed > 20 ) && ( Height < 100 ) ) AddPos += transform.up * -ElevatorTrim * (IHeight * 0.008f) * Speed *0.02f;;
				
        GetComponent<Rigidbody>().velocity += AddPos * Time.deltaTime ;
		
		
	}
	
	//more realistic, but harder to control
	void UpdateForces2( )
	{		
		GetComponent<Rigidbody>().AddRelativeForce( Vector3.forward * EngineForce * Throttle  );
		//rigidbody.AddRelativeTorque( new Vector3( Aeleron *0.5f, 0, Steer * 0.01f ) );
	    
		float forwardVelo = Vector3.Dot(GetComponent<Rigidbody>().velocity,transform.forward);
	    float sqrVelo = forwardVelo*forwardVelo;
		
		Vector3 dragDirection = transform.InverseTransformDirection(GetComponent<Rigidbody>().velocity);
	    Vector3 dragAndBrake = drag+new Vector3(0,0,brakeDrag*brake);
	    Vector3 dragForces = -Vector3.Scale(dragDirection,dragAndBrake)*GetComponent<Rigidbody>().velocity.magnitude;
	    GetComponent<Rigidbody>().AddForce(transform.TransformDirection(dragForces));
		
		
		//Automatic flaps used for takeoff assist		
		float ElevatorOffset = 0; 
		Lift = Speed ;
		if (( Speed > 1 ) && ( Speed < 40 )) 
		{
			
			ElevatorOffset = ElevatorTrim;
		}
		//else
		{
			//Lift = Speed * 0.5f;			
		}
		GetComponent<Rigidbody>().AddForce( transform.up * Lift * 800 );
		
		 //stabilization (to keep the plane facing into the direction it's moving)

		
		
    	//Vector3 stabilizationForces = -Vector3.Scale(dragDirection,stabilizingDrag)*rigidbody.velocity.magnitude;
    	//rigidbody.AddForceAtPosition(transform.TransformDirection(stabilizationForces),transform.position-transform.forward*Length);
    	//rigidbody.AddForceAtPosition(-transform.TransformDirection(stabilizationForces),transform.position+transform.forward*Length);           	 
    	
    //elevator
    	GetComponent<Rigidbody>().AddRelativeTorque(Vector3.right* (Elevator+ElevatorOffset));   		
		print ( transform.localEulerAngles.z );
		
    //ailerons

    //if(!grounded)
        //rigidbody.AddTorque(-transform.forward*sqrVelo*Elevator); 
		GetComponent<Rigidbody>().AddRelativeTorque(-Vector3.forward*sqrVelo*Aeleron); 
		GetComponent<Rigidbody>().AddRelativeForce( Vector3.right * Aeleron );
    
		//strive to be level
		//Vector3 Level = transform.rotation * transform.forward * 0.5f ;
		//rigidbody.AddForce( Level );
		
	}
}
