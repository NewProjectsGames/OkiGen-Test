using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using RootMotion.FinalIK;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour
{
    private Animator _animator;
    [SerializeField] private ArmIK hand;

    [SerializeField] private ProductController target;
    [SerializeField] private float speedHand;
    [SerializeField] private Transform basket;
    [SerializeField] private Transform[] basketPosProduct;
    [SerializeField] private LayerMask pickUpLayer;
    private Transform _pseudoTransform;
    private bool _isPickUp;
    private Vector3 _startPointPseudo;
    private Vector3 _startRotationPlayer;
     private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.PlayerController = this;
        _pseudoTransform = new GameObject().transform;
        _startPointPseudo = _pseudoTransform.position = hand.solver.hand.transform.position;
        _startRotationPlayer = transform.eulerAngles;
        hand.solver.arm.target = _pseudoTransform.transform;
        hand.solver.IKPositionWeight = 0;
        hand.solver.IKRotationWeight = 0;
    }

    public void SetTarget(ProductController productController)
    {
        if (target)
        {
            return;
        }
        hand.solver.IKPositionWeight = 1;
        hand.solver.IKRotationWeight = 1;
        // float dist = Vector3.Distance(productController.transform.position, hand.solver.chest.transform.position) *
        //              1.3f;
        // hand.solver.arm.armLengthMlp = dist <= 1 ? 1 : dist;
        // Tween dd = null;
        // dd = DOTween.To(() => _pseudoTransform.position, pos => _pseudoTransform.position = pos,
        //     productController.transform.position, 1).OnUpdate(delegate
        // {
        //     dd.target = productController.transform.position;
        // });
        //    StartChase();
        //  _pseudoTransform.DOMove(productController.transform.position, 1f);
        target = productController;
        _animator.SetTrigger("PickUp");
        transform.DOLookAt(target.transform.position, .5f,AxisConstraint.Y);
     }


//AnimationEvent
    public void PickUp()
    {
        target.transform.SetParent(hand.solver.hand.transform);
        target.transform.localPosition = Vector3.zero;
        target.GetComponent<Rigidbody>().isKinematic = true;
        _isPickUp = true;
        DOTween.To(() => hand.solver.arm.armLengthMlp, x => hand.solver.arm.armLengthMlp = x, 1, .5f);
        DOTween.To(() => hand.solver.IKPositionWeight, x => hand.solver.IKPositionWeight = x, 0, .3f);
        DOTween.To(() => hand.solver.IKRotationWeight, x => hand.solver.IKRotationWeight = x, 0, .3f);
        GameManager.Instance.FxManager.SpawnPickUpFx(target.transform.position);
    }

//AnimationEvent
    public void DropToBasket()
    {
        GameManager.Instance.CheckMissionComplete(target.Product);
        var rb = target.GetComponent<Rigidbody>();
        var col = target.GetComponent<Collider>();
        col.isTrigger = true;
        target.transform.SetParent(basket);
        target.transform.localPosition = basketPosProduct[Random.Range(0, basketPosProduct.Length)].localPosition;
        target.transform.DOScale(transform.lossyScale / 1.3f, .5f)
            .OnComplete(() =>
            {
                col.isTrigger = false;
                rb.angularDrag = 5;
                rb.drag = 5;
                rb.isKinematic = false;
                rb.constraints = RigidbodyConstraints.FreezePositionY;
                col.gameObject.layer = LayerMask.NameToLayer("PickupItem");
            });
        GameManager.Instance.FxManager.SpawnPickUpFx(target.transform.position);
        _isPickUp = false;
        target = null;
        GameManager.Instance.FxManager.SpawnFloatText(transform.position);
        _pseudoTransform.position = _startPointPseudo;
        transform.DORotate(_startRotationPlayer, .5f);
        //    Destroy(target);
    }

    public void Complete()
    {
        _animator.SetTrigger("Complete");
    }
    // Update is called once per frame
    void Update()
    {
        if (target && !_isPickUp)
        {
            Vector3 direction = (target.transform.position - _pseudoTransform.position);
            _pseudoTransform.position += direction * speedHand * Time.deltaTime;

            float dist = Vector3.Distance(transform.position, _pseudoTransform.position) *
                         1.3f;
            hand.solver.arm.armLengthMlp = dist <= 1 ? 1 : dist;
        }
        // float dist = Vector3.Distance(target.position, hand.solver.chest.transform.position) * 1.3f;
        // hand.solver.arm.armLengthMlp = dist <= 1 ? 1 : dist;
        // if (Input.GetMouseButton(0))
        // {
        //     Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //     RaycastHit hit;
        //     if (Physics.Raycast(ray, out hit, 100))
        //     {
        //         Debug.Log(hit.transform.name);
        //         Debug.Log("hit");
        //         target.position = hit.point;
        //         hand.solver.arm.target = target;
        //
        //         //hand.transform.position = hit.point;
        //     }
        // }
    }
}