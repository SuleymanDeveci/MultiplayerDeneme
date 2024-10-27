using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    public Transform Target;                      // kameranin bakacagi hedefin tranform verisini tutar  (Atamasi PlayerMovement Scripti 26. Satirda yapiliyor)
    public float VerticalSensitivity = 2f;        // Kamera bakisi icin dikey hassasiyet
    public float HorizontalSensitivity = 6f;      // Kamera bakisi icin yatay hassasiyet

    private float verticalRotation;               // Kameranin dikeyde ne kadar hareket edecegini tutar
    private float horizontalRotation;             // Kameranin yatayda ne kadar hareket edecegini tutar

    private void LateUpdate()
    {
        if(Target == null)     // eger Target tanimlanmamis ise return yapar, bu sayede target tanimlnmadan onceki gecen surede hata mesaji firlatmaz
        {
            return;
        }

        transform.position = new Vector3(Target.position.x, Target.position.y + 0.5f, Target.position.z);  // kamera'nin pozisyonunu targetin bulundugu pozisyona esitliyoruz boylece FPS bakis acisi elde ediyoruz

        float mouseX = Input.GetAxis("Mouse X");   // oyuncudan mouse X degerini aliyoruz
        float mouseY = Input.GetAxis("Mouse Y");   // oyuncudan mouse Y degerini aliyoruz

        verticalRotation -= mouseY * VerticalSensitivity;        // aldigimiz inputu sensitiviy,ty degeri ile carpip verticalRotation degiskenine atiyoruz
        horizontalRotation += mouseX * HorizontalSensitivity;    // aldigimiz inputu sensitiviy,ty degeri ile carpip horizontalRotation degiskenine atiyoruz

        transform.rotation = Quaternion.Euler(verticalRotation, horizontalRotation, 0);  // hesapladigimiz degiskenleri kameranin rotationuna uyguluyoruz
    }
}
