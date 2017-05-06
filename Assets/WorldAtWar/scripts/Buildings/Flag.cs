using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WW { 

public class Flag : MonoBehaviour {

	// Use this for initialization
	void Start () {
    Selectable sel = GetComponent<Selectable>();
      Transform t = transform.Find(sel.Data.Team.ToString());
      t.gameObject.SetActive(true);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

}