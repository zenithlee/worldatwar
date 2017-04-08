using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class Game : MonoBehaviour {

  public List<Selectable> Selection = new List<Selectable>();
  public GameObject GameTerrain;
  public GameObject MyArmy;  
  SelectionBounds Bounds;

  enum CameraModes { Follow };
  CameraModes CameraMode = CameraModes.Follow;
  public CameraController MainCamera;

  enum States { Menu, Game, Placing };
  States State = States.Game;

  // Use this for initialization
  void Start()
  {
    Bounds = transform.GetComponentInChildren<SelectionBounds>();
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
    //DeselectAll();
    Selection.Add(t);
  }

  void PlaceMe(GameObject go)
  {
    go.transform.parent = MyArmy.transform;

    RaycastHit hit;
    
    Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
    if (GameTerrain.GetComponent<Collider>().Raycast(ray, out hit, Mathf.Infinity))
    {
      NavMeshAgent nm = go.GetComponent<NavMeshAgent>();
      float r = go.GetComponent<Selectable>().GetRadius() * 5;
      if (nm != null)
      {
       
        go.GetComponent<NavMeshAgent>().Warp(hit.point + new Vector3(Random.value*r, 0, Random.value*r));
      }
      else
      {
        go.transform.position = hit.point + new Vector3(Random.value * r, 0, Random.value * r);
      }
      
    }
  }



  void MoveSelection(Vector3 v)
  {
    float xp = 0;
    float zp = 0;
    float cut = Selection.Count / 2;
    foreach (Selectable t in Selection)
    {
      float r = t.GetRadius() * 4;
      t.SetTarget(v + new Vector3(xp, 0, zp));
      xp+=r;
      if ( xp  > cut )
      {
        xp = r/2;
        zp += 2;
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
  
	
	// Update is called once per frame
	void Update () {

    DoCamera();

    Debug.Log(EventSystem.current.IsPointerOverGameObject());
    if ( !EventSystem.current.IsPointerOverGameObject()) { 
      if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
      {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (GameTerrain.GetComponent<Collider>().Raycast(ray, out hit, Mathf.Infinity))
        {
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
          MoveSelection(hit.point);
        }
      }
    }
    Bounds.SetUnits(Selection);
  }
}
