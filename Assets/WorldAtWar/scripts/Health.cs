using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {

  public float HealthValue = 1; //0 = death, 1 = full health

  void OnCollisionEnter(Collision c)
  {    
    float force = c.relativeVelocity.magnitude;
    Debug.Log("Col!" + force);
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
  

  void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
