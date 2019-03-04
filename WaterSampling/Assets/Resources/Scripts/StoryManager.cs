using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using TMPro;

public class StoryManager : MonoBehaviour {

    public enum TapOrDrag {
        Tap,
        Drag
    }
    public enum ManipulationType {
        Transform,
        Rotate,
        Scale
    }
    public enum ManipulationAxis {
        X,
        Y,
        Z
    }

    AudioSource audioSource;

    public int currentStep;

    [SerializeField]
    public GameObject slider;

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
        [SerializeField]
        public bool hasSlider;
        [SerializeField, Range(0,1)]
        public float sliderTarget;
        [SerializeField]
        public bool manipulateObject;
        [SerializeField]
        public ManipulationType manipulationType;
        [SerializeField]
        public ManipulationAxis manipulationAxis;
        [SerializeField]
        public float manipulationMultiplier;
    }

    public void Awake() {
        audioSource = GetComponent<AudioSource>();
        currentStep = 0;
    }

    public void Update() {
        for (var i = 0; i < Input.touchCount; ++i) {
            if (Input.GetTouch(i).phase == TouchPhase.Began) {
                Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(i).position);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit)) {
                    GameObject.Find("TextMeshPro Text").GetComponent<TextMeshProUGUI>().text = hit.transform.gameObject.name;
                    foreach(Step elem in steps) {
                        if(hit.transform.gameObject == elem.objectTarget && currentStep == elem.stepOrder && !audioSource.isPlaying) {
                            if(elem.animClip != null) {
                                hit.transform.gameObject.GetComponent<Animator>().Play(elem.animClip.name);
                            }
                            if(elem.audioClip != null) {
                                playAudio(elem.audioClip);
                            }
                            if (elem.hasSlider) {
                                if (!slider.activeSelf) {
                                    slider.SetActive(true);
                                    StartCoroutine(CheckSlider(elem));
                                }
                            } else {
                               slider.SetActive(false);
                            }
                            if(currentStep == steps.Length) {
                                GameObject.Find("PauseButton").GetComponent<PauseMenu>().Pause();
                                GameObject.Find("PlayButton").SetActive(false);
                            }
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

    IEnumerator CheckSlider(Step elem) {
        while (slider.GetComponent<Slider>().value != elem.sliderTarget) {
            Vector3 p = elem.objectTarget.transform.localPosition;
            Vector3 r = elem.objectTarget.transform.localRotation.eulerAngles;
            Vector3 s = elem.objectTarget.transform.localScale;
            switch (elem.manipulationType) {
                case ManipulationType.Transform:
                switch (elem.manipulationAxis) {
                    case ManipulationAxis.X:
                    p.x = slider.GetComponent<Slider>().value * elem.manipulationMultiplier;
                    break;
                    case ManipulationAxis.Y:
                    p.y = slider.GetComponent<Slider>().value * elem.manipulationMultiplier;
                    break;
                    case ManipulationAxis.Z:
                    p.z = slider.GetComponent<Slider>().value * elem.manipulationMultiplier;
                    break;
                }
                break;
                case ManipulationType.Rotate:
                switch (elem.manipulationAxis) {
                    case ManipulationAxis.X:
                    GameObject.Find("TextMeshPro Text").GetComponent<TextMeshProUGUI>().text = ("" + r);
                    elem.objectTarget.transform.localEulerAngles = new Vector3(slider.GetComponent<Slider>().value * elem.manipulationMultiplier, r.y,r.z);
                    break;
                    case ManipulationAxis.Y:
                    elem.objectTarget.transform.Rotate(new Vector3(r.x,slider.GetComponent<Slider>().value * elem.manipulationMultiplier,r.z));
                    break;
                    case ManipulationAxis.Z:
                    elem.objectTarget.transform.Rotate(new Vector3(r.x,r.y,slider.GetComponent<Slider>().value * elem.manipulationMultiplier));
                    break;
                }
                break;
                case ManipulationType.Scale:
                switch (elem.manipulationAxis) {
                    case ManipulationAxis.X:
                    s.x = slider.GetComponent<Slider>().value * elem.manipulationMultiplier;
                    break;
                    case ManipulationAxis.Y:
                    s.y = slider.GetComponent<Slider>().value * elem.manipulationMultiplier;
                    break;
                    case ManipulationAxis.Z:
                    s.z = slider.GetComponent<Slider>().value * elem.manipulationMultiplier;
                    break;
                }
                break;
            }
            yield return null;
        }
    }
}
