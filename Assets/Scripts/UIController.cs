using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI missionText;
    [SerializeField] private GameObject completePanel;

    private void Start()
    {
        GameManager.Instance.UIController = this;
    }

    public void SetMissionText(Mission mission)
    {
        missionText.text = $"Collect {mission.Count} {mission.Product}s";
    }

    public void Complete()
    {
        missionText.gameObject.SetActive(false);
        Invoke(nameof(InvokeComplete), 2.5f);
    }

    void InvokeComplete()
    {
        completePanel.SetActive(true);
    }
}