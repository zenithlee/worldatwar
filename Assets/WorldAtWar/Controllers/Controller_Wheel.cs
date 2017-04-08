using UnityEngine;
using System.Collections;

public class Controller_Wheel : MonoBehaviour {
	
	public Transform Wheel;
	WheelCollider c;
	// Use this for initialization
	void Start () {
		c= GetComponent<WheelCollider>();
	}
	
	// Update is called once per frame
	void Update () {		
		Vector3 euler = Wheel.transform.localRotation.eulerAngles;
		euler.Set( 90, euler.y + c.rpm *0.11f, 0 );		
		Wheel.transform.Rotate( 0, c.rpm * 0.1f, 0  );
	}
}
