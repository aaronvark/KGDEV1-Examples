using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DesignPatterns {

	public delegate void SoundEvent( string soundName );

	//interface to encapsulate the "SFXPlayer" behaviour of the SFXManager
	public interface ISFXPlayer {
		event SoundEvent onPlaySound;
		void Play(AudioClip clip, float volume = 1, float pan = 0);
	}
	
	//Singleton for playing of SFX, specifically tailored to 2D games
	//SFX class needs to be called from many places in the program
	// - Improvements: use an enum to indicate which sound is being played, not a clip
	//	-> This also means we need to think about how sound clips are managed (who loads them, where do we link the enum and the clips?)
	//  -> , which is typically a problem we can solve using a small Editor Tool (overview in which we link clips and enum, SFXManager loads them)
	public class SFXManager : MonoBehaviour, ISFXPlayer {
		private static SFXManager instance;
		public static ISFXPlayer Instance {
			get {
				//lazy initialization
				if ( instance == null ) instance = CreateInstance();
				return instance;
			}
		}

		public event SoundEvent onPlaySound;

		private const int NUM_CHANNELS = 16;
		//This kind of setup is useful for 2D games where sounds play on the camera, and can only be panned left/right
		private AudioSource[] channels = new AudioSource[NUM_CHANNELS];
		private int currentChannel = 0;

		public void Play(AudioClip clip, float volume = 1, float pan = 0) {
			currentChannel = ++currentChannel % NUM_CHANNELS;
			channels[currentChannel].clip = clip;
			channels[currentChannel].volume = volume;
			channels[currentChannel].panStereo = pan;
			channels[currentChannel].Play();

			if ( onPlaySound != null ) {
				onPlaySound(clip.name);
			}
		}

		private static SFXManager CreateInstance() {
			//do more complex initialization
			GameObject g = GameObject.Find("_sfxManager");
			if ( g == null ) {
				g = new GameObject("_sfxManager");
			}
			
			SFXManager component = g.AddComponent<SFXManager>();
			component.Initialize();

			return component;
		}

		private void Initialize() {
			//instance-side initialization (Awake might be called too late!)
			GameObject g;
			for( int i = 0; i < NUM_CHANNELS; ++i ) {
				g = new GameObject("channel"+(i+1));
				channels[i] = g.AddComponent<AudioSource>();
				channels[i].spatialBlend = 0; //2D
				g.transform.parent = transform;
				g.transform.localPosition = Vector3.zero;
			}
		}
	}

	//Observer
	//Enemy listens to SFXManager to respond to sounds being played
	public class Enemy : MonoBehaviour {
		void Start() {
			SFXManager.Instance.onPlaySound += SoundPlayed;
		}

		private void SoundPlayed( string soundName ) {
			//react to a sound being played!
			switch( soundName ) {
				case "footstep":
					//alert, alert!
					//here we can have this act as an input to a statemachine we're running
					//for instance: 
					//	- Calm enemy hears sound, goes to alerted state.
					//	- Wanders around for 10 seconds. If they find nothing, goes back to calm (patrol?)
					//		if they find something, go into attack state.
					//	- If enemy dies, goes back to calm. If loses sight of enemy, goes back into alert state
				break;
			}
		}
	}
}
