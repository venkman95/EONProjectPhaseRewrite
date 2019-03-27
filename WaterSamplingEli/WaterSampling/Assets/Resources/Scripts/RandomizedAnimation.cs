using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizedAnimation : MonoBehaviour
{
    public GameObject Object;
    public StoryManager storymanager;
    public AnimationClip clipA;
    public AnimationClip clipB;

    public int randomint;
    public int steptoactivate;
    private bool isrunning = false;

    
    // Update is called once per frame
    void Update()
    {

        if (storymanager.currentStep + 1 == steptoactivate && isrunning == false )
        {
            isrunning = true;
            randomint = Random.Range(0, 4);
            if (randomint == 3)
            {
                Object.GetComponent<Animator>().Play(clipA.name);
            }
            if (randomint < 3)
            {
                Object.GetComponent<Animator>().Play(clipB.name);
            }


        }
    }
}
