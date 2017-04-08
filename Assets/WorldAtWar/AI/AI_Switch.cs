using UnityEngine;
using System.Collections;

public class AI_Switch : MonoBehaviour {		
	
	public int ID = 1;
	
	public bool On = false;
	public bool Toggle = true;
	public bool Clickable = true;
	public bool Collidable = true;
	public bool Reset = false;
	public float ResetTime = 5;
	public float ResetCounter = 0;
	
	bool PreviousOn = false;
	public MonoBehaviour[] Targets;	
	
	public Material OffMaterial;
	public Material OnMaterial;
	
	
	// Use this for initialization
	void Start () {
		UpdateLinks( );
	}
	
	void OnDrawGizmosSelected( )
	{
		foreach( MonoBehaviour target in Targets )
		{
			Gizmos.color = Color.red;
			Gizmos.DrawLine( transform.position, target.transform.position );
		}
	}
	
	
	void OnMouseDown( )
	{
		if ( Clickable ) ActivateSwitch( );
	}
	
	void OnTriggerEnter( Collider other )
	{
		if ( Collidable ) ActivateSwitch( );
	}
	
	public void ActivateSwitch( )
	{
		if ( Toggle )
		{
			if ( On == false) On = true; else On = false;
		}
		else
		{
			On = true;			
		}
	}
	
	public void Switch( bool b )
	{
		On = b;
	}
	
	void UpdateLinks( )
	{
		foreach( MonoBehaviour target in Targets )
		{
			if ( target is AI_Door )
			{
				AI_Door door = target as AI_Door ;
				door.Switch( On );
			}
			
			if ( target is AI_Light )
			{
				AI_Light light = target as AI_Light ;
				light.Switch( On );
			}
			if ( target is AI_Switch )
			{
				AI_Switch theswitch = target as AI_Switch ;
				theswitch.Switch( On );
			}
			
		}
		if ( (On == false) && OffMaterial ) GetComponent<Renderer>().material = OffMaterial;
		if ( (On == true ) && OnMaterial ) GetComponent<Renderer>().material = OnMaterial;
	}
	
	// Update is called once per frame
	void Update () {
		if( On != PreviousOn ) UpdateLinks( );
		PreviousOn = On;
		
		if ( On && Reset )
		{
			ResetCounter += Time.deltaTime;
			if ( ResetCounter >= ResetTime ) 
			{
				Switch ( false );
				ResetCounter = 0;
			}
		}
	}
}
