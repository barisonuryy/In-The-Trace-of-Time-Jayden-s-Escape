using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAiming : MonoBehaviour
{
    // Start is called before the first frame update
    public Camera playerCamera; // Oyuncunun kamerasını referans alıyoruz
    public Transform weaponTransform; // Silahın transform referansı
    public Transform shoulderTransform; // Omuzun transform referansı
    public Vector3 aimOffset; // Omuz için bir offset ekleyebiliriz

    void Update()
    {
        AimWeaponAtMouse();
    }

    void AimWeaponAtMouse()
    {
        // Mouse'un ekran üzerindeki pozisyonunu al
        Vector3 mousePosition = Input.mousePosition;

        // Mouse pozisyonunu kullanarak bir ray oluştur
        Ray ray = playerCamera.ScreenPointToRay(mousePosition);

        // Ray'in çarpma bilgisini tutmak için bir RaycastHit oluştur
        RaycastHit hit;

        // Raycast yaparak ray'in neye çarptığını kontrol et
        if (Physics.Raycast(ray, out hit))
        {
            // Çarptığı noktayı al
            Vector3 hitPoint = hit.point;

            // Omuz pozisyonunu al
            Vector3 shoulderPosition = shoulderTransform.position;

            // Silahın omuz pozisyonundan çarptığı noktaya bakmasını sağla
            Vector3 targetDirection = (hitPoint - shoulderPosition).normalized + aimOffset;

            // Silahı hedef yönüne döndür
            weaponTransform.rotation = Quaternion.LookRotation(targetDirection);
        }
    }
}
