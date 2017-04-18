using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[Serializable]
public class TeamData {

  [SerializeField]
  public int TeamID = 0;  
} 

public class Team : MonoBehaviour {

  //public int TeamID = 0;
  public TeamData Data = new TeamData();

  public GameObject BaseProto;
  public GameObject BarracksProto;
  public GameObject VehicleFactoryProto;
  public GameObject WallProto;
  public GameObject StorageProto;
  public GameObject MineProto;
  public GameObject PowerStationProto;

  public GameObject AssaultProto;
  public GameObject GunnerProto;
  public GameObject SniperProto;
  public GameObject EngineerProto;

  public GameObject JeepProto;
  public GameObject TankProto;  

  public GameObject Marker1Proto;


  void AddAllToTeam()
  {
    for ( int i=0; i< transform.childCount; i++)
    {
      Transform t = transform.GetChild(i);
      Selectable s = t.GetComponent<Selectable>();
      if ( s != null )
      {
        s.Data.Team = Data.TeamID;
      }
    }
  }
	// Use this for initialization
	void Start () {
    AddAllToTeam();
  }
	
	// Update is called once per frame
	void Update () {
		
	}
}
