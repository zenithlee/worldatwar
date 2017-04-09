using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundMan : MonoBehaviour {

  AudioSource _AudioSource;
  public AudioClip Beep;
  // Use this for initialization

  public void Play(string ClipName)
  {
    if (_AudioSource.isPlaying)
    {
      _AudioSource.Stop();
    }

    if ( ClipName == "Beep")
    {
      _AudioSource.clip = Beep;
    }
    
    _AudioSource.loop = false;
    _AudioSource.Play();
  }

  public void DoCancel()
  {
    Play("Beep");
  }

	void Start () {
    _AudioSource = GetComponent<AudioSource>();
  }
	
	// Update is called once per frame
	void Update () {
		
	}
}
