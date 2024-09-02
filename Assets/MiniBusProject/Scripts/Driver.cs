using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using DG.Tweening;

public class Driver : MonoBehaviour
{
    public Animator AnimatorDriver;
    public RigBuilder RigBuilder;
    public Transform right;
    public Transform left;
    public Transform gearhand;
    public Transform ikTargetLeftHand;
    public Transform ikTargetRightHand;
    public Transform steeringWheelTransform;
    public float gearInput = 0f;


    public RCC_CarControllerV3 CarController
    {
        get
        {
            if (_carController == null)
                _carController = GetComponentInParent<RCC_CarControllerV3>();
            return _carController;
        }
    }
    private RCC_CarControllerV3 _carController;
    void Start()
    {
        AnimatorDriver.SetBool("Sit_Bus", true);
    }

    void Update()
    {

        //  If changing gear.
        if (CarController.changingGear)
            gearInput = 1f;
        else
            gearInput -= Time.deltaTime * 5f;

        //  Clamping gear input.
        if (gearInput < 0)
            gearInput = 0f;
        if (gearInput > 1)
            gearInput = 1f;


      

        // Direksiyonun dünya uzayýndaki rotasyonunu al
        Quaternion steeringWheelRotation = steeringWheelTransform.rotation;

        // ÝK hedeflerinin rotasyonunu güncelle
        /*  ikTargetLeftHand.rotation = left.rotation;
          ikTargetRightHand.rotation = right.rotation;
  */



        ikTargetLeftHand.position = left.position;
        ikTargetLeftHand.DORotateQuaternion(left.rotation, 0.3f);

        if (gearInput > .5f)
        {
            ikTargetRightHand.DOLocalMove(gearhand.localPosition, 0.3f);
            ikTargetRightHand.DORotateQuaternion(gearhand.rotation, 0.3f);

        }
        else
        {
            ikTargetRightHand.position = right.position;
            ikTargetRightHand.DORotateQuaternion(right.rotation, 0.3f);
        }



    }

}
