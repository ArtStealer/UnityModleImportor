using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rebindAnim : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Animator a = GetComponent<Animator>();
        a.Rebind();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
