using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TMT_SoundDeactive : MonoBehaviour
{
    AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        StartCoroutine(Deactive());
    }

    IEnumerator Deactive()
    {
        yield return new WaitUntil(() => !audioSource.isPlaying);
        gameObject.SetActive(false);
    }
}
