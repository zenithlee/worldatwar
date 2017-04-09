using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(AudioSource))]
public class AudioNode : MonoBehaviour {

  AudioSource _AudioSource;
  public AudioClip SelectSound;
  public AudioClip AckSound;
  public AudioClip Move;
  public AudioClip Action;  

  void DoPlace()
  {
    Play(AckSound);
  }

  void DoSelect()
  {
    Play(SelectSound);
  }

  void DoSetTarget(Vector3 v)
  {
    Play(AckSound);

    Invoke("MoveSound", 0.5f);
  }

  void DoAction()
  {
    Play(Action);
  }

  void MoveSound()
  {
    Play(Move);
  }

  void Play(AudioClip c)
  {
    if ( _AudioSource.isPlaying)
    {
      _AudioSource.Stop();
    }
    
      _AudioSource.clip = c;
      _AudioSource.loop = false;
      _AudioSource.Play();    
  }
  // Use this for initialization
  void Start () {
    _AudioSource = GetComponent<AudioSource>();
  }
	
	// Update is called once per frame
	void Update () {
		
	}
}
