using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBlip : MonoBehaviour {


  public void Kill()
  {
    GameObject.Destroy(this.gameObject, 0.5f);
  }

	// Use this for initialization
	void Start () {
    
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
