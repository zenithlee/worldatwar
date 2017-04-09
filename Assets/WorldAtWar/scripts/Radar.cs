using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Radar : MonoBehaviour, IPointerDownHandler
{
  private Vector3 clickMousePos;
  private Vector3 clickPos;
  public void OnPointerDown(PointerEventData eventData)
  {
    Vector2 localCursor;
    RectTransform rect1 = GetComponent<RectTransform>();
    Vector2 pos1 = eventData.position;
    if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(rect1, pos1, null, out localCursor))
    {
      return;
    }

    int xpos = (int)(localCursor.x);
    int ypos = (int)(localCursor.y);

    if (xpos < 0) xpos = xpos + (int)rect1.rect.width / 2;
    else xpos += (int)rect1.rect.width / 2;

    if (ypos > 0) ypos = ypos + (int)rect1.rect.height / 2;
    else ypos += (int)rect1.rect.height / 2;

    //convert clicks to percentage
    Vector2 pPoint = new Vector2(xpos / 100.0f, ypos / 100.0f);    
    
    //now get the terrain and figure out the percentage
    GameObject go = GameObject.Find("Game/Terrain");
    
    Terrain t = go.GetComponent<Terrain>();
    Vector3 pos = new Vector3(t.terrainData.size.x * pPoint.x, 0, t.terrainData.size.z * pPoint.y);
    Debug.Log(pos);
    SendMessageUpwards("Click", pos);

    //Debug.Log("Correct Cursor Pos: " + xpos + " " + ypos);
  }

  void OnMouseDown()
  {
    Debug.Log("Radar Clicked");
  }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
