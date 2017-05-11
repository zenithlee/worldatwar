using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace WW
{
  public class SaveLoad : MonoBehaviour
  {
    // Use this for initialization
    private void Start()
    {
    }

    public void Save()
    {
      string GameName = "SavedGames" + "/SavedGame.dat";
      Directory.CreateDirectory("SavedGames");

      BinaryFormatter bf = new BinaryFormatter();
      FileStream file = new FileStream(GameName, FileMode.Create);

      Dictionary<string, object> d = new Dictionary<string, object>();

      Game g = GetComponent<Game>();
      d["gamedata"] = g.Data;
      // save our data

      Transform map = transform.Find("Map");
      Team[] ta = map.GetComponentsInChildren<Team>();
      //save each team data
      List<TeamData> teams = new List<TeamData>();
      foreach (Team t in ta)
      {
        teams.Add(t.Data);
      }

      d["teams"] = teams;


      List<object> sels = new List<object>();
      foreach (Team t in ta)
      { 
        Selectable[] csels = t.gameObject.GetComponentsInChildren<Selectable>();

        foreach( Selectable s in csels ) {
          List<object> obs = new List<object>();
          obs.Add(s.Serialize());
          Vehicle v = s.GetComponent<Vehicle>();
          if (v != null) obs.Add(v.Data);
          Building b = s.GetComponent<Building>();
          if (b != null) obs.Add(b.Data);

          sels.Add(obs);
        }        
      }

      d["selectables"] = sels;


      bf.Serialize(file, d);
      file.Close();
      Debug.Log("Saved:" + GameName);
    }

    public void Load()
    {
      string GameName = "SavedGames" + "/SavedGame.dat";
      FileStream file = new FileStream(GameName, FileMode.Open);
      BinaryFormatter bf = new BinaryFormatter();
      Game g = GetComponent<Game>();
      Dictionary<string, object> o = (Dictionary<string, object>)bf.Deserialize(file);

      //GAMEDATA
      g.Data = (GameData)o["gamedata"];

      //TEAMS
      List<TeamData> teams = (List<TeamData>)o["teams"];

      Transform map = transform.Find("Map");
      Team[] ta = map.GetComponentsInChildren<Team>();
      //save each team data
      // TODO: Create Teams from scratch, setup prefabs depending on team ID
      for (int i = 0; i < g.Data.NumTeams; i++)
      {
        ta[i].Data = teams[i];
        ta[i].Clear();
      }

      // SELECTABLES
      List<object> sels = (List<object>)o["selectables"];
      BuildMan bm = GetComponent<BuildMan>();
      foreach( List<object> d in sels )
      {        
        bm.BuildFromData(d);
      }

    }

    // Update is called once per frame
    private void Update()
    {
    }
  }
}