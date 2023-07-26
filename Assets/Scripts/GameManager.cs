using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private UIController _uiController;
    private PlayerController _playerController;
    private SpawnController _spawnController;
    private FxManager _fxManager;
    private ConveyorController _conveyorController;

    public PlayerController PlayerController
    {
        get => _playerController;
        set => _playerController = value;
    }

    public SpawnController SpawnController
    {
        get => _spawnController;
        set => _spawnController = value;
    }

    public UIController UIController
    {
        get => _uiController;
        set
        {
            _uiController = value;
            if (_mission == null)
                GenerateMission();
        }
    }

    public FxManager FxManager
    {
        get => _fxManager;
        set => _fxManager = value;
    }

    public ConveyorController ConveyorController
    {
        get => _conveyorController;
        set => _conveyorController = value;
    }

    public bool IsCompleteLevel
    {
        get => _isCompleteLevel;
        set => _isCompleteLevel = value;
    }

    private bool _isCompleteLevel;
    Mission _mission;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }


    private void GenerateMission()
    {
        _mission = new Mission();
        _mission.Product = (Product)Random.Range(0, Enum.GetValues(typeof(Product)).Length);
        _mission.Count = Random.Range(1, 6);
        _uiController.SetMissionText(_mission);
    }

    public void CheckMissionComplete(Product product)
    {
        if (_mission.Product == product)
        {
            _mission.Count--;
            _uiController.SetMissionText(_mission);

            if (_mission.Count <= 0)
            {
                IsCompleteLevel = true;
                CompleteMission();
            }
        }
    }

    void CompleteMission()
    {
        _conveyorController.DisableConveyor();
        FxManager.ActivateCompleteFx();
        PlayerController.Complete();
        UIController.Complete();
    }

    public void NextLevel()
    {
        SceneLoader.Instance.LoadSceneAsyncCoroutine("OkiGenTest");
        SceneManager.LoadScene(1);
    }
}