using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputMan : MonoBehaviour {

  //public enum INPUTSTATE { NONE, PANNING, SELECTING, MOVING, ATTACK }
  //public Types.SelectionState State = Types.SelectionState.PanView;
  public Cursor cursor;

  public Vector3 mousePress;
  public Vector3 mouseRelease;
  public Vector3 mouseDif;

  GameObject GameTerrain;
  CameraController MainCam;

  // Use this for initialization
  void Start () {
    MainCam = GameObject.Find("Camera").GetComponent<CameraController>();
    GameTerrain = GameObject.FindGameObjectWithTag("Terrain");
	}
 
  /**
  * returns true if the mouse is over a UI element
  */
  public bool OverUI()
  {
    return EventSystem.current.IsPointerOverGameObject();
  }

  void CheckKeys()
  {
    if (Input.GetKeyUp(KeyCode.S))
    {
      SendMessage("SaveGame");
    }
  }

  RaycastHit GetWorldClick()
  {
    RaycastHit hit;
    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    GameTerrain.GetComponent<Collider>().Raycast(ray, out hit, Mathf.Infinity);   
    return hit;
  }

  void CheckPress()
  {   
      if ( Input.GetMouseButtonDown(0))
    {
      //over UI
      if (OverUI())
      {
        return;
      }

      RaycastHit hit = GetWorldClick();
      mousePress = hit.point;     
    }
  }

  void CheckDelta()
  {
    if (Input.GetMouseButton(0)==true) {

      if (OverUI())
      {
        return;
      }


      if ( cursor.State == Types.SelectionState.None )
      {
        RaycastHit hit = GetWorldClick();
        if (hit.transform != null)
        {
          mouseDif = mousePress - hit.point;

          if ((mouseDif.magnitude > 10) && (hit.transform == GameTerrain.transform))
          {
            if (cursor.State == Types.SelectionState.None)
            {
              cursor.SetState(Types.SelectionState.PanView);
            }
          }
        }
      }
    }

    if ( cursor.State == Types.SelectionState.PanView )
    {
      RaycastHit hit = GetWorldClick();
      if ( hit.transform != null ) {
        mouseDif = mousePress - hit.point;
      }
      else
      {
        mouseDif = Vector3.zero;
      }      
     // Debug.Log(mouseDif);
      MainCam.PanBy(mouseDif * 2);
      mousePress = hit.point;
    }
  }

  void CheckRelease()
  {
    
    //if we clicked on something and our distance is minimal, action it
    if (Input.GetMouseButtonUp(0))
    {

      if (OverUI())
      {
        cursor.SetState(Types.SelectionState.None);
        return;
      }


      mouseRelease = GetWorldClick().point;
      float dist = Vector3.Distance(mousePress, mouseRelease);
      //Debug.Log(dist);
      if ( dist < 10 )
      {        
        //this was a tap and not a drag
        CheckSelect();
        CheckMove();
      }
      else
      {
        
      }
      cursor.SetState( Types.SelectionState.None );
    }    
  }

  void CheckSelect()
  {
   
  }
  
  void CheckMove()
  {       
      if (cursor.State != Types.SelectionState.PanView)
      {
        SendMessage("MoveSelection", mouseRelease);
      }   
  }

  void CheckScrollWheel()
  {
    float Scroll = Input.GetAxis("Mouse ScrollWheel") * 15;
    if (Scroll != 0)
    {
      //this.transform.Translate( new Vector3(0,0,Scroll));

      Camera[] cams = MainCam.transform.GetComponentsInChildren<Camera>();
      foreach( Camera c in cams )
      {
        c.orthographicSize -= Scroll;
        if ( c.orthographicSize < 1 ) c.orthographicSize = 1;
        if (c.orthographicSize > 100) c.orthographicSize = 100;
      }
    }
  }


  // Update is called once per frame
  void Update () {
    CheckPress();
    CheckDelta();
    
    CheckKeys();
    CheckScrollWheel();
  }

  void LateUpdate()
  {
    CheckRelease();
  }
}
