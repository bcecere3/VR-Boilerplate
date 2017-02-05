using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class Soundtap : VRTK_InteractableObject {

	public AudioClip SFX;

	private AudioSource source;
	// Use this for initialization
	void Awake () {
		source = GetComponent<AudioSource>();
	}
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public override void StartTouching(GameObject currentTouchingObject){
		base.StartTouching (currentTouchingObject);
		Debug.Log ("Happned");
		source.PlayOneShot(SFX,1);
	}

}
