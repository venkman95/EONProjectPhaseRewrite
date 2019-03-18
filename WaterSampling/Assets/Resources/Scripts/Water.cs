using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    StoryManager storyManager;

    GameObject water;

    private void Awake() {
        storyManager = GameObject.Find("EventSystem").GetComponent<StoryManager>();
        water = GameObject.Find("waterv1");
    }

    private void Start() {
        water.SetActive(false);
    }

    void Update()
    {
        switch (storyManager.currentStep) {
            case 0:
                water.SetActive(true);
                break;
            case 1:
                water.SetActive(false);
                break;
            case 4:
                water.SetActive(true);
                break;
            case 5:
                StartCoroutine(Lerp(100,1));
                break;
            case 10:
                water.SetActive(false);
                break;
        }
    }

    IEnumerator Lerp(float target,float time) {
        float elapsedTime = 0;
        while (elapsedTime < time) {
            water.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(0,water.GetComponent<SkinnedMeshRenderer>().GetBlendShapeWeight(0) + target/time);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
