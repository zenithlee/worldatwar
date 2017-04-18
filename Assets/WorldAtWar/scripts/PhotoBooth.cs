using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[ExecuteInEditMode]
public class PhotoBooth : MonoBehaviour {

  public string Folder = "ScreenShots";

	// Use this for initialization
	void Start () {
    Directory.CreateDirectory(Folder);
	}

  void TakePhoto()
  {
    DateTime d = DateTime.Now;
    string Name = d.ToString("yyyyMMddHHmmss");
    Application.CaptureScreenshot(Folder + "/" + Name + ".png");
    Debug.Log("Captured " + Name);
  }
	
	// Update is called once per frame
	void Update () {
		if ( Input.GetKeyUp(KeyCode.Alpha1))
    {
      TakePhoto();
    }
	}
}
