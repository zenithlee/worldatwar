using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour {

  public Types.SelectionState State = Types.SelectionState.None;

  public GameObject PointerCursor;
  public GameObject BuildCursor;
  public GameObject MoveCursor;
  public GameObject AttackCursor;

  public void HideAllCursors()
  {    
    BuildCursor.SetActive(false);
    MoveCursor.SetActive(false);
    AttackCursor.SetActive(false);
    PointerCursor.SetActive(false);
  }

  public void SetState( Types.SelectionState newState )
  {
    State = newState;
    HideAllCursors();

    if (State == Types.SelectionState.None)
    {
      PointerCursor.SetActive(true);
    }

    if ( State == Types.SelectionState.BuildAt)
    {
      BuildCursor.SetActive(true);
    }
    if (State == Types.SelectionState.MoveTo)
    {
      MoveCursor.SetActive(true);
    }
    if (State == Types.SelectionState.Attack)
    {
      AttackCursor.SetActive(true);
    }
  }

  public void SetPosition(Vector3 v)
  {
    transform.position = v;
  }
  // Use this for initialization
  void Start () {
    PointerCursor = transform.Find("PointerCursor").gameObject;
    BuildCursor = transform.Find("BuildCursor").gameObject;
    MoveCursor = transform.Find("MoveCursor").gameObject;
    AttackCursor = transform.Find("AttackCursor").gameObject;
  }
	
	// Update is called once per frame
	void Update () {
		
	}
}
