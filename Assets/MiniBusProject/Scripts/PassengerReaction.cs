using UnityEngine;

public class PassengerReaction : MonoBehaviour
{
    public Transform leftHandTarget;
    public Transform rightHandTarget;
    public Transform leftFootTarget;
    public Transform righFootTarget;
    public Rigidbody vehicleRigidbody;
    public float maxTiltAngle = 15f; // Maximum tilt angle for passenger reaction
    public Animator animator;

    public float IK = 0f;

    private Vector3 previousVelocity;
    private float tiltAngle;


    
    void Start()
    {
        // Baþlangýçtaki minibüs hýzýný al
       
    //    vehicleRigidbody = RCC_SceneManager.Instance.activePlayerVehicle.rigid;
        previousVelocity = vehicleRigidbody.velocity;

    }

    void Update()
    {
        // Þimdiki ve önceki velocity arasýndaki farký bul (ani hýzlanma veya frenleme için)
        Vector3 currentVelocity = vehicleRigidbody.velocity;
        Vector3 deltaVelocity = currentVelocity - previousVelocity;

        // Fren ya da hýzlanmayý tespit et (z eksenini dikkate alarak)
        float deltaSpeed = deltaVelocity.z;

        // Frenleme olduðunda yolcuyu öne eð, hýzlanmada arkaya eð
        tiltAngle = Mathf.Clamp(-deltaSpeed * 5f, -maxTiltAngle, maxTiltAngle);
        transform.localRotation = Quaternion.Euler(tiltAngle, 0, 0);

        if(IK >0.95)
        {
         
            righFootTarget.parent = vehicleRigidbody.gameObject.transform;
            leftFootTarget.parent = vehicleRigidbody.gameObject.transform;



        }
        // IK hedeflerini güncelleyin
        animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, IK);
        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, IK);
        animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandTarget.position);
        animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandTarget.position);
        animator.SetIKPosition(AvatarIKGoal.RightFoot, righFootTarget.position);
        animator.SetIKPosition(AvatarIKGoal.LeftFoot, leftFootTarget.position);


        // Þimdiki velocity'yi sakla
        previousVelocity = currentVelocity;
    }
}
