using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

  public Vector3 Offset = new Vector3(50, 80, -20);
  public Vector3 TargetPosition;
  public Vector3 TargetLookAt;
  // Use this for initialization
  public Vector3 CurrentVelocity;
  public float Speed = 10;
  public float RotationSpeed = 10;

  public void SetLookAt( Vector3 v)
  {
    TargetLookAt = v;
     TargetPosition = v + Offset;
    //this.transform.LookAt(v);    
    
    
  }
  void Start () {
    TargetPosition = this.transform.position;
  }
	
	// Update is called once per frame
	void Update () {
    transform.position = Vector3.SmoothDamp(transform.position, TargetPosition, ref CurrentVelocity, Speed);

    Vector3 _direction = (TargetLookAt - transform.position).normalized;
    Quaternion _lookRotation = Quaternion.LookRotation(_direction);
    transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * RotationSpeed);
  }
}
