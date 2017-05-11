using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BuildingData
{
  [SerializeField]
  public float PowerRequired = 1;
}

public class Building : MonoBehaviour {

  public BuildingData Data = new BuildingData();

 public void DoPlace()
  {

  }

  public void DoSelect()
  {

  }

  public void DoMove(Vector3 v)
  {
    this.transform.position = v;
  }

  public void DoSetTarget( Vector3 v)
  {
    this.transform.position = v;
  }

  // Use this for initialization
  void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
