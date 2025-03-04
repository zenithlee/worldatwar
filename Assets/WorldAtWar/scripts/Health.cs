﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {

  public float HealthValue = 1; //0 = death, 1 = full health
  public float InitialHealth = 1;

  void OnCollisionEnter(Collision c)
  {    
    float force = c.relativeVelocity.magnitude;
    //Debug.Log("Col!" + force);
    Projectile p = c.transform.GetComponent<Projectile>();
    if ( p )
    {
      HealthValue -= p.DamagePerImpact;
      if (HealthValue <= 0)
      {
        SendMessage("DoDie");
      }
    }
  }

  void CheckLife()
  {
    if (HealthValue <= 0)
    {
      SendMessage("DoDie");
    }
  }
  

  void Start () {
    InitialHealth = HealthValue;
    InvokeRepeating("CheckLife", 1, 1);
  }
	
	// Update is called once per frame
	void Update () {
   
  }
}
