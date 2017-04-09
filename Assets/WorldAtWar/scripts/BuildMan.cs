using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildMan : MonoBehaviour {

  float TimeLeft = 5;
  public GameObject Mine;
  public GameObject Barracks;
  public GameObject WarFactory;
  public GameObject Assault;
  public GameObject Tank;

  public void BuildAssault()
  {
    GameObject go = Instantiate(Assault);    
    SendMessage("PlaceMe", go);
  }

  public void BuildTank()
  {
    GameObject go = Instantiate(Tank);    
    SendMessage("PlaceMe", go);
  }

  public void BuildBarracks()
  {
    GameObject go = Instantiate(Barracks);    
    SendMessage("PlaceMe", go);
    SendMessage("SelectMe", go.GetComponent<Selectable>());
  }

  public void BuildWarFactory()
  {
    GameObject go = Instantiate(WarFactory);
    SendMessage("PlaceMe", go);
  }

  public void BuildMine()
  {
    GameObject go = Instantiate(Mine);    
    SendMessage("PlaceMe", go);
  }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
