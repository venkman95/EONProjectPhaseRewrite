using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

    Image chapter1Sprite;
    Image chapter2Sprite;
    Image chapter3Sprite;
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

    /*
     * MAKE SURE THAT IN THE INSPECTOR YOU SET THE SIZE OF THE TITLE, SUMMARY, SPRITE, AND SCENENUM ARRAYS TO BE THE EXACT SAME AND GREATER THAN 0
     */

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
        chapter1Sprite = GameObject.Find("Chapter1/ChapterIcon").GetComponent<Image>();
        chapter2Sprite = GameObject.Find("Chapter2/ChapterIcon").GetComponent<Image>();
        chapter3Sprite = GameObject.Find("Chapter3/ChapterIcon").GetComponent<Image>();
    }

    public void UpdateText() {
        eventManager.GetComponent<TextManager>().chapterTitles = chapterTitles;
        eventManager.GetComponent<TextManager>().chapterSummaries = chapterSummaries;
        eventManager.GetComponent<TextManager>().chapterSprites = chapterSprites;
        eventManager.GetComponent<TextManager>().chapterSceneNums = chapterSceneNums;

        lowerIndex = eventManager.GetComponent<TextManager>().lowerIndex;
        upperIndex = eventManager.GetComponent<TextManager>().upperIndex;

        if (lowerIndex == 0 && chapterTitles.Length > 3) {
            moreChapters.GetComponent<Button>().interactable = true;
            prevChapters.GetComponent<Button>().interactable = false;
        } else {
            moreChapters.GetComponent<Button>().interactable = false;
            prevChapters.GetComponent<Button>().interactable = true;
        }
        StartCoroutine(Fade(new Color(1,1,1,1),0.5f));
    }

    public void UpdateIndexBounds(int buttonPressed) {
        if (buttonPressed == 1) {
            lowerIndex = 3;
            for(int i = 3; i != 0; i--) {
                if(upperIndex + i > chapterTitles.Length) {
                    upperIndex += i;
                    break;
                }
            }
        } else {
            lowerIndex = 0;
            upperIndex = 2;
        }
    }

    public void LoadScene(int buttonPressed) {
        switch (buttonPressed) {
            case 0:
            if(upperIndex != 2) {
                SceneManager.LoadScene(eventManager.GetComponent<TextManager>().chapterSceneNums[3]);
            } else {
                SceneManager.LoadScene(eventManager.GetComponent<TextManager>().chapterSceneNums[0]);
            }
            break;
            case 1:
            if (upperIndex != 2) {
                SceneManager.LoadScene(eventManager.GetComponent<TextManager>().chapterSceneNums[4]);
            } else {
                SceneManager.LoadScene(eventManager.GetComponent<TextManager>().chapterSceneNums[1]);
            }
            break;
            case 2:
            if (upperIndex != 2) {
                SceneManager.LoadScene(eventManager.GetComponent<TextManager>().chapterSceneNums[5]);
            } else {
                SceneManager.LoadScene(eventManager.GetComponent<TextManager>().chapterSceneNums[2]);
            }
            break;
        }
    }
    IEnumerator Fade(Color color, float time) {
        float elapsedTime = 0;

        while(elapsedTime < time) {
            chapter1Title.color = new Color(color.r, color.g, color.b,Mathf.Lerp(color.a,0,(elapsedTime / time)));
            chapter2Title.color = new Color(color.r,color.g,color.b,Mathf.Lerp(color.a,0,(elapsedTime / time)));
            chapter3Title.color = new Color(color.r,color.g,color.b,Mathf.Lerp(color.a,0,(elapsedTime / time)));
            chapter1Summary.color = new Color(color.r,color.g,color.b,Mathf.Lerp(color.a,0,(elapsedTime / time)));
            chapter2Summary.color = new Color(color.r,color.g,color.b,Mathf.Lerp(color.a,0,(elapsedTime / time)));
            chapter3Summary.color = new Color(color.r,color.g,color.b,Mathf.Lerp(color.a,0,(elapsedTime / time)));
            chapter1Sprite.color = new Color(0.3176471f,0.3176471f,0.3176471f,Mathf.Lerp(color.a,0,(elapsedTime / time)));
            chapter2Sprite.color = new Color(0.3176471f,0.3176471f,0.3176471f,Mathf.Lerp(color.a,0,(elapsedTime / time)));
            chapter3Sprite.color = new Color(0.3176471f,0.3176471f,0.3176471f,Mathf.Lerp(color.a,0,(elapsedTime / time)));

            elapsedTime += Time.deltaTime;
            yield return null;  
        }
        elapsedTime = 0;

        color = new Color(color.r,color.g,color.b,0);

        List<TextMeshProUGUI> tTFI = new List<TextMeshProUGUI>();
        List<Image> iTFI = new List<Image>();

        bool trySuccess = false;

        switch (upperIndex != 2) {
            case true:
            chapter1Title.text = chapterTitles[3];
            tTFI.Add(chapter1Title);
            try { chapter2Title.text = chapterTitles[4]; trySuccess = true; } catch { GameObject.Find("Chapter2").GetComponent<Button>().interactable = false; }
            if (trySuccess) {
                tTFI.Add(chapter2Title);
                trySuccess = !trySuccess;
                GameObject.Find("Chapter2").GetComponent<Button>().interactable = true;
            }
            try { chapter3Title.text = chapterTitles[5]; trySuccess = true; } catch { GameObject.Find("Chapter3").GetComponent<Button>().interactable = false; }
            if (trySuccess) {
                tTFI.Add(chapter3Title);
                trySuccess = !trySuccess;
                GameObject.Find("Chapter3").GetComponent<Button>().interactable = true;
            }
            chapter1Summary.text = chapterSummaries[3];
            tTFI.Add(chapter1Summary);
            try { chapter2Summary.text = chapterSummaries[4]; trySuccess = true; } catch { }
            if (trySuccess) {
                tTFI.Add(chapter2Summary);
                trySuccess = !trySuccess;
            }
            try { chapter3Summary.text = chapterSummaries[5]; trySuccess = true; } catch { }
            if (trySuccess) {
                tTFI.Add(chapter3Summary);
                trySuccess = !trySuccess;
            }
            chapter1Sprite.sprite = chapterSprites[3];
            iTFI.Add(chapter1Sprite);
            try { chapter2Sprite.sprite = chapterSprites[4]; trySuccess = true; } catch { }
            if (trySuccess) {
                iTFI.Add(chapter2Sprite);
                trySuccess = !trySuccess;
            }
            try { chapter3Sprite.sprite = chapterSprites[5]; trySuccess = true; } catch { }
            if (trySuccess) {
                iTFI.Add(chapter3Sprite);
                trySuccess = !trySuccess;
            }
            break;
            case false:
            chapter1Title.text = chapterTitles[0];
            tTFI.Add(chapter1Title);
            try { chapter2Title.text = chapterTitles[1]; trySuccess = true; } catch { GameObject.Find("Chapter2").GetComponent<Button>().interactable = false; }
            if (trySuccess) {
                tTFI.Add(chapter2Title);
                trySuccess = !trySuccess;
                GameObject.Find("Chapter2").GetComponent<Button>().interactable = true;
            }
            try { chapter3Title.text = chapterTitles[2]; trySuccess = true; } catch { GameObject.Find("Chapter3").GetComponent<Button>().interactable = false; }
            if (trySuccess) {
                tTFI.Add(chapter3Title);
                trySuccess = !trySuccess;
                GameObject.Find("Chapter3").GetComponent<Button>().interactable = true;
            }
            chapter1Summary.text = chapterSummaries[0];
            tTFI.Add(chapter1Summary);
            try { chapter2Summary.text = chapterSummaries[1]; trySuccess = true; } catch { }
            if (trySuccess) {
                tTFI.Add(chapter2Summary);
                trySuccess = !trySuccess;
            }
            try { chapter3Summary.text = chapterSummaries[2]; trySuccess = true; } catch { }
            if (trySuccess) {
                tTFI.Add(chapter3Summary);
                trySuccess = !trySuccess;
            }
            chapter1Sprite.sprite = chapterSprites[0];
            iTFI.Add(chapter1Sprite);
            try { chapter2Sprite.sprite = chapterSprites[1]; trySuccess = true; } catch { }
            if (trySuccess) {
                iTFI.Add(chapter2Sprite);
                trySuccess = !trySuccess;
            }
            try { chapter3Sprite.sprite = chapterSprites[2]; trySuccess = true; } catch { }
            if (trySuccess) {
                iTFI.Add(chapter3Sprite);
                trySuccess = !trySuccess;
            }
            break;
        }

        while (elapsedTime < time) {
            foreach(TextMeshProUGUI elem in tTFI) {
                elem.color = new Color(color.r,color.g,color.b,Mathf.Lerp(color.a,1,(elapsedTime / time)));
            }
            foreach (Image elem in iTFI) {
                elem.color = new Color(0.3176471f,0.3176471f,0.3176471f,Mathf.Lerp(color.a,1,(elapsedTime / time)));
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
