using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    public Camera mainCamera; // Ana kamerayý atanacak alan

    private void Start()
    {
        mainCamera = Camera.main;
    }


    private void LateUpdate()
    {
        if (mainCamera != null)
        {
            // Bu nesnenin yönünü kameraya doðru çevirmek için döndürme iþlemi
            Vector3 direction = mainCamera.transform.position - transform.position;
            direction.y = 0; // Y eksenindeki rotayý sýfýrlayarak dikey açýlarý kaldýrýr
            Quaternion rotation = Quaternion.LookRotation(direction);
            rotation *= Quaternion.Euler(0f, 180f, 0f);
            transform.rotation = rotation;
        }
        else
        {
            Debug.LogError("Ana kamera atanmadý.");
        }
    }

}
