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
    public int TeamID = 0;
    [SerializeField]
    public float Credits = 0;
  }

  public class Team : MonoBehaviour
  {

    //public int TeamID = 0;
    public TeamData Data = new TeamData();

    public GameObject[] Prototypes;

    public GameObject Marker1Proto;
    public GameObject RadarDotBuildingProto;
    public GameObject RadarDotVehicleProto;
    public GameObject RadarDotInfantryProto;

    public GameObject GetProtoByType(Types.ConstructionTypes type)
    {
      foreach (GameObject go in Prototypes)
      {
        Selectable s = go.GetComponent<Selectable>();
        if ((s != null ) && (s.Data.Type == type))
        {
          return go;
        }
      }
      return null;
    }

    void AddAllToTeam()
    {
      for (int i = 0; i < transform.childCount; i++)
      {
        Transform t = transform.GetChild(i);
        Selectable s = t.GetComponent<Selectable>();
        if (s != null)
        {
          s.Data.Team = Data.TeamID;
        }
      }

      foreach (GameObject go in Prototypes)
      {
        Selectable s = go.GetComponent<Selectable>();
        if (s != null )
        {
          s.Data.Team = Data.TeamID;
        }
      }
    }

    public void Clear()
    {
      while ( transform.childCount> 0 )
      {
        GameObject.Destroy(transform.GetChild(0).gameObject);
        transform.GetChild(0).parent = null;       
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