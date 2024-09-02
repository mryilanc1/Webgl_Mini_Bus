using TMPro;
using UnityEngine;

public class Passenger : MonoBehaviour
{
    public bool isPickedUp = false;
    
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

    public void OnPickedUp( SitPoint Sitpoint )
    {
        if (isPickedUp) return;
        isPickedUp = true;
        // Yolcuyu minib�se ta��yoruz
       
       
        transform.SetParent(Sitpoint.transform);
      
        transform.localPosition = Vector3.zero; // Minib�ste belirlenen noktaya ta��
        transform.localEulerAngles = Vector3.zero; // Minib�ste belirlenen noktaya ta��
        animator.SetBool("Sit_Bus", true);
        animator.SetBool("Idle", false);

    }

    public void OnDroppedOff( Transform DoorExitPoint )
    {
        if (!isPickedUp) return;
        this.transform.parent.GetComponent<SitPoint>().IsPicked = false;
        isPickedUp = false;
        // Yolcuyu orijinal konumuna d�nd�r�yoruz
        transform.SetParent(originalParent);
        transform.position = DoorExitPoint.position + new Vector3();
        transform.localEulerAngles = Vector3.zero;
        animator.SetBool("Idle", true);
        animator.SetBool("Sit_Bus", false);

    }
}
