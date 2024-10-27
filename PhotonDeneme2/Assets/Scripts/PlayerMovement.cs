using Fusion;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    private CharacterController _controller;  // unity'nin kendi bunyesinde bulunan karakter kontrolcusunu tanimliyoruz
    public Camera Camera;                     // Karakterin uzerindeki FPS kamerasini tanimliyoruz

    private Vector3 _velocity;                // karakterin hiz ile ilgili degisikliklerini bu degisken uzerinde yapip saonrasinda karaktere uyguluyoruz
    private bool _jumpPressed;                // hoplama tusuna basilip basilmadigi bilgisini tutan boolean degisken
    //***** Hoplama bilgisini boolean degisken icerisinde tutmamizin sebebi tusa bastigimiz anin butun clientlerde ayni anda algilanmasini saglamak. biz tusa basip basmadigimizi update metodunda kontol ediyoruz
    // eger tusa basmis isek _jumpPressed degiskenini true yapip, herkeste ayni anda ve ayni sayida cagirilan FixedUpdateNetwork() metodu icerisinde eger _jumpPressed = true ise hoplama islemini yapiyoruz
    // bu sayede hoplama islemimiz biz dahil odadaki herkes tarafindan ayni anda gorunuyor

    public float PlayerSpeed = 2f;            // karakterin hiz degiskeni
    public float JumpForce = 5f;              // karakterin hoplama gucu degiskeni
    public float GravityValue = -9.81f;       // karaktere uygulanacak olan yer cekimi kuvveti'nin degeri

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();  // _controller degiskeninin atamasini yapiyoruz
    }

    public override void Spawned() // Unity Awake metodunun, Photon versiyonu
    {
        if (HasStateAuthority)  // eger bu PlayerMovement scriptinin bulundugu networkRunner, yerel networkRunner ise true doner  (yani playerin kendisi ise true doner)
        {
            Camera = Camera.main;   // Camera nesnesine, main Camera'yi atiyoruz
            Camera.GetComponent<FirstPersonCamera>().Target = transform;  // Camera'nin icerisindeki FirsPersonCamera componentinin Target degiskenine, karakterin transformunu atiyoruz
            // bu sayede Awake aninda kamera FPS pozisyonuna geciyor, kameranin sonraki hareketlerini ise FirsPersonCamera scriptinin icerisinde Mouse inputlari ile kontrol ediyoruz
        }
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump"))  // burada Jump butonuna basildigi bilgisini oyuncudan aliyoruz ama henuz servere bildirmiyoruz, servere bu bilgi FixedUpdateNetwork metodu icerisinde gidiyor
        {
            _jumpPressed = true;
        }
    }

    public override void FixedUpdateNetwork() // bu metod tum clientlerde ayni anda calisir, o yuzden karakter hareketi gibi herkeste ayni anda gorunmesi gereken kodlar, burada calistirilir
    {
        if (_controller.isGrounded) // eger karakter yere temas ediyor ise
        {
            _velocity = new Vector3(0, -1, 0); // karakterin tas, engel gibi ufak nesnelerin uzerinden sekip surekli yukariya dogru gitmemesi icin karaktere asagi dogru surekli ufak bir guc uygulaniyor
        }

        Quaternion cameraRotationY = Quaternion.Euler(0, Camera.transform.rotation.eulerAngles.y, 0);   // Kameranin Y ekseninde ne tarafa bakiyor ise "W" tusuna basinca karakterin o tarafa gitmesi icin,
            // kameranin Y ekseni bilgisini aliyoruz, Y eksenini almamizin sebebi, kameranin saga ve sola bakma hareketi Kameranin rotationunda Y ekseninin degerini degsitiriyor olmasý

        transform.rotation = Quaternion.Euler(0, Camera.transform.rotation.eulerAngles.y, 0);  // Kamera y ekseninde ne tarafa bakiyor ise karakterin o tarafa bakmasini saglayan kod

        Vector3 move = cameraRotationY * new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * PlayerSpeed * Runner.DeltaTime;  // cameraRotation: karakterin, kameranin baktigi yone gitmesini sagliyor
            // diger kodlarda kullanicidan alinan inputlar dogrultusunda karakterin hareket etmesini sagliyor

        _velocity.y += GravityValue * Runner.DeltaTime;  // karaktere manual olarak yer cekimi uygulaniyor 
        if(_jumpPressed && _controller.isGrounded)  // eger hoplama tusuna basilmis ise ve karakter yere temas ediyor ise
        {
            _velocity.y += JumpForce;  // _velocity.y degiskenine JumpForce ekle
        }
        _controller.Move(move + _velocity * Runner.DeltaTime); // yukarida hesaplanan move ve _velocity degiskenlerinin karaktere uygulandigi yer

        _jumpPressed = false; // hoplama tusunu sifirla
    }
}
