using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestionManager : MonoBehaviour
{
    public string question;
    public int answer;
    public string[] choices;
    List<GameObject> answerLayouts = new List<GameObject>();
    public List<TextMeshProUGUI> textToBeFaded = new List<TextMeshProUGUI>();
    public List<Image> imageToBeFaded = new List<Image>();
    GameObject qAPanel;
    GameObject questionPanel;

    private void Awake() {
        qAPanel = GameObject.Find("QAPanel");
        questionPanel = GameObject.Find("QuestionPanel");
        answerLayouts.Add(GameObject.Find("AnswerLayout2"));
        answerLayouts.Add(GameObject.Find("AnswerLayout3"));
        answerLayouts.Add(GameObject.Find("AnswerLayout4"));
    }

    private void Start() {
        
    }

    public void Question() {
        StartCoroutine(FadeUI(1,true));
    }

    //FadeUI coroutine
    IEnumerator FadeUI(float targetTime, bool fadeIn) {
        textToBeFaded.Add(questionPanel.GetComponentInChildren<TextMeshProUGUI>());
        imageToBeFaded.Add(qAPanel.GetComponent<Image>());
        imageToBeFaded.Add(questionPanel.GetComponent<Image>());

        //depending on the length of choices[] add button children from AnswerLayout2/3/4 to toBeFadedIn[]
        switch (choices.Length) {
            case 2:
                foreach (Transform child in answerLayouts[0].transform) {
                    textToBeFaded.Add(child.GetChild(0).GetComponent<TextMeshProUGUI>());
                    imageToBeFaded.Add(child.GetComponent<Image>());
                }
                break;
            case 3:
                foreach (Transform child in answerLayouts[1].transform) {
                    textToBeFaded.Add(child.GetChild(0).GetComponent<TextMeshProUGUI>());
                    imageToBeFaded.Add(child.GetComponent<Image>());
                }
                break;
            case 4:
                foreach (Transform child in answerLayouts[2].transform) {
                    textToBeFaded.Add(child.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>());
                    imageToBeFaded.Add(child.gameObject.GetComponent<Image>());
                }
                break;
            default:
                Debug.Log("The number of choices that you have must be between 2-4");
                break;

        }
        float elapsedTime = 0;

        //update text boxes

        while(elapsedTime < targetTime) {
            if (fadeIn){
                foreach (TextMeshProUGUI elem in textToBeFaded) {
                    elem.color = new Color(0,0,0,Mathf.Lerp(0,1,(elapsedTime / targetTime)));
                    Debug.Log(elem.text + elem.color);
                }
                foreach (Image elem in imageToBeFaded) {
                    if(elem.gameObject.name == "QAPanel") {
                        elem.color = new Color(0.25f,0.25f,0.25f,Mathf.Lerp(0,0.75f,(elapsedTime / targetTime)));
                    } else {
                        elem.color = new Color(1,1,1,Mathf.Lerp(0,1,(elapsedTime / targetTime)));
                    } 
                }
            } else {
                foreach (TextMeshProUGUI elem in textToBeFaded) {
                    elem.color = new Color(0,0,0,Mathf.Lerp(1,0,(elapsedTime / targetTime)));
                }
                foreach (Image elem in imageToBeFaded) {
                    if (elem.gameObject.name == "QAPanel") {
                        elem.color = new Color(0.25f,0.25f,0.25f,Mathf.Lerp(0.75f,0,(elapsedTime / targetTime)));
                    } else {
                        elem.color = new Color(1,1,1,Mathf.Lerp(1,0,(elapsedTime / targetTime)));
                    }
                }
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
    //call from OnClick event from AnswerButtons
    public void CheckAnswer() {

    }
}
