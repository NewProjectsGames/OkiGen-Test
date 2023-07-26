using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductController : MonoBehaviour
{
    [SerializeField] private Product product;
    public Product Product => product;

    private void OnMouseDown()
    {
        GameManager.Instance.PlayerController.SetTarget(this);
    }
}