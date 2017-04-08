using UnityEngine;
using System.Collections;

public class AI_Light : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	public void Switch( bool in_on )
	{
		if ( in_on ) TurnOn() ; else TurnOff( );
	}
	
	void TurnOn( )
	{
		GetComponent<Light>().enabled = true;
	}
	
	void TurnOff( )
	{
		GetComponent<Light>().enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
