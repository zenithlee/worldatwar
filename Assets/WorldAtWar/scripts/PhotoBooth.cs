﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class PhotoBooth : MonoBehaviour {

  public string Folder = "ScreenShots";
  public GameObject CaptureButton;

	// Use this for initialization
	void Start () {
    Directory.CreateDirectory(Folder);
	}

  IEnumerator ieTakePhoto()
  {
    CaptureButton.SetActive(false);
    yield return new WaitForEndOfFrame();

    DateTime d = DateTime.Now;
    string Name = d.ToString("yyyyMMddHHmmss");
    Application.CaptureScreenshot(Folder + "/" + Name + ".png");
    Debug.Log("Captured " + Name);

    yield return new WaitForEndOfFrame();
    CaptureButton.SetActive(true);
  }

  public void TakePhoto()
  {
    StartCoroutine(ieTakePhoto());
  }
	
	// Update is called once per frame
	void Update () {
		if ( Input.GetKeyUp(KeyCode.Alpha1))
    {
      TakePhoto();
    }
	}
}
