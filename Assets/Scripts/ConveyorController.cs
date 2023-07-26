using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorController : MonoBehaviour
{
    [SerializeField] private GameObject conveyor;

    public void DisableConveyor()
    {
        conveyor.SetActive(false);
    }
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.ConveyorController = this;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
