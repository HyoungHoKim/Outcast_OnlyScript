using System.Collections;
using UnityEngine;
//using UnityScript.Scripting.Pipeline;

public class RevolverHolder : MonoBehaviour
{
    public Pistol_02 revolverParent;

    public Collider[] gunColliders;

    //Bullet 
    public int numberOfChambers = 6;
    public GameObject bulletPrefab;
    public float bulletPlacementRadius = 0.0102f;
    public float bulletForwardAxisOffset = 0.0092f;

    // Bullet Status
    private GameObject[] bullets;
    private bool[] bulletSpent;
    public Transform insertPos;

    //Holder Rotaion : Open, Close 
    public Quaternion openRot;
    public Quaternion closeRot;
    public float openSpeed = 1000f;
    public float openDurationReloadNeeded = 3f;
    public float openDurationReloadNotNeeded = 0.65f;
    public float minStayOpenDuration = 0.65f;
    public bool isOpen = false;
    private bool isInsertMag = false;

    // Auto-Close Info
    private float openStartTime;
    private float stayOpenDuration;
    private float defaultOpenSpinSpeed;

    // Rotation while open
    private float revolverClickAngle = 0f;
    private int currentRevolvIndex = 0;
    private int shellEjectionIndex = -1;

    // Rotation while whooting
    private Quaternion rotationTarget;
    private bool isAnimating = false;
    private Coroutine positionCoroutine;

    // States while open
    private bool shouldSpin = false;
    private bool canEjectShells = false;

    //Spin part 
    public Transform SpinPart;
    public float rotationSpeed = 0.1f;
    public float minRotationSpeedForReloading = 200f;
    public float shellEjectionForce = 1f;

    //Sound 
    public AudioClip revolverOpenClip;
    public AudioClip revolverCloseClip;
    private AudioSource openCloseAS;


    // Start is called before the first frame update
    void Start()
    {
        this.closeRot = this.transform.localRotation;
        bullets = new GameObject[numberOfChambers];
        bulletSpent = new bool[numberOfChambers];
        ReloadBarrel();

        defaultOpenSpinSpeed = minRotationSpeedForReloading;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.shouldSpin && minRotationSpeedForReloading >= 1)
        {
            SpinPart.transform.localRotation = SpinPart.transform.localRotation * Quaternion.AngleAxis(Time.deltaTime * -minRotationSpeedForReloading, Vector3.right);

            minRotationSpeedForReloading -= 10f;

            //Debug.Log($"minRotationSpeedForReloading : {minRotationSpeedForReloading}");

            int index = (int)Mathf.Floor((Time.time - this.openStartTime) / 0.135f);
            index = index % this.numberOfChambers;
            if (shellEjectionIndex != index && canEjectShells)
            {
                EjectShell(index);
            }
            shellEjectionIndex = index;
        }
        else if (this.isAnimating)
        {
            SpinPart.localRotation = Quaternion.Lerp(SpinPart.transform.localRotation, this.rotationTarget, rotationSpeed);
            float curAngle = Quaternion.Angle(SpinPart.transform.localRotation, this.rotationTarget);
            if (curAngle < 1)
            {
                this.isAnimating = false;
            }
        }
    }

    public void Revolve()
    {
        Debug.Log("Spin!");
        bulletSpent[currentRevolvIndex] = true;
        this.rotationTarget = RotationForIndex(++currentRevolvIndex);
        this.isAnimating = true;

        if (currentRevolvIndex > numberOfChambers - 1)
        {
            currentRevolvIndex = 0;
        }
    }

    #region Bullet : 
    // Bullet Respawn 
    void ReloadBarrel()
    {

        //Debug.Log("Spawn Bullet");

        foreach(GameObject bullet in bullets)
        {
            if(bullet != null)
            {
                Destroy(bullet);
            }
        }

        for(int i = 0; i < numberOfChambers; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, SpinPart.transform);
            float degrees = AngleForIndex(i) + 30f;
            float x = bulletPlacementRadius * Mathf.Cos(degrees * Mathf.Deg2Rad);
            float y = bulletPlacementRadius * Mathf.Sin(degrees * Mathf.Deg2Rad);

            bullet.transform.localPosition = new Vector3(bulletForwardAxisOffset, x, y);
            Collider col = bullet.GetComponentInChildren<Collider>();
            foreach (Collider col2 in gunColliders)
            {
                Physics.IgnoreCollision(col, col2);
            }
            col.enabled = false;
            bullet.GetComponentInChildren<Collider>().enabled = false;

            bulletSpent[i] = false;
            bullets[i] = bullet;
        }
    }

    private void EjectShell(int shellIndex)
    {
        Debug.Log(shellIndex);

        if (bullets[shellIndex] != null && bulletSpent[shellIndex] == true)
        {
            GameObject bullet = bullets[shellIndex];
            bullet.transform.parent = null;
            bullet.GetComponentInChildren<Collider>().enabled = true;

            Rigidbody rb = bullet.GetComponentInChildren<Rigidbody>();
            rb.isKinematic = false;
            rb.useGravity = true;
            rb.AddForce(shellEjectionForce * -bullet.transform.right, ForceMode.Force);

            //Destroy bullet after some time left

            bullets[shellIndex] = null;
        }

        isInsertMag = false;
    }
    #endregion

    #region OpenAndClose : 
    public void OpenForReload(bool isRight)
    {
        if (isOpen)
        {
            return;
        }

        this.openStartTime = Time.time;

        this.stayOpenDuration = HasShotsToReload() ? openDurationReloadNeeded : openDurationReloadNotNeeded;

        if (this.positionCoroutine != null)
        {
            StopCoroutine(this.positionCoroutine);
        }
        this.positionCoroutine = StartCoroutine(AnimationBarrelToOpen(isRight ? this.openRot : Quaternion.Inverse(this.openRot), isRight));

        minRotationSpeedForReloading = defaultOpenSpinSpeed;
        shouldSpin = true;
        isOpen = true;

        //openCloseAS.clip = this.revolverCloseClip;
        //openCloseAS.Play();

    }

    public bool CloseFromReload(bool isRight)
    {
        if (!isOpen && !isInsertMag)
        {
            return false;
        }

        if (this.positionCoroutine != null)
        {
            StopCoroutine(this.positionCoroutine);
        }

        this.positionCoroutine = StartCoroutine(AnimationBarrelToClose(this.closeRot, isRight));

        isOpen = false;
        canEjectShells = false;

        revolverParent.Reload();

        //openCloseAS.clip = this.revolverCloseClip;
        //openCloseAS.Play();

        return true;
    }

    private IEnumerator AnimationBarrelToOpen(Quaternion rot, bool isRight)
    {
        if (isRight == true)
        {
            while (transform.localRotation != rot && transform.localRotation.x > rot.x)
            {
                this.transform.localRotation = Quaternion.Lerp(transform.localRotation, rot, Time.deltaTime * openSpeed);

                yield return null;
            }
        }
        else
        {
            while (transform.localRotation != rot && transform.localRotation.x < rot.x)
            {

                this.transform.localRotation = Quaternion.Lerp(transform.localRotation, rot, Time.deltaTime * openSpeed);

                yield return null;
            }
        }

        if (isOpen)
        {
            canEjectShells = true;
        }
        else
        {
            shouldSpin = false;
            this.isAnimating = true;
        }
    }

    private IEnumerator AnimationBarrelToClose(Quaternion rot, bool isRight)
    {
        //Debug.Log($"Close : {transform.localRotation}");

        if (isRight == true)
        {
            while (transform.localRotation != rot && transform.localRotation.x < rot.x)
            {
                this.transform.localRotation = Quaternion.Lerp(transform.localRotation, rot, Time.deltaTime * openSpeed);

                yield return null;
            }
        }
        else
        {
            while (transform.localRotation != rot && transform.localRotation.x > rot.x)
            {

                this.transform.localRotation = Quaternion.Lerp(transform.localRotation, rot, Time.deltaTime * openSpeed);

                yield return null;
            }
        }


        if (isOpen)
        {
            canEjectShells = true;
        }
        else
        {
            shouldSpin = false;
            this.isAnimating = true;
        }
    }
    #endregion

    #region AngleCalculate :
    float AngleForIndex(int curIndex)
    {
        return 360.0f * ((float)curIndex / (float)numberOfChambers);
    }

    int IndexForAngle(float angle)
    {
        return (int)Mathf.Floor((angle / 360.0f) * (float)numberOfChambers);
    }


    Quaternion RotationForIndex(int curIndex)
    {
        float angle = AngleForIndex(curIndex);
        return Quaternion.AngleAxis(angle, Vector3.left);
    }

    bool HasShotsToReload()
    {
        foreach(bool spent in this.bulletSpent)
        {
            if(spent == true)
            {
                return true;
            }
        }
        return false;
    }

    float BarrelAngle()
    {
        Vector3 forwardVector = SpinPart.transform.localRotation * Vector3.forward;
        float angle = Mathf.Atan2(forwardVector.y, forwardVector.z) * Mathf.Rad2Deg;
        if(angle < 0)
        {
            angle += 360.0f;
        }
        return angle;
    }

    #endregion

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "MAGAZINE" || other.tag == "MAGAZINE_REFLECT")
        {
            isInsertMag = true;

            if (other.tag == "MAGAZINE")
            {
                revolverParent.MagTag = "MAGAZINE";
                revolverParent.beaming = false;
            }
            else if (other.tag == "MAGAZINE_REFLECT")
            {
                revolverParent.MagTag = "MAGAZINE_REFLECT";
                revolverParent.beaming = true;
            }

            Debug.Log(revolverParent.MagTag);

            other.transform.parent.GetComponent<PickUp>().ReleaseMag();

            other.transform.position = insertPos.transform.position;

            //revolverParent.Reload();

            ReloadBarrel();

            other.gameObject.SetActive(false);
            other.transform.SetParent(null);
        }
    }

}
