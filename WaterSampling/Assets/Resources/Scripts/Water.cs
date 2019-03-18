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
