using UnityEngine;
using Leap;
using Leap.Unity;

public class FighterController : MonoBehaviour
{

    LeapProvider LeapProviderObject; // Allows use of Leapmotion Sensor's available methods
    private ManagerGame managerGameObject; // Allows use of the methods and variables in the managerGame.cs file

    private void Start()
    {
        managerGameObject = GameObject.FindGameObjectWithTag("ManagerGame").GetComponent<ManagerGame>();
        LeapProviderObject = FindObjectOfType<LeapProvider>() as LeapProvider;
    }

    [System.Serializable]
    public class Boundary
    {
        public float xMin, xMax, zMin, zMax; // Controls where the ship is allowed to be in the game
    }

    public float speed, tilt;
    public Boundary boundary;

    public GameObject shot; // The missile that is fired from the ship
    public Transform shotSpawn; // Location where the missiles are fired from (always just in front of the nose of the ship)

    float nextFire = 0.0f;
    public float fireRate;
    bool shotHasReset = true; // Prevents users from just keeping their hand closed

    void Update()
    {
        if (managerGameObject.getGameIsInProgress() && !managerGameObject.getGameIsPaused() || managerGameObject.getTutorialMode() || managerGameObject.getIsWarping())
        {
            Frame frame = LeapProviderObject.CurrentFrame;
            transform.position = new Vector3(transform.position.x, transform.position.y, 2.5f);
            foreach (Hand hand in frame.Hands)
            {
                Quaternion rotation = Quaternion.Euler(hand.Basis.rotation.ToQuaternion().eulerAngles.z, 270, 0);
                transform.rotation = rotation;
                transform.position = new Vector3(
                    Mathf.Clamp(hand.PalmPosition.x * 50, boundary.xMin, boundary.xMax),
                    0.0f,
                    Mathf.Clamp(2.5f, 2, 4)
                );
                if (hand.GrabStrength > 0.6 && Time.time > nextFire && shotHasReset)
                {
                    nextFire = Time.time + fireRate;
                    Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
                    shotHasReset = false;
                }
                if (hand.GrabStrength < 0.3)
                {
                    shotHasReset = true;
                }
                break;
            }

#if UNITY_EDITOR
            if (Input.GetKey(KeyCode.RightArrow) && transform.position.x < 7)
            {
                transform.position = new Vector3(transform.position.x + .3f, transform.position.y, transform.position.z);
            }
            if (Input.GetKey(KeyCode.LeftArrow) && transform.position.x > -7)
            {
                transform.position = new Vector3(transform.position.x - .3f, transform.position.y, transform.position.z);
            }
            if (Input.GetKeyDown(KeyCode.Space)) Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
#endif
        }
    }
}