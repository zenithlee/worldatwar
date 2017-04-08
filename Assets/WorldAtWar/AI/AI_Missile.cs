using UnityEngine;
using System.Collections;

public class AI_Missile : AI_Agent {
	
	public AI_Agent Owner;
	public GameObject Explosion;
	public float Damage= 20;
	public float TravelForce = 30;
	public float FuelSeconds = 5;
	public bool TrackEnemy = true;
	// Use this for initialization
	new void Start () {		
	}
	
	// Update is called once per frame
	new void Update () {
		base.Update( );
		
	
		if ( Target )
		{
			TargetWorldPosition = Target.position ;
			TargetWorldRotation = Quaternion.LookRotation(Target.position - transform.position);
			if ( transform.GetComponent<Rigidbody>()  && TrackEnemy )
			{	
				Vector3 direction = Target.position - transform.position;
				transform.GetComponent<Rigidbody>().AddForce( direction.normalized * TravelForce , ForceMode.Force );
			}			
			//transform.rigidbody.AddRelativeForce( Vector3.forward * TravelForce , ForceMode.Force );
		}
		UpdateRotation();
		CheckFuel( );
		
	}
	
	void CheckFuel( )
	{
		FuelSeconds -= Time.deltaTime; 
		if ( FuelSeconds < 0 ) Destroy( this.gameObject );
	}
	
	new void OnDrawGizmosSelected( )
	{		
		Gizmos.DrawLine( transform.position, transform.position + transform.forward * TriggerDistance );
		
		Gizmos.color = Color.red;
		Gizmos.DrawLine( transform.position, TargetWorldPosition );
		
		
	}
	
	void OnCollisionEnter( Collision other )
	{
		//print ( "hit with " + other.collider.name );
		AI_Agent ai = other.gameObject.GetComponent<AI_Agent>( );
		if ( ai )
		{
			if (( Owner == null ) || (other.gameObject != Owner.gameObject ))
			{
				ai.Health -= Damage;
				
				if ( ai.Health <= 0 ) Destroy( other.gameObject );
			}
		}
		if ( Explosion ) 
		{
			
			Destroy ( Instantiate( Explosion, transform.position, transform.rotation ), 10 );
		}
		
		Destroy( this.gameObject );		
	}
	
	public void Fire( float ExplosiveForce )
	{		
		
		if ( transform.GetComponent<Rigidbody>() )
		{
			transform.GetComponent<Rigidbody>().AddRelativeForce( Vector3.forward * ExplosiveForce , ForceMode.Acceleration );
		}
	}
		
}
