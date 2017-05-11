using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace WW { 

public class AudioMan : MonoBehaviour {

  public AudioMixerGroup MixerGroup;
  public AudioSource Music;
  public float MusicVolume = 0.5f;
  public AudioClip MainMusic;
  public AudioClip AttackMusic;

  public AudioSource Beeps;
  public AudioClip Beep1;


  public AudioSource Units;

	// Use this for initialization
	void Start () {
    Music = gameObject.AddComponent<AudioSource>();
    Music.outputAudioMixerGroup = MixerGroup;
    Music.volume = MusicVolume;
    Beeps = gameObject.AddComponent<AudioSource>();
    Beeps.outputAudioMixerGroup = MixerGroup;
    Units = gameObject.AddComponent<AudioSource>();
    Units.outputAudioMixerGroup = MixerGroup;

    PlayMusic();
      InvokeRepeating("CheckMusic", 5, 5);
  }

    void CheckMusic()
    {
      Vehicle[] sels = GetComponentsInChildren<Vehicle>();
      bool IsAttack = false;
      foreach( Vehicle v in sels )
      {
        if ( v.State == Vehicle.States.Attack )
        {
          IsAttack = true;
        }
      }

      if ( IsAttack == true )
      {
        if ( Music.clip != AttackMusic )
        {
          Music.clip = AttackMusic;
          Music.Play();
        }
        
      }
      else
      {
        if (Music.clip != MainMusic)
        {
          Music.clip = MainMusic;
          Music.Play();
        }
        
      }
    }

  void PlayMusic()
  {
    Music.clip = MainMusic;
    Music.Play();
  }

  void SelectMe(Selectable t)
  {
    Beeps.clip = Beep1;
    Beeps.Play();
  }
  
  // Update is called once per frame
    void Update () {
		
	}
}


}