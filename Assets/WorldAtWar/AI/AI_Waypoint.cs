using UnityEngine;
using System.Collections;

/**
 * Waypoint for a Patrol
 * Add this to a GameObject or Transform to allow waiting at each point
 * 
 * */

public class AI_Waypoint : MonoBehaviour {
	// Use this for initialization
	
	public float WaitTime = 0;
	
	void Start () {		
		
	}
	
	void OnDrawGizmos( )
	{
		Gizmos.color = new Color( 0.4f,0.1f,0.4f, 0.7f );
		Gizmos.DrawWireCube( transform.position, new Vector3(0.2f,1f,0.2f) );				
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
