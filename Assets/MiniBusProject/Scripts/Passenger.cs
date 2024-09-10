using TMPro;
using UnityEngine;

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
