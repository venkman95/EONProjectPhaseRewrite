using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using TMPro;
using Vuforia;

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

    GameObject qAPanel;

    AudioSource audioSource;

    public int currentStep;
    private bool introPlayed = false;
    private bool finished = false;
    [SerializeField]
    public AudioClip introAudio;
    [SerializeField]
    public AudioClip outroAudio;

    [SerializeField]
    public GameObject slider;

    [SerializeField]
    public Step[] steps;

    [System.Serializable]
    public class Step : object {
        [SerializeField]
        public GameObject objectTarget;
        [SerializeField]
        public AudioClip audioClip;
        [SerializeField]
        public AudioClip missTap;
        [SerializeField]
        public AnimationClip animClip;
        [SerializeField]
        public int stepOrder;
        [SerializeField]
        public AnimationClip highlightThis;
        [SerializeField]
        public GameObject highlightTarget;
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
        [SerializeField]
        public bool hasQuestion;
        [SerializeField]
        public String question;
        [SerializeField]
        //Do not set this array to be larger than 4
        public String[] choices;
        [SerializeField]
        public int correctChoice;
    }

    public void Awake() {
        qAPanel = GameObject.Find("QAPanel");
        audioSource = GetComponent<AudioSource>();
        currentStep = 0;
    }

    public void Start() {
        //move this to play intro audio when the marker first comes into view
        AudioListener.pause = false;
    }

    public void Update() {
        if (currentStep == steps.Length && !audioSource.isPlaying && finished==false)
        {
            finished = true;
            //PlayAudio(outroAudio);
            GameObject.Find("EventSystem").GetComponent<PauseMenu>().Pause();
            GameObject.Find("PlayButton").SetActive(false);
        }
        if (!audioSource.isPlaying && introPlayed==true) {
            steps[currentStep].highlightTarget.gameObject.GetComponent<Animator>().Play(steps[currentStep].highlightThis.name);
        }
        for (var i = 0; i < Input.touchCount; ++i) {
            if (Input.GetTouch(i).phase == TouchPhase.Began) {
                Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(i).position);
                RaycastHit hit;
                if (Physics.Raycast(ray,out hit)) {
                    //GameObject.Find("TextMeshPro Text").GetComponent<TextMeshProUGUI>().text = hit.transform.gameObject.name;
                    foreach (Step elem in steps) {
                        if (hit.transform.gameObject == elem.objectTarget && currentStep == elem.stepOrder && (!audioSource.isPlaying && !audioSource.loop)) {
                            currentStep++;
                            if (elem.animClip != null) {
                                //play the animation for the step
                                //maybe update for next sprint multiple animations to play in sequence
                                hit.transform.gameObject.GetComponent<Animator>().Play(elem.animClip.name);
                            }
                            if (elem.audioClip != null) {
                                //play audio for the step
                                PlayAudio(elem.audioClip);
                            }
                            
                            if (elem.hasSlider) {
                                if (!slider.activeSelf) {
                                    //activate slider and add an EventListener that calls CheckSlider(Step) everytime the slider value changes
                                    slider.SetActive(true);
                                    slider.GetComponent<Slider>().onValueChanged.AddListener(delegate { CheckSlider(elem); });
                                }
                            } else {
                                slider.SetActive(false);
                            }
                            if (elem.hasQuestion) {
                                //send necessary data to the QuestionManager and call Question()
                                qAPanel.GetComponent<QuestionManager>().question = elem.question;
                                qAPanel.GetComponent<QuestionManager>().choices = elem.choices;
                                qAPanel.GetComponent<QuestionManager>().answer = elem.correctChoice;
                                if (elem.audioClip != null) {
                                    Invoke("Question",elem.audioClip.length);
                                }
                                Question();
                            }
                        } else if (hit.transform.gameObject != elem.objectTarget && currentStep == elem.stepOrder && !audioSource.isPlaying) {
                            PlayAudio(elem.missTap);
                        }
                    }
                }
            }
        }
    }

    public void Question() {
        qAPanel.GetComponent<QuestionManager>().Question();
        Debug.Log("yeet");
    }

    public void PlayAudio(AudioClip audio) {
        audioSource.clip = audio;
        audioSource.Play();
    }

    public void PlayIntro() {
        if (!introPlayed) {
            introPlayed = !introPlayed;
            PlayAudio(introAudio);
        }
    }

    //adjusts the position/rotation/scale of the object along one axis depending on the value of the slider.
    public void CheckSlider(Step elem) {
        Vector3 p = elem.objectTarget.transform.localPosition;
        Quaternion r = elem.objectTarget.transform.localRotation;
        Vector3 s = elem.objectTarget.transform.localScale;
        float sliderMultiply = slider.GetComponent<Slider>().value * elem.manipulationMultiplier;
        switch (elem.manipulationType) {
            case ManipulationType.Transform:
                switch (elem.manipulationAxis) {
                    case ManipulationAxis.X:
                        p.x = sliderMultiply;
                        break;
                    case ManipulationAxis.Y:
                        p.y = sliderMultiply;
                        break;
                    case ManipulationAxis.Z:
                        p.z = sliderMultiply;
                        break;
                }
                break;
            case ManipulationType.Rotate:
                switch (elem.manipulationAxis) {
                    case ManipulationAxis.X:
                        GameObject.Find("TextMeshPro Text").GetComponent<TextMeshProUGUI>().text = ("" + r);
                        GameObject.Find("TextMeshPro Text (1)").GetComponent<TextMeshProUGUI>().text = ("" + (slider.GetComponent<Slider>().value * elem.manipulationMultiplier));
                        elem.objectTarget.transform.Rotate(new Vector3(sliderMultiply,r.y,r.z));
                        break;
                    case ManipulationAxis.Y:
                        elem.objectTarget.transform.Rotate(new Vector3(r.x,sliderMultiply,r.z));
                        break;
                    case ManipulationAxis.Z:
                        elem.objectTarget.transform.Rotate(new Vector3(r.x,r.y,sliderMultiply));
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
    }
}