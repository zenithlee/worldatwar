using UnityEngine;
using System.Collections;

public class AI_Brain : AI_Agent {
	
	public AI_Agent NormalBehaviour ;
	public AI_Agent NearBehaviour;
	public AI_Agent FarBehaviour ;
	
	public AI_Agent CurrentBehaviour;
	// Use this for initialization
	new void Start () {
	
	}
	
	// Update is called once per frame
	new void LateUpdate () {
		//Debug.Log ( NormalBehaviour );
		base.LateUpdate( ); //calls GetDistanceToTarget
		
		//near
		if ( DistanceToTarget < TriggerDistance ) 
		{
			CurrentBehaviour = NearBehaviour;
			NearBehaviour.enabled = true;
			NormalBehaviour.enabled = false;			
			FarBehaviour.enabled = false;
			NearBehaviour.Target = this.Target;
		} //far
		else if(( DistanceToTarget > TriggerDistance * 2 ) && ( FarBehaviour ))
		{
			CurrentBehaviour = FarBehaviour;
			NormalBehaviour.enabled = false;
			NearBehaviour.enabled = false;
			FarBehaviour.enabled = true;
			FarBehaviour.Target = this.Target;
		}
		else //wherever you are
		{			
			CurrentBehaviour = NormalBehaviour;
			NearBehaviour.enabled = false;
			FarBehaviour.enabled = false;
			NormalBehaviour.Target = this.Target;
			NormalBehaviour.enabled = true;
		}
	}
}
