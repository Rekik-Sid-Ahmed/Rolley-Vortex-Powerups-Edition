using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour {
	 public Transform from;
    public Transform to;

    private float timeCount = 0.0f;
	public float rot=30;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	 
		transform.rotation = Quaternion.Euler(rot,0,0);
		rot++;
        

		
	}
}
