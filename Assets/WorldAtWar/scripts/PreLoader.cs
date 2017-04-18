using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PreLoader : MonoBehaviour {

  public Slider progress;
  private AsyncOperation async = null;
  // Use this for initialization
  void Start () {
    StartCoroutine(LoadALevel("scene1"));
	}

  private IEnumerator LoadALevel(string levelName)
  {
    async = SceneManager.LoadSceneAsync(levelName);
    yield return async;
  }

  // Update is called once per frame
  void Update () {
    if ( async!=null ) { 
      progress.value = async.progress;
    }
  }
}
