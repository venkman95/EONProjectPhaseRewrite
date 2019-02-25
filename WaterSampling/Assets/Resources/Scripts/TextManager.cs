using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextManager : MonoBehaviour
{
    TextMeshProUGUI chapter1Title = GameObject.Find("Chapter1/ChapterTitle").GetComponent<TextMeshProUGUI>();


    [SerializeField]
    public string[] chapterTitles;

    [SerializeField]
    public string[] chapterSummaries;

    public void UpdateText() {
        chapter1Title.text = chapterTitles[0];
    }
}
