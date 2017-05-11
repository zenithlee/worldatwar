using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

namespace WW
{

  [Serializable]
  public class GameData
  {
    [SerializeField]
    public float Version = 1.0f;
    [SerializeField]
    public int NumTeams = 2;
  }

  public class Game : MonoBehaviour
  {
    public GameData Data = new GameData();

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

    InputMan inputMan;

    // Use this for initialization
    void Start()
    {
      Bounds = transform.GetComponentInChildren<SelectionBounds>();
      cursor = transform.GetComponentInChildren<Cursor>();
      MainCamera.SetLookAt(MyArmy.transform.Find("Flag").transform.position);
      inputMan = transform.GetComponent<InputMan>();
      InvokeRepeating("NotifySelection", 1.1f, 1.1f);
    }

    void NotifySelection()
    {
      GetComponent<UIMan>().SetSelection(Selection);
    }

    public void SaveGame()
    {
      SaveLoad sl = GetComponent<SaveLoad>();
      sl.Save();
      
    }

    public void LoadGame()
    {
      SaveLoad sl = GetComponent<SaveLoad>();
      sl.Load();      
    }

    public void Quit()
    {
      Application.Quit();
    }

    public void SelectAll()
    {
      DeselectAll();
      foreach (Selectable t in MyArmy.GetComponentsInChildren<Selectable>())
      {
        //t.GetComponent<AudioNode>().
        if (t.GetComponent<Vehicle>() != null)
        {
          t.SendMessage("Select");
        }
      }
    }

    public void SelectInfantry()
    {
      SelectByType(Types.ConstructionTypes.Assault);
      SelectByType(Types.ConstructionTypes.Gunner, true);
      SelectByType(Types.ConstructionTypes.Sniper, true);
    }

    public void SelectVehicles()
    {
      SelectByType(Types.ConstructionTypes.Tank);
      SelectByType(Types.ConstructionTypes.Jeep, true);
      SelectByType(Types.ConstructionTypes.APC, true);
      SelectByType(Types.ConstructionTypes.LargeTank, true);
      SelectByType(Types.ConstructionTypes.SmallTank, true);
    }

    public void SelectJeeps()
    {
      SelectByType(Types.ConstructionTypes.Jeep);
    }

    public void SelectByType(Types.ConstructionTypes type, bool Add = false)
    {
      if (Add == false)
      {
        DeselectAll();
      }
      foreach (Selectable t in MyArmy.GetComponentsInChildren<Selectable>())
      {
        if (t.Data.Type == type)
        {
          t.SendMessage("Select");
        }
      }
    }

    public void DeselectAll()
    {
      foreach (Selectable t in Selection)
      {
        t.Deselect();
        //t.GetComponent<Vehicle>().Deselect();
      }
      Selection.Clear();
      SendMessage("DoDeselectAll");
    }

    void SelectMe(Selectable t)
    {
      if ( cursor.State == Types.SelectionState.None)
      {
        DeselectAll();
      }

      if ( cursor.State == Types.SelectionState.Attack)
      {
        Attack(t);
      }

      if (t.Data.Team == MyTeam.Data.TeamID)
      {
        Selection.Add(t);
        cursor.SetState(Types.SelectionState.MoveTo);
      }
      
    }

    void Attack( Selectable sel)
    {
      foreach (Selectable t in Selection)
      {
        Vehicle v = t.GetComponent<Vehicle>();
        if ( v != null )
        {
          float r = sel.GetRadius();
          v.DoSetTarget(sel.transform.position + new Vector3(UnityEngine.Random.value * r,0, UnityEngine.Random.value * r));
        }
      }
    }

    void PlaceMe(GameObject go)
    {
      Selectable gs = go.GetComponent<Selectable>();
      if (gs.Data.Team == MyTeam.Data.TeamID)
      {
        SelectMe(gs);
        cursor.SetState(Types.SelectionState.BuildAt);
      }

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
      go.transform.parent = null;
      MyTeam.GetComponent<LimitsMan>().CheckBuild(go.Data.Type);

      GameObject.Destroy(go.gameObject, 0.5f);
    }

    public void KillSelected()
    {
      List<Selectable> tempList = new List<Selectable>();
      foreach (Selectable s in Selection)
      {
        tempList.Add(s);
      }

      foreach ( Selectable s in tempList)
      {
        KillMe(s);
      }
    }

    public void RepairSelected()
    {
      foreach (Selectable s in Selection)
      {
        MyTeam.RepairUnit(s);
      }

      MyTeam.GetComponent<LimitsMan>().RegisterTransaction(); 
    }

    bool IsSpaceOccupied(Selectable s, Vector3 v)
    {
      float rr = s.GetRadius()/2.0f;
      RaycastHit[] hits = (Physics.BoxCastAll(v, new Vector3(rr, rr, rr), Vector3.right));

      bool occupied = false;

      foreach( RaycastHit hit in hits )
      {
        if (hit.transform == GameTerrain.transform) continue;
        else
        if (hit.transform == s.transform) continue;
        else
        {
          occupied = true;
        }
      }
      return occupied;
    }

    void MoveSelection(Vector3 v)
    {
      float xp = 0;
      float zp = 0;
      float cut = Mathf.Sqrt(Selection.Count);
      foreach (Selectable t in Selection)
      {
        if ((t.GetComponent<Building>() != null) && (t.IsPlacing == false))
        {
          continue;
        }
        if ((t.GetComponent<Building>() != null) && (t.IsPlacing == true))
        {
          Vector3 bpos = Tools.GetSnapXZ(t.gameObject, v);
          if ( !IsSpaceOccupied( t, bpos)) { 
          t.MoveTo(bpos);
            }
          continue;
        }

        if (Vector3.Distance(v, t.transform.position) < t.GetRadius() * 2)
        {
          continue;
        }


        float r = t.GetRadius() * 3;

        Vector3 pos = Tools.GetSnap(t.gameObject, v + new Vector3(xp - cut / 2 * r, 0, zp - cut / 2 * r));
        t.SetTarget(pos);

        xp += r;
        if (xp > cut * r)
        {
          xp = 0;
          zp += r;
        }
      }
    }

    void DoCamera()
    {
      //Camera c = MainCamera.GetComponent<Camera>();
      /*if (Selection.Count > 0)
      {
        MainCamera.SetLookAt(Bounds.Center);
      }
      */
    }

    void Click(Vector3 v)
    {
      if (Selection.Count > 0)
      {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(v, out hit, 100, NavMesh.AllAreas))
        {
          MoveSelection(hit.position);
        }
        else
        {
          SendMessage("DoCancel");
        }
      }
      else
      {
        MainCamera.SetLookAt(v);
      }
      //GetComponent<BuildMan>().MarkerAt(this.transform, hit.position);

    }

    void AddBlip(Vector3 v)
    {
      GameObject go = Instantiate(MapBlip, v, Quaternion.identity);
      go.GetComponent<MapBlip>().Kill();
    }

    void ClickAt( Vector3 v )
    {

    }

    void CheckCursor()
    {
      RaycastHit hit;
      Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
      if (Physics.Raycast(ray, out hit, Mathf.Infinity))
      {
        // Debug.Log(hit.transform.name);
        cursor.SetPosition(hit.point);

        if (hit.transform == GameTerrain.transform)
        {
          cursor.SetState(Types.SelectionState.None);
        }
        else
        {
          Selectable s = hit.transform.GetComponent<Selectable>();
          if (s != null)
          {
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
      RaycastHit hit;
      Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
      if (GameTerrain.GetComponent<Collider>().Raycast(ray, out hit, Mathf.Infinity))
      {
        // Debug.Log(EventSystem.current.IsPointerOverGameObject());
        if (!inputMan.OverUI())
        {
          if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
          {
            AddBlip(hit.point);
            //SelectedUnit.transform.position = hit.point;
            foreach (Selectable t in Selection)
            {
              if (Input.GetMouseButtonUp(1))
              {
                t.Action();
              }
            }
          }
        }
      }
    }

    public void MoveCameraHome()
    {
      MainCamera.MoveTo(MyTeam.transform.position);
    }

    // Update is called once per frame
    void Update()
    {

      DoCamera();
      CheckCursor();
      CheckDrag();
      
      Bounds.SetUnits(Selection);
    }
  }

}