using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollZoom : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
    float Scroll = Input.GetAxis("Mouse ScrollWheel") * 5;
    if ( Scroll != 0 )
    {
      //this.transform.Translate( new Vector3(0,0,Scroll));
      
      GetComponent<CameraController>().Offset += this.transform.forward * Scroll; 
    }
  }
}
