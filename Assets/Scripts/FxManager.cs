using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class FxManager : MonoBehaviour
{
    private Camera _camera;
    [SerializeField] private Transform canvas;
    [SerializeField] private GameObject prefabFloatText;
    [SerializeField] private GameObject prefabPickUpFx;
    [SerializeField] private GameObject prefabDropFx;
    [SerializeField] private GameObject completeFx;
    [SerializeField] private GameObject cameraFinish;
    public void SpawnFloatText(Vector3 pos)
    {
        var ft = Instantiate(prefabFloatText, canvas);
        ft.transform.position = _camera.WorldToScreenPoint(pos + Vector3.up * 2);
        ft.transform.DOMoveY(ft.transform.position.y + 100, 1);
        ft.transform.DOScale(0, .6f).SetDelay(.4f).OnComplete(() => Destroy(ft));
    }

    public void SpawnPickUpFx(Vector3 pos)
    {
        var fx = Instantiate(prefabPickUpFx, pos, Quaternion.identity);
        Destroy(fx,3);
    }

    public void SpawnDropFx(Vector3 pos)
    {
        var fx = Instantiate(prefabDropFx, pos, Quaternion.identity);
        Destroy(fx,3);
    }
public void ActivateCompleteFx()
    {
        cameraFinish.SetActive(true);
        completeFx.SetActive(true);
    }
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.FxManager = this;
        _camera = Camera.main;
    }

 
}