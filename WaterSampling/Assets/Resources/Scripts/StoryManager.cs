using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class StoryManager : MonoBehaviour {

    public enum TapOrDrag {
        Tap,
        Drag
    }

    [SerializeField]
    public Step[] steps;

    [System.Serializable]
    public class Step : object{
        [SerializeField]
        GameObject objectTarget;
        [SerializeField]
        AudioClip audioClip;
        [SerializeField]
        int stepOrder;
        [SerializeField]
        public TapOrDrag tapOrDrag;
    }

    public void Awake() {
        
    }

    public void playAudio() {

    }
}
