using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonNextLevel : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(NextLevel);
    }

    void NextLevel()
    {
        GameManager.Instance.NextLevel();
    }
}