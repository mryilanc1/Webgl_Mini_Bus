using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Passenger : MonoBehaviour
{
    public bool isPickedUp = false;
    public bool IsStanding = false;

    private Transform originalParent;
    [SerializeField]
    private Animator animator;
    
    public Transform targetStopTransform;
    public StopArea TargetStopArea;
    public TextMeshProUGUI TextTargetArea;

    public TwoBoneIKConstraint RighHand_IK;
    public TwoBoneIKConstraint LeftHand_IK;


    private void Start()
    {
        originalParent = transform.parent;
        TextTargetArea.text = TargetStopArea.ToString();
      
    }

    public void OnPickedUp( SitPoint Sitpoint,bool IsStanding )
    {
        if (isPickedUp) return;
        isPickedUp = true;
        // Yolcuyu minibüse taþýyoruz
       
       
        transform.SetParent(Sitpoint.transform);
      
        transform.localPosition = Vector3.zero; // Minibüste belirlenen noktaya taþý
        transform.localEulerAngles = Vector3.zero; // Minibüste belirlenen noktaya taþý
        if(IsStanding)
        {
            animator.SetBool("Standing", true);
            animator.SetBool("Idle", false);
            animator.SetBool("Crouching", false);

            if(Sitpoint.leftHandIKTarget != null)
            {
                LeftHand_IK.data.target.DOMove(Sitpoint.leftHandIKTarget.transform.position,0.25f);
                LeftHand_IK.data.target.DORotateQuaternion(Sitpoint.leftHandIKTarget.transform.rotation, 0.25f);
                if (Sitpoint.leftHandIKHint != null)
                {
                    LeftHand_IK.data.hint.DOMove(Sitpoint.leftHandIKHint.transform.position, 0.25f);
                }
                LeftHand_IK.weight = 1f;

            }

            if (Sitpoint.RightHandIKTarget != null)
            {
               
                RighHand_IK.data.target.DOMove(Sitpoint.RightHandIKTarget.transform.position, 0.25f);
                RighHand_IK.data.target.DORotateQuaternion(Sitpoint.RightHandIKTarget.transform.rotation, 0.25f);
                if (Sitpoint.RightHandIKHint != null)
                {
                    RighHand_IK.data.hint.DOMove(Sitpoint.RightHandIKHint.transform.position, 0.25f);
                }
                RighHand_IK.weight = 1;
            }
        }
        else
        {
            animator.SetBool("Sit_Bus", true);
            animator.SetBool("Idle", false);
            animator.SetBool("Standing", false);
            animator.SetBool("Crouching", false);
        }

    }

    public void OnDroppedOff( Transform DoorExitPoint )
    {
        if (!isPickedUp) return;
        this.transform.parent.GetComponent<SitPoint>().IsPicked = false;
        isPickedUp = false;
        IsStanding = false;
        // Yolcuyu orijinal konumuna döndürüyoruz
        transform.SetParent(originalParent);
        transform.position = DoorExitPoint.position + new Vector3();
        transform.localEulerAngles = Vector3.zero;
        animator.SetBool("Idle", true);
        animator.SetBool("Sit_Bus", false);
        animator.SetBool("Standing", false);
        animator.SetBool("Crouching", false);
    }

    public void OnCrouching(bool IsCrouched)
    {
        animator.SetBool("Crouching", IsCrouched);
        animator.SetBool("Standing", !IsCrouched);

    }

    
}
