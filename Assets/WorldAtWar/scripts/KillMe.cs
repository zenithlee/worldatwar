using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillMe : MonoBehaviour {

  public float KillTime = 2;

	// Use this for initialization
	void Start () {
    Invoke("Kill", KillTime);
	}

  void Kill()
  {
    GameObject.Destroy(this.gameObject, 0.1f);
  }
	
	// Update is called once per frame
	void Update () {
		
	}
}
