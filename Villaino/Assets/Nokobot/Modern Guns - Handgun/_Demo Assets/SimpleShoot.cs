//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//[AddComponentMenu("Nokobot/Modern Guns/Simple Shoot")]
//public class SimpleShoot : MonoBehaviour
//{
//    [Header("Prefab Refrences")]
//    public GameObject bulletPrefab;
//    public GameObject casingPrefab;
//    public GameObject muzzleFlashPrefab;

//    [Header("Location Refrences")]
//    [SerializeField] private Animator gunAnimator;
//    [SerializeField] private Transform barrelLocation;
//    [SerializeField] private Transform casingExitLocation;

//    [Header("Settings")]
//    [Tooltip("Specify time to destory the casing object")] [SerializeField] private float destroyTimer = 2f;
//    [Tooltip("Bullet Speed")] [SerializeField] private float shotPower = 500f;
//    [Tooltip("Casing Ejection Speed")] [SerializeField] private float ejectPower = 150f;


//    void Start()
//    {
//        if (barrelLocation == null)
//            barrelLocation = transform;

//        if (gunAnimator == null)
//            gunAnimator = GetComponentInChildren<Animator>();
//    }

//    void Update()
//    {
//        //If you want a different input, change it here
//        if (Input.GetButtonDown("Fire1"))
//        {
//            //Calls animation on the gun that has the relevant animation events that will fire
//            gunAnimator.SetTrigger("Fire");
//        }
//    }


//    //This function creates the bullet behavior
//    void Shoot()
//    {
//        if (muzzleFlashPrefab)
//        {
//            //Create the muzzle flash
//            GameObject tempFlash;
//            tempFlash = Instantiate(muzzleFlashPrefab, barrelLocation.position, barrelLocation.rotation);

//            //Destroy the muzzle flash effect
//            Destroy(tempFlash, destroyTimer);
//        }

//        //cancels if there's no bullet prefeb
//        if (!bulletPrefab)
//        { return; }

//        // Create a bullet and add force on it in direction of the barrel
//        Instantiate(bulletPrefab, barrelLocation.position, barrelLocation.rotation).GetComponent<Rigidbody>().AddForce(barrelLocation.forward * shotPower);

//    }

//    //This function creates a casing at the ejection slot
//    void CasingRelease()
//    {
//        //Cancels function if ejection slot hasn't been set or there's no casing
//        if (!casingExitLocation || !casingPrefab)
//        { return; }

//        //Create the casing
//        GameObject tempCasing;
//        tempCasing = Instantiate(casingPrefab, casingExitLocation.position, casingExitLocation.rotation) as GameObject;
//        //Add force on casing to push it out
//        tempCasing.GetComponent<Rigidbody>().AddExplosionForce(Random.Range(ejectPower * 0.7f, ejectPower), (casingExitLocation.position - casingExitLocation.right * 0.3f - casingExitLocation.up * 0.6f), 1f);
//        //Add torque to make casing spin in random direction
//        tempCasing.GetComponent<Rigidbody>().AddTorque(new Vector3(0, Random.Range(100f, 500f), Random.Range(100f, 1000f)), ForceMode.Impulse);

//        //Destroy casing after X seconds
//        Destroy(tempCasing, destroyTimer);
//    }

//}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class SimpleShoot : MonoBehaviour
{

    public GameObject bulletPrefab;
    public GameObject casingPrefab;
    public GameObject muzzleFlashPrefab;
    public Transform barrelLocation;
    public Transform casingExitLocation;


    public float shotPower = 100f;

    public bool isGrab = false;

    public AudioClip fireClip; //총 발사 사운드 클립
    AudioSource fireAudio;     //핸드건에 추가한 오디오소스컴포넌트를 담을 변수

    public HandState currentGrab;



    void Start()
    {
        if (barrelLocation == null)
            barrelLocation = transform;

        fireAudio = GetComponent<AudioSource>();
        currentGrab = HandState.NONE;
    }

    void Update()
    {

    }

    public void grabGun()
    {
        isGrab = true;
    }

    public void dropGun()
    {
        isGrab = false;
    }

    public void SetGraspState(HandState state)
    {
        currentGrab = state;
    }

    public void SetGraspNONE()
    {
        if (!GetComponent<XRGrabInteractable>().isSelected)
            currentGrab = HandState.NONE;

    }

    public void SetGraspLEFT()
    {
        currentGrab = HandState.LEFT;
    }

    public void SetGraspRIGHT()
    {
        currentGrab = HandState.RIGHT;
    }

    public void Shoot()
    {
        if (isGrab == true)
        {
            GameObject tempFlash;
            Instantiate(bulletPrefab, barrelLocation.position, barrelLocation.rotation).GetComponent<Rigidbody>().AddForce(barrelLocation.forward * shotPower);
            tempFlash = Instantiate(muzzleFlashPrefab, barrelLocation.position, barrelLocation.rotation);


            fireAudio.PlayOneShot(fireClip);
        }
    }

    void CasingRelease()
    {
        GameObject casing;
        casing = Instantiate(casingPrefab, casingExitLocation.position, casingExitLocation.rotation) as GameObject;
        casing.GetComponent<Rigidbody>().AddExplosionForce(550f, (casingExitLocation.position - casingExitLocation.right * 0.3f - casingExitLocation.up * 0.6f), 1f);
        casing.GetComponent<Rigidbody>().AddTorque(new Vector3(0, Random.Range(100f, 500f), Random.Range(10f, 1000f)), ForceMode.Impulse);
    }


}

