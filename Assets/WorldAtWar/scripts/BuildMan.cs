using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BuildMan : MonoBehaviour {

  float TimeLeft = 5;
  public Team MyTeam;
  public GameObject MyArmy;
  public LimitsMan Limits;
  public GameObject GameTerrain;

  public void Build(Types.ConstructionTypes type)
  {

  }

  public void BuildBase()
  {    
    if ( Limits.CanIBuild( Types.ConstructionTypes.Base)) { 
      GameObject go = Instantiate(MyTeam.BaseProto);
      SendMessage("PlaceMe", go);
    }
  }

  public void MarkerAt(Transform parent, Vector3 v)
  {
    GameObject go = Instantiate(MyTeam.Marker1Proto);
    go.transform.parent = parent;
    go.transform.position = v;
  }

  public void BuildAssault()
  {
    GameObject go = Instantiate(MyTeam.AssaultProto);    
    SendMessage("PlaceMe", go);
  }

  public void BuildGunner()
  {
    GameObject go = Instantiate(MyTeam.GunnerProto);
    SendMessage("PlaceMe", go);
  }

  public void BuildSniper()
  {
    GameObject go = Instantiate(MyTeam.SniperProto);
    SendMessage("PlaceMe", go);
  }

  public void BuildEngineer()
  {
    GameObject go = Instantiate(MyTeam.EngineerProto);
    SendMessage("PlaceMe", go);
  }

  public void BuildJeep()
  {
    GameObject go = Instantiate(MyTeam.JeepProto);
    SendMessage("PlaceMe", go);
  }

  public void BuildPowerStation()
  {
    GameObject go = Instantiate(MyTeam.PowerStationProto);
    SendMessage("PlaceMe", go);
  }

  public void BuildWall()
  {
    GameObject go = Instantiate(MyTeam.WallProto);
    SendMessage("PlaceMe", go);
  }

  public void BuildTank()
  {
    GameObject go = Instantiate(MyTeam.TankProto);    
    SendMessage("PlaceMe", go);
  }

  public void BuildBarracks()
  {
    GameObject go = Instantiate(MyTeam.BarracksProto);    
    SendMessage("PlaceMe", go);
    SendMessage("SelectMe", go.GetComponent<Selectable>());
  }

  public void BuildWarFactory()
  {
    GameObject go = Instantiate(MyTeam.VehicleFactoryProto);
    SendMessage("PlaceMe", go);
  }

  public void BuildMine()
  {
    GameObject go = Instantiate(MyTeam.MineProto);    
    SendMessage("PlaceMe", go);
  }

  public void BuildContainer()
  {
    GameObject go = Instantiate(MyTeam.StorageProto);
    SendMessage("PlaceMe", go);
  }

  void PlaceMe(GameObject go)
  {
    go.transform.parent = MyArmy.transform;

    RaycastHit hit;

    Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2.5f, Screen.height / 2, 0));
    if (GameTerrain.GetComponent<Collider>().Raycast(ray, out hit, Mathf.Infinity))
    {
      NavMeshAgent nm = go.GetComponent<NavMeshAgent>();
      if (nm != null)
      {
        float r = go.GetComponent<Selectable>().GetRadius() * 5;
        Vector3 pos = Tools.GetSnap(go, hit.point + new Vector3(Random.value * r, 0, Random.value * r));
        go.GetComponent<NavMeshAgent>().Warp(pos);
      }
      else
      {
        go.GetComponent<Selectable>().IsPlacing = true;
        float r = go.GetComponent<Selectable>().GetRadius() * 2;
        Vector3 pos = Tools.GetSnap(go, hit.point + new Vector3(Random.value * r, 0, Random.value * r));
        go.transform.position = pos;
      }

    }
  }

  // Use this for initialization
  void Start () {
    MyArmy = GetComponent<Game>().MyArmy;
    MyTeam = MyArmy.GetComponent<Team>();
    Limits = MyTeam.GetComponent<LimitsMan>();
    GameTerrain = GetComponent<Game>().GameTerrain;
  }
	
	// Update is called once per frame
	void Update () {
		
	}
}
