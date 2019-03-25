using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestionManager : MonoBehaviour
{
    public string question;
    public int answer;
    public string[] choices;
    List<GameObject> answerLayouts;

    private void Awake() {
        answerLayouts.Add(GameObject.Find("AnswerLayout2"));
        answerLayouts.Add(GameObject.Find("AnswerLayout3"));
        answerLayouts.Add(GameObject.Find("AnswerLayout4"));
    }

    public void Question() {
        StartCoroutine(FadeUI(1,true));
    }

    //FadeUI coroutine
    IEnumerator FadeUI(float targetTime, bool fadeIn) {
        List<GameObject> toBeFaded = new List<GameObject>();
        //depending on the length of choices[] add button children from AnswerLayout2/3/4 to toBeFadedIn[]
        switch (choices.Length) {
            case 2:
                foreach (Transform child in answerLayouts[0].transform) {
                    toBeFaded.Add(child.gameObject);
                }
                break;
            case 3:
                foreach (Transform child in answerLayouts[1].transform) {
                    toBeFaded.Add(child.gameObject);
                }
                break;
            case 4:
                foreach (Transform child in answerLayouts[2].transform) {
                    toBeFaded.Add(child.gameObject);
                }
                break;
            default:
                Debug.Log("The number of choices that you have must be between 2-4");
                break;

        }
        float elapsedTime = 0;
        while(elapsedTime < targetTime) {
            if (fadeIn){

            } else {

            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
    //call from OnClick event from AnswerButtons
    public void CheckAnswer() {

    }
}
