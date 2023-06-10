using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TMT_SceneLoadManager : MonoBehaviour
{
    private void Start()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Additive);
    }
}
