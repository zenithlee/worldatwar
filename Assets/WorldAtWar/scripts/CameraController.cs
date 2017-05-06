using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
  
  public Vector3 TargetPosition;
  public Vector3 TargetLookAt;
  // Use this for initialization
  public Vector3 CurrentVelocity;
  public float Speed = 10;
  public float RotationSpeed = 10;

  public bool BoundsActive = false;
  public Vector3 BoundsMin;
  public Vector3 BoundsMax;

  void CheckBounds()
  {
    if (BoundsActive == false) return;
    if (TargetPosition.x > BoundsMax.x) TargetPosition.x = BoundsMax.x;
    if (TargetPosition.y > BoundsMax.y) TargetPosition.y = BoundsMax.y;
    if (TargetPosition.z > BoundsMax.z) TargetPosition.z = BoundsMax.z;
    if (TargetPosition.x < BoundsMin.x) TargetPosition.x = BoundsMin.x;
    if (TargetPosition.y < BoundsMin.y) TargetPosition.y = BoundsMin.y;
    if (TargetPosition.z < BoundsMin.z) TargetPosition.z = BoundsMin.z;
  }

  public void ZoomBy( float f )
  {
    GetComponent<Camera>().orthographicSize += f;
  }

  public void SetLookAt( Vector3 v)
  {
    TargetLookAt = v;
     TargetPosition = v;
    //this.transform.LookAt(v);   
    CheckBounds();
  }

  public void PanBy( Vector3 v)
  {
    TargetPosition += v;
    CheckBounds();
  }

  public void MoveTo(Vector3 v )
  {
    TargetPosition = v ;
    CheckBounds();
  }

  void Start () {
    //TargetPosition = this.transform.position;
  }

  void Update()
  {
    transform.position = Vector3.SmoothDamp(transform.position, TargetPosition, ref CurrentVelocity, Speed);
  }
	
	// Update is called once per frame
	void Update2 () {
    transform.position = Vector3.SmoothDamp(transform.position, TargetPosition, ref CurrentVelocity, Speed);

    Vector3 _direction = (TargetLookAt - transform.position).normalized;
    Quaternion _lookRotation = Quaternion.LookRotation(_direction);
    transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * RotationSpeed);
  }
}
