using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildMan : MonoBehaviour {

  float TimeLeft = 5;
  public GameObject Marker1;
  public GameObject Mine;
  public GameObject Barracks;
  public GameObject WarFactory;
  public GameObject Assault;
  public GameObject Gunner;
  public GameObject Jeep;
  public GameObject Tank;
  
  
  public void MarkerAt(Transform parent, Vector3 v)
  {
    GameObject go = Instantiate(Marker1);
    go.transform.parent = parent;
    go.transform.position = v;
  }

  public void BuildAssault()
  {
    GameObject go = Instantiate(Assault);    
    SendMessage("PlaceMe", go);
  }

  public void BuildGunner()
  {
    GameObject go = Instantiate(Gunner);
    SendMessage("PlaceMe", go);
  }

  public void BuildJeep()
  {
    GameObject go = Instantiate(Jeep);
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
