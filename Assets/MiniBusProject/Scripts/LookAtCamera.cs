using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    public Camera mainCamera; // Ana kameray� atanacak alan

    private void Start()
    {
        mainCamera = Camera.main;
    }


    private void LateUpdate()
    {
        if (mainCamera != null)
        {
            // Bu nesnenin y�n�n� kameraya do�ru �evirmek i�in d�nd�rme i�lemi
            Vector3 direction = mainCamera.transform.position - transform.position;
            direction.y = 0; // Y eksenindeki rotay� s�f�rlayarak dikey a��lar� kald�r�r
            Quaternion rotation = Quaternion.LookRotation(direction);
            rotation *= Quaternion.Euler(0f, 180f, 0f);
            transform.rotation = rotation;
        }
        else
        {
            Debug.LogError("Ana kamera atanmad�.");
        }
    }

}
