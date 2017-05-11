using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
  public float DamagePerImpact = 0.1f; //assault life  = 1, Tank = 10
  public Vector3 Power = new Vector3(0, 0, 100);
  public GameObject MyExplosion;

  public void Fire(Vector3 v)
  {
    GetComponent<Rigidbody>().AddRelativeForce(Power);
  }

  public void OnCollisionEnter(Collision c)
  {
    //Selectable s = c.gameObject;    
    //Debug.Log(c);
    Instantiate(MyExplosion, transform.position, Quaternion.identity);
    GameObject.Destroy(this.gameObject, 0.5f);
  }

  // Use this for initialization
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {

  }
}
