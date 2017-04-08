using UnityEngine;
using System.Collections;

public class AI_Door : MonoBehaviour {
	
	public float Speed = 2;
	public Vector3 OpenOffset;
	public Vector3 OpenRotation;
	public Vector3 ClosedOffset;
	public Vector3 ClosedRotation;
	
	Vector3 OriginalPosition;
	//Quaternion OriginalRotation;
	
	Vector3 TargetPosition;
	Quaternion TargetRotation; 
	
	// Use this for initialization
	void Start () {
		OriginalPosition = this.transform.position;
		//OriginalRotation = this.transform.rotation;
		TargetPosition = OriginalPosition;
	}
	
	public void Switch( bool in_on )
	{
		if ( in_on ) OpenDoor() ; else CloseDoor( );
	}
	
	void OpenDoor( )
	{
		TargetPosition = OriginalPosition + OpenOffset;
		TargetRotation = Quaternion.Euler(OpenRotation);
	}
	void CloseDoor( )
	{
		TargetPosition = OriginalPosition + ClosedOffset;
		TargetRotation = Quaternion.Euler(ClosedRotation);
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = Vector3.Lerp( transform.position, TargetPosition, Time.deltaTime * Speed );
		transform.rotation = Quaternion.Lerp( transform.rotation, TargetRotation, Time.deltaTime * Speed );
	}
}
