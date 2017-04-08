using UnityEngine;
using System.Collections;

public class Controller_Vehicle : MonoBehaviour {

	public float Steering = 0;
	public float SteerMax = 45;
	public float Accelerator = 0;
	public float Power = 20;
	
	public WheelCollider FR_Collider;
	public Transform FR_Wheel;
	public WheelCollider FL_Collider;
	public Transform FL_Wheel;
	
	public WheelCollider BR_Collider;
	public Transform BR_Wheel;
	public WheelCollider BL_Collider;
	public Transform BL_Wheel;
	
	public float RotationRatio = 6;
	public float Rotation = 0;
	private float TurnRatioR  = 0.6f;
	private float TurnRatioL  = 0.6f;
	
	
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void LateUpdate () {
		
		Steering += Input.GetAxis( "Horizontal" );
		if ( Steering > SteerMax ) Steering = SteerMax;
		if ( Steering < -SteerMax ) Steering = -SteerMax;
		Accelerator = Input.GetAxis( "Vertical" );
		
		FR_Collider.motorTorque = Accelerator * Power;
		FL_Collider.motorTorque = Accelerator * Power;
		
		FR_Collider.steerAngle = Steering;
		FL_Collider.steerAngle = Steering;
		Rotation += (FR_Collider.rpm * RotationRatio) * Time.deltaTime;		
		
		if ( Steering > 0 ) 
		{
			TurnRatioL = 1;
			TurnRatioR = 0.6f;
		}
		else{
			TurnRatioL = 0.6f;
			TurnRatioR = 1;
		}
		
		FR_Wheel.localEulerAngles = new Vector3( Rotation , FR_Collider.steerAngle * TurnRatioR , 90 );
		FL_Wheel.localEulerAngles = new Vector3( Rotation , FL_Collider.steerAngle * TurnRatioL , 90 );
		
		BR_Wheel.localEulerAngles = new Vector3( Rotation , BR_Collider.steerAngle , 90 );
		BL_Wheel.localEulerAngles = new Vector3( Rotation , BL_Collider.steerAngle , 90 );
	}
}
