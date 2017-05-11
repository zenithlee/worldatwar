using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour {

  public float MiningRate = 10;
  public float MiningCounter = 10;
  public float Lithim = 0;

	// Use this for initialization
	void Start () {
		
	}

  void CheckMine()
  {
    MiningCounter -= Time.deltaTime;
    if ( MiningCounter < 0 )
    {
      MiningCounter = MiningRate;
      Lithim++;
    }
  }
	
	// Update is called once per frame
	void Update () {
    CheckMine();
	}
}
