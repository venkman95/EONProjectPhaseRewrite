using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class TextManager : MonoBehaviour
{
    TextMeshProUGUI chapter1Title;
    TextMeshProUGUI chapter2Title;
    TextMeshProUGUI chapter3Title;
    TextMeshProUGUI chapter1Summary;
    TextMeshProUGUI chapter2Summary;
    TextMeshProUGUI chapter3Summary;

    GameObject moreChapters;
    GameObject prevChapters;
    GameObject eventManager;

    int lowerIndex = 0;
    int upperIndex = 2;

    int sceneToLoad;

    [SerializeField]
    public string[] chapterTitles;

    [SerializeField]
    public string[] chapterSummaries;

    [SerializeField]
    public Sprite[] chapterSprites;

    [SerializeField]
    public int[] chapterSceneNums;

    public void Awake() {
        chapter1Title = GameObject.Find("Chapter1/ChapterTitle").GetComponent<TextMeshProUGUI>();
        chapter2Title = GameObject.Find("Chapter2/ChapterTitle").GetComponent<TextMeshProUGUI>();
        chapter3Title = GameObject.Find("Chapter3/ChapterTitle").GetComponent<TextMeshProUGUI>();
        chapter1Summary = GameObject.Find("Chapter1/ChapterSummary").GetComponent<TextMeshProUGUI>();
        chapter2Summary = GameObject.Find("Chapter2/ChapterSummary").GetComponent<TextMeshProUGUI>();
        chapter3Summary = GameObject.Find("Chapter3/ChapterSummary").GetComponent<TextMeshProUGUI>();
        moreChapters = GameObject.Find("MoreChapters");
        prevChapters = GameObject.Find("BackChapters");
        eventManager = GameObject.Find("EventSystem");
    }

    public void Start() {
        moreChapters.SetActive(false);
        prevChapters.SetActive(false);
    }

    public void Update() {
        //Debug.Log(moreChapters.activeSelf);
    }

    public void UpdateText() {
        eventManager.GetComponent<TextManager>().chapterTitles = chapterTitles;
        eventManager.GetComponent<TextManager>().chapterSummaries = chapterSummaries;
        eventManager.GetComponent<TextManager>().chapterSprites = chapterSprites;
        eventManager.GetComponent<TextManager>().chapterSceneNums = chapterSceneNums;

        lowerIndex = eventManager.GetComponent<TextManager>().lowerIndex;
        upperIndex = eventManager.GetComponent<TextManager>().upperIndex;

        Debug.Log("lower index is:"+lowerIndex+" chapter titles length is: "+chapterTitles.Length);
        if (lowerIndex == 0 && chapterTitles.Length > 3) {
            Debug.Log("getting in the block");
            moreChapters.SetActive(true);
            prevChapters.SetActive(false);
        } else {
            moreChapters.SetActive(false);
            prevChapters.SetActive(true);
        }
        StartCoroutine(FadeOut(1,0,1));
        //update text values
        //update sprites
        //write something to fade UI in
    }

    public void UpdateIndexBounds(int buttonPressed) {
        if (buttonPressed == 1) {
            lowerIndex += 3;
            for(int i = 3; i != 0; i--) {
                if(upperIndex + i > chapterTitles.Length) {
                    upperIndex += i;
                    break;
                }
            }
        } else {
            lowerIndex -= 3;
            upperIndex -= 3;
        }
        Debug.Log(lowerIndex);
        Debug.Log(upperIndex);
    }

    public void LoadScene(int buttonPressed) {
        foreach(string elem in chapterTitles) {
            switch (buttonPressed) {
                case 0:
                    
                sceneToLoad = chapterSceneNums[buttonPressed + lowerIndex];
                break;
                case 1:
                break;
                case 2:
                break;
            }
        }
        SceneManager.LoadScene(chapterSceneNums[sceneToLoad]);
    }
    IEnumerator FadeOut(float alphaStart, float alphaFinish, float time) {
        float elapsedTime = 0;

        while(elapsedTime < time) {
            Mathf.Lerp(alphaStart,alphaFinish,(elapsedTime / time));
            yield return null;
        }
    }
}
