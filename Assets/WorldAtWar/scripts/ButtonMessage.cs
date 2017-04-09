using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonMessage : MonoBehaviour {

  public string Message = "OK";
	
  void OnMouseDown()
  {
    Debug.Log("Pressed");
    SendMessageUpwards("ButtonPress", Message);
  }

  // Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
