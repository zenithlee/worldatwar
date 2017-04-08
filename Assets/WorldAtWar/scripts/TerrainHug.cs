using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainHug : MonoBehaviour {

  public Transform backLeft;
  public Transform backRight;
  public Transform frontLeft;
  public Transform frontRight;
  public RaycastHit lr;
  public RaycastHit rr;
  public RaycastHit lf;
  public RaycastHit rf;
  public Vector3 upDir;
  void Update2()
  {
    transform.rotation = transform.parent.rotation;
    Physics.Raycast(backLeft.position + Vector3.up, Vector3.down, out lr);
    Physics.Raycast(backRight.position + Vector3.up, Vector3.down, out rr);
    Physics.Raycast(frontLeft.position + Vector3.up, Vector3.down, out lf);
    Physics.Raycast(frontRight.position + Vector3.up, Vector3.down, out rf);
    upDir = (Vector3.Cross(rr.point - Vector3.up, lr.point - Vector3.up) +
             Vector3.Cross(lr.point - Vector3.up, lf.point - Vector3.up) +
             Vector3.Cross(lf.point - Vector3.up, rf.point - Vector3.up) +
             Vector3.Cross(rf.point - Vector3.up, rr.point - Vector3.up)
            ).normalized;
    Debug.DrawRay(rr.point, Vector3.up);
    Debug.DrawRay(lr.point, Vector3.up);
    Debug.DrawRay(lf.point, Vector3.up);
    Debug.DrawRay(rf.point, Vector3.up);
    
    transform.up = new Vector3(upDir.x, upDir.y, upDir.z);    
   // transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x,transform.parent.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);

    
    //transform.rotation = 
  }
}
