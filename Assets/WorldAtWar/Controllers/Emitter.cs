using UnityEngine;
using System.Collections;

public class Emitter : MonoBehaviour {
	
	public Transform Template;
		
	public float Rate = 0.01f;
	private float Counter = 0;
	public  int amount = 0;
	public int MaxAmount = 500;
	public float Radius = 5;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Counter += Time.deltaTime;
		if ( Counter > Rate ) 
		{
			Emit( );
			Counter = 0;
			//Application.CaptureScreenshot( "caps/screen_" + amount + ".png" ) ;
		}
	}
	
	void Emit( )
	{
		if ( amount >= MaxAmount ) 
		{
			Destroy( transform.GetChild(0).gameObject);
			amount--;			
		}
		amount++;
		Transform o = Instantiate( Template, transform.position + new Vector3( Random.value * Radius , 0, Random.value* Radius ), transform.rotation ) as Transform;
		o.GetComponent<Rigidbody>().AddForce( 0, 0, -100) ;
		o.parent = this.transform;		
	}
}
