using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace WW
{

  [Serializable]
  public class TeamData
  {

    [SerializeField]
    public int Team = 0;
    public float Credits = 0;
  }

  public class Team : MonoBehaviour
  {

    //public int TeamID = 0;
    public TeamData Data = new TeamData();

    public GameObject BaseProto;
    public GameObject BarracksProto;
    public GameObject VehicleFactoryProto;
    public GameObject WallProto;
    public GameObject StorageProto;
    public GameObject MineProto;
    public GameObject PowerStationProto;
    public GameObject ContainerProto;

    public GameObject AssaultProto;
    public GameObject GunnerProto;
    public GameObject SniperProto;
    public GameObject EngineerProto;

    public GameObject JeepProto;
    public GameObject TankProto;

    public GameObject Marker1Proto;
    public GameObject RadarDotBuildingProto;
    public GameObject RadarDotVehicleProto;
    public GameObject RadarDotInfantryProto;


    void AddAllToTeam()
    {
      for (int i = 0; i < transform.childCount; i++)
      {
        Transform t = transform.GetChild(i);
        Selectable s = t.GetComponent<Selectable>();
        if (s != null)
        {
          s.Data.Team = Data.Team;
        }
      }
    }

    public void RepairUnit(Selectable s)
    {
      Health h = s.GetComponent<Health>();
      float cost = s.Data.Cost * ((h.InitialHealth - h.HealthValue) / h.InitialHealth);
      Data.Credits -= cost;
      h.HealthValue = h.InitialHealth;     
    }
     
  // Use this for initialization
  void Start()
    {
      AddAllToTeam();      
      GetComponent<LimitsMan>().RegisterTransaction();
    }

    // Update is called once per frame
    void Update()
    {

    }
  }


}