using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class StoryManager : MonoBehaviour {

    public enum TapOrDrag {
        Tap,
        Drag
    }

    AudioSource audioSource;

    public int currentStep;

    [SerializeField]
    public Step[] steps;

    [System.Serializable]
    public class Step : object{
        [SerializeField]
        public GameObject objectTarget;
        [SerializeField]
        public AudioClip audioClip;
        [SerializeField]
        public AnimationClip animClip;
        [SerializeField]
        public int stepOrder;
        [SerializeField]
        public TapOrDrag tapOrDrag;
    }

    public void Awake() {
        audioSource = GetComponent<AudioSource>();
    }

    public void Update() {
        for (var i = 0; i < Input.touchCount; ++i) {
            if (Input.GetTouch(i).phase == TouchPhase.Began) {
                Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(i).position);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit)) {
                    foreach(Step elem in steps) {
                        if(hit.transform.gameObject == elem.objectTarget && currentStep == elem.stepOrder && !audioSource.isPlaying) {
                            playAudio(elem.audioClip);
                            //play animation
                            currentStep++;
                        }
                    }
                }
            }
        }
    }

    public void playAudio(AudioClip audio) {
        audioSource.clip = audio;
        audioSource.Play();
    }
}
