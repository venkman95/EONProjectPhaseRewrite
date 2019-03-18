using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    public GameObject storyManager;

    public GameObject water;

    private void Awake() {
        storyManager = this.gameObject;
        water = GameObject.Find("waterv1");
    }

    private void Start() {
        water.SetActive(false);
    }

    void Update() {
        switch (storyManager.GetComponent<StoryManager>().currentStep) {
            case 1:
                water.SetActive(true);
                break;
            case 2:
                water.SetActive(false);
                break;
            case 5:
                water.SetActive(true);
                break;
            case 6:
                StartCoroutine(Lerp(100,1));
                break;
            case 11:
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
