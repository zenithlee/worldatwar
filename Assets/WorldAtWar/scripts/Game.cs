using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

[Serializable]
public class GameData
{
  [SerializeField]
  public float Version = 1.0f;
}

public class Game : MonoBehaviour {
  GameData Data = new GameData();
  public List<Selectable> Selection = new List<Selectable>();
  public GameObject GameTerrain;
  public GameObject MyArmy;
  public Team MyTeam;
  public Cursor cursor;
  public GameObject MapBlip; // a ping on the map, object self destructs quickly
  public GameObject Explosion;

  SelectionBounds Bounds;

  enum CameraModes { Follow };
  CameraModes CameraMode = CameraModes.Follow;
  public CameraController MainCamera;

  enum States { Menu, Game, Placing };
  States State = States.Game;

  public float MinimumMoveDistance = 6;

  // Use this for initialization
  void Start()
  {
    Bounds = transform.GetComponentInChildren<SelectionBounds>();
    cursor = transform.GetComponentInChildren<Cursor>();
    MainCamera.SetLookAt(MyArmy.transform.Find("Flag").transform.position);
  }

  void Save()
  {
    string GameName = "SavedGames" + "/SavedGame.dat";
    Directory.CreateDirectory("SavedGames");
    
    BinaryFormatter bf = new BinaryFormatter();       
    FileStream file = new FileStream(GameName, FileMode.Create);
    
    //save our data
    bf.Serialize(file, Data);

    Transform map = transform.Find("Map");
    Team[] ta = map.GetComponentsInChildren<Team>();
    //save each team data
    foreach( Team t in ta)
    {
      bf.Serialize(file, t.Data);
    }
    
    file.Close();
    Debug.Log("Saved:" + GameName);
  }

  public void SelectAll()
  {
    DeselectAll();
    foreach (Selectable t in MyArmy.GetComponentsInChildren<Selectable>())
    {
      //t.GetComponent<AudioNode>().
      if ( t.GetComponent<Vehicle>() != null) { 
        t.SendMessage("Select");
      }
    }    
  }

  public void SelectAssault()
  {
    SelectByType(Types.ConstructionTypes.Assault);
    SelectByType(Types.ConstructionTypes.Gunner);
    SelectByType(Types.ConstructionTypes.Sniper);
  }

  public void SelectTanks()
  {
    SelectByType(Types.ConstructionTypes.Tank);
  }

  public void SelectJeeps()
  {
    SelectByType(Types.ConstructionTypes.Jeep);
  }

  public void SelectByType(Types.ConstructionTypes type)
  {
    DeselectAll();
    foreach (Selectable t in MyArmy.GetComponentsInChildren<Selectable>())
    {
      if ( t.Data.Type == type )
      {
        t.SendMessage("Select");
      }
    }
  }

  public void DeselectAll()
  {
    foreach(Selectable t in Selection )
    {
      t.Deselect();
      //t.GetComponent<Vehicle>().Deselect();
    }
    Selection.Clear();
  }

  void SelectMe(Selectable t)
  {
    if ( t.Data.Team == MyTeam.Data.TeamID ) { 
      Selection.Add(t);
      cursor.SetState(Types.SelectionState.MoveTo);
    }
  }

  void PlaceMe(GameObject go)
  {    
    SelectMe(go.GetComponent<Selectable>());
    cursor.SetState(Types.SelectionState.BuildAt);
  }

  void MoveMe(GameObject go, Vector3 t)
  {
    go.GetComponent<Selectable>().SetTarget(t);
  }

  void KillMe(Selectable go)
  {
    Instantiate(Explosion, go.transform.position, go.transform.rotation);
    Selection.Remove(go.GetComponent<Selectable>());
    Bounds.SetUnits(Selection);
    GameObject.Destroy(go.gameObject, 0.5f);
  }

  void MoveSelection(Vector3 v)
  {
    float xp = 0;
    float zp = 0;
    float cut = Mathf.Sqrt(Selection.Count);
    foreach (Selectable t in Selection)
    {      
      if ((t.GetComponent<Building>()!=null) && (t.IsPlacing == false))
      {
        continue;
      }
      if ((t.GetComponent<Building>() != null) && (t.IsPlacing == true))
      {
        Vector3 bpos = Tools.GetSnap(t.gameObject, v);
        t.MoveTo(bpos);
        continue;
      }

      if (Vector3.Distance(v, t.transform.position) < t.GetRadius()*2)
      {
        continue;
      }


      float r = t.GetRadius() * 3;

      Vector3 pos = Tools.GetSnap(t.gameObject, v + new Vector3(xp-cut/2*r, 0, zp-cut/2*r));
      t.SetTarget(pos);      

      xp +=r;
      if ( xp  > cut*r )
      {
        xp = 0;
        zp += r;
      }
    }  
  }

  void DoCamera()
  {
    //Camera c = MainCamera.GetComponent<Camera>();
    if ( Selection.Count > 0 ) {
      MainCamera.SetLookAt(Bounds.Center);
    }
  }

  void Click(Vector3 v)
  {
    if ( Selection.Count >0 ) { 
    NavMeshHit hit;
    if (NavMesh.SamplePosition(v, out hit, 100, NavMesh.AllAreas))
    {
      MoveSelection(hit.position);
    }
    else {
      SendMessage("DoCancel");
    }
    } else
    {
      MainCamera.SetLookAt(v);
    }
    //GetComponent<BuildMan>().MarkerAt(this.transform, hit.position);

  }

  void AddBlip( Vector3 v)
  {
    GameObject go = Instantiate(MapBlip, v, Quaternion.identity);
    go.GetComponent<MapBlip>().Kill();
  }

  void CheckCursor()
  {
    RaycastHit hit;
    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    if (Physics.Raycast(ray, out hit, Mathf.Infinity))
    {
     // Debug.Log(hit.transform.name);
      cursor.SetPosition(hit.point);

      if ( hit.transform == GameTerrain.transform )
      {
        cursor.SetState(Types.SelectionState.MoveTo);
      }
      else
      {      
        Selectable s = hit.transform.GetComponent<Selectable>();
        if (s != null) {
          if (MyTeam.Data.TeamID != s.Data.Team)
          {
            cursor.SetState(Types.SelectionState.Attack);
          }
          else
          {
            cursor.SetState(Types.SelectionState.None);
          }
        }
      }
    }
  }

  void CheckDrag()
  {

  }

  void CheckKeys()
  {
    if ( Input.GetKeyUp(KeyCode.S))
    {
      Save();
    }
  }

  // Update is called once per frame
  void Update () {

    DoCamera();
    CheckCursor();
    CheckDrag();
    CheckKeys();

    RaycastHit hit;
    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    if (GameTerrain.GetComponent<Collider>().Raycast(ray, out hit, Mathf.Infinity))
    {
      cursor.SetPosition(hit.point);
    }

      // Debug.Log(EventSystem.current.IsPointerOverGameObject());
      if ( !EventSystem.current.IsPointerOverGameObject()) { 
        if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
        {              
          if (GameTerrain.GetComponent<Collider>().Raycast(ray, out hit, Mathf.Infinity))
          {
            AddBlip(hit.point);
            //SelectedUnit.transform.position = hit.point;
            foreach (Selectable t in Selection)
            {
              if ( Input.GetMouseButtonUp(1))
              {
                t.Action();
              }          
            }
          }

          if (Input.GetMouseButtonUp(0))
          {
            if (cursor.State == Types.SelectionState.Attack)
            {
              MoveSelection(hit.point);
            }
            if (cursor.State == Types.SelectionState.MoveTo)
            {
              MoveSelection(hit.point);
            }
          
          }
        }
      }
    Bounds.SetUnits(Selection);
  }
}
