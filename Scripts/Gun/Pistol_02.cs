using QFX.SFX;
using RootMotion.Dynamics;
using RootMotion.FinalIK;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Valve.VR;

public class Pistol_02 : Pistol
{
    // Beam
    public bool reflect = true;                         
    public Material reflectionMaterial;               
    public int maxReflections = 3;                     
    public string beamTypeName = "laser_beam";          
    public float maxBeamHeat = 1.0f;                   
    public bool infiniteBeam = false;                   
    public Material beamMaterial;                      
    public Color beamColor = Color.red;                
    public float startBeamWidth = 0.5f;                 
    public float endBeamWidth = 1.0f;                                
    private GameObject beamGO;                          
    public bool beaming = false;


  //  public EnemyController2 enemyCon;  
    List<Vector3> reflectionPoints = new List<Vector3>();
    List<GameObject> reflectionHitObjects = new List<GameObject>();
    GameObject LastHitObject;

    //hit reaction
    public float unpin = 10f;
    public float force = 10f;

    // Range
    public float range = 9999.0f;                       
    public Transform raycastStartSpot;                  
    public float Force = 100.0f;

    //Bullet Hole
    public GameObject bulletHole;
    public GameObject[] hitEffects = new GameObject[] { null };
    public bool isUseBullet = false;

    //HandTypeCheck
    private GameObject hand;
    bool controllerInRightHand;

    //Revolver Open & Close
    public RevolverHolder revolvingHolder;
    public float openAcceleration;
    public float openEmptyAcceleration;
    public float closeAcceleration;
    private MyLinearAcceleration linearAccelerationTracker;

    public string MagTag;

    public GameObject lazerPointer;

    protected override void Awake()
    {
        base.Awake();

        revolvingHolder.revolverParent = this;
        linearAccelerationTracker = new MyLinearAcceleration();

        // Make sure raycastStartSpot isn't null
        if (raycastStartSpot == null)
            raycastStartSpot = projectileSpawnSpot;

        MagTag = "MAGAZINE";
    }

    protected override void Update()
    {
        base.Update();

        if (this.transform.parent != null) lazerPointer.SetActive(true);
        else lazerPointer.SetActive(false);

        if (this.transform.parent != null) hand = this.transform.parent.gameObject;

        if (hand != null && this.transform.parent != null)
        {
            Vector3 gunAcceleration;
            Vector3 gunVelocity = linearAccelerationTracker.LinearAcceleration(out gunAcceleration, this.transform.parent.localPosition, 8);
            if (gunVelocity == Vector3.zero) return;

            gunAcceleration = this.transform.parent.InverseTransformDirection(gunAcceleration);
            gunVelocity = this.transform.parent.InverseTransformDirection(gunVelocity);

            float OpenAcceleration = (this.CurrentAmmo == 0) ? openEmptyAcceleration : openAcceleration;

            bool isDecelerating = !Sign(gunAcceleration.x, gunVelocity.x);
            controllerInRightHand = hand.GetComponent<SteamVR_Behaviour_Pose>().inputSource == SteamVR_Input_Sources.RightHand;

            if (isDecelerating)
            {
                if (Sign(gunAcceleration.x, (controllerInRightHand ? OpenAcceleration : -OpenAcceleration)) && Mathf.Abs(gunAcceleration.x) > Mathf.Abs(OpenAcceleration) && CurrentAmmo <= 0)
                {
                    Debug.Log("Open!!");
                    //revolvingHolder.OpenForReload(controllerInRightHand);
                }
                else if (Sign(gunAcceleration.x, (controllerInRightHand ? closeAcceleration : -closeAcceleration)) && Mathf.Abs(gunAcceleration.x) > Mathf.Abs(closeAcceleration))
                {
                    //Debug.Log($"Close!! : {gunAcceleration.x}");

                    revolvingHolder.CloseFromReload(controllerInRightHand);
                }
            }

        }

        if (beaming && CurrentAmmo > 0) Beam();

        if (CurrentAmmo <= 0)
        {
            Debug.Log("Update Beaming false");

            revolvingHolder.OpenForReload(controllerInRightHand);
            //beaming = false;
            return;
        }


    }

    bool Sign(float a, float b)
    {
        return (a > 0 && b > 0 || a <= 0 && b <= 0);
    }

    new
    public void Fire()
    {
        Debug.Log(MagTag);

        if (MagTag == "MAGAZINE") FireBasic();
        else if(MagTag == "MAGAZINE_REFLECT")
        {
            beaming = false;
            FireBeam();
            Invoke("reStartReBeam", 0.1f);
        }
    }

    void reStartReBeam()
    {
        beaming = true;
    }

    // Raycasting system
    public void FireBasic()
    {
        Debug.Log("fIRE!!!!!");
        
        if (isUseBullet == true)
        {
            base.Fire();
            return;
        }

        // First make sure there is ammo
        if (CurrentAmmo <= 0)
        {
            DryFire();
            return;
        }

        CurrentAmmo--;
        
        revolvingHolder.Revolve();
        
        // The ray that will be used for this shot
        Ray ray = new Ray(raycastStartSpot.position, raycastStartSpot.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, range))
        {
            Debug.Log(hit.collider.name);

            // Damage
            //   hit.collider.gameObject.SendMessageUpwards("ChangeHP", 30, SendMessageOptions.DontRequireReceiver);

            if (hit.collider.gameObject.CompareTag("ENEMY"))
            {
                Debug.Log("으악");
                var controller2 = hit.collider.gameObject.GetComponentInParent<EnemyHealth>();
                controller2.CurHP = controller2.CurHP - 50f;
                //  enemyCon.enmeyHealth = enemyCon.enmeyHealth - 50f;
                Debug.Log(controller2.CurHP);

                var broadcaster = hit.collider.attachedRigidbody.GetComponentInChildren<MuscleCollisionBroadcaster>();

                if (broadcaster != null)
                {
                    Debug.Log("Puppet Hit Reation");
                    broadcaster.Hit(unpin, ray.direction * force, hit.point);
                }
            }

         if (hit.collider.gameObject.CompareTag("ENEMYHEAD"))
         {
             Debug.Log("머리으악");
             var controller2 = hit.collider.gameObject.GetComponentInParent<EnemyHealth>();
             controller2.CurHP = controller2.CurHP - 100;
         }
            Debug.DrawRay(raycastStartSpot.position, raycastStartSpot.forward * 100f, Color.red, 100f);

            // Place the bullet hole in the scene
            if (bulletHole != null)
                Instantiate(bulletHole, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));


            foreach (GameObject hitEffect in hitEffects)
            {
                if (hitEffect != null)
                    Instantiate(hitEffect, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
            }

            // Add force to the object that was hit
            if (hit.transform.GetComponentInChildren<PuppetMaster>())
            {
                Debug.Log("Add Explosion!!");
                hit.rigidbody.AddExplosionForce(Force, hit.point, 1.0f);
            }
        }

       

        //Recoil();

        GameObject muzfx = muzzleEffects[1];
        if (muzfx != null)
        {
            Instantiate(muzfx, muzzleEffectPosition.position, muzzleEffectPosition.rotation);
        }

        //GameObject shellGO = Instantiate(shell, shellSpitPosition.position, shellSpitPosition.rotation) as GameObject;
        //shellGO.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(shellSpitForce + Random.Range(0, shellForceRandom), 0, 0), ForceMode.Impulse);
        //shellGO.GetComponent<Rigidbody>().AddRelativeTorque(new Vector3(shellSpitTorqueX + Random.Range(-shellTorqueRandom, shellForceRandom), shellSpitTorqueY + Random.Range(-shellTorqueRandom, shellTorqueRandom), 0), ForceMode.Impulse);


        GetComponent<AudioSource>().PlayOneShot(fireSound);

    }

    public void Beam()
    {
        // Set the beaming variable to true
        // beaming = true;

        // Make the beam effect if it hasn't already been made.  This system uses a line renderer on an otherwise empty instantiated GameObject
        if (beamGO == null)
        {
            beamGO = new GameObject(beamTypeName, typeof(LineRenderer));
            beamGO.transform.parent = transform;        // Make the beam object a child of the weapon object, so that when the weapon is deactivated the beam will be as well	- was beamGO.transform.SetParent(transform), which only works in Unity 4.6 or newer;
        }

        LineRenderer beamLR = beamGO.GetComponent<LineRenderer>();
        beamLR.material = beamMaterial;
        beamLR.material.SetColor("_TintColor", beamColor);
        beamLR.startWidth = startBeamWidth;
        beamLR.endWidth = endBeamWidth;

        // The number of reflections
        int reflections = 0;

        // All the points at which the laser is reflected
        reflectionPoints = new List<Vector3>();
        reflectionHitObjects = new List<GameObject>();
        // Add the first point to the list of beam reflection points
        reflectionPoints.Add(raycastStartSpot.position);

        // Hold a variable for the last reflected point
        Vector3 lastPoint = raycastStartSpot.position;

        // Declare variables for calculating rays
        Vector3 incomingDirection;
        Vector3 reflectDirection;

        // Whether or not the beam needs to continue reflecting
        bool keepReflecting = true;

        // Raycasting (damgage, etc)
        Ray ray = new Ray(lastPoint, raycastStartSpot.forward);
        RaycastHit hit;

        do
        {
            // Initialize the next point.  If a raycast hit is not returned, this will be the forward direction * range
            Vector3 nextPoint = ray.direction * range;

            if (Physics.Raycast(ray, out hit, range))
            {
                // Set the next point to the hit location from the raycast
                nextPoint = hit.point;

                // Calculate the next direction in which to shoot a ray
                incomingDirection = nextPoint - lastPoint;
                reflectDirection = Vector3.Reflect(incomingDirection, hit.normal);
                ray = new Ray(nextPoint, reflectDirection);

                // Update the lastPoint variable
                lastPoint = hit.point;

                // Hit Effects
                foreach (GameObject hitEffect in hitEffects)
                {
                    if (hitEffect != null)
                        Instantiate(hitEffect, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
                }

                LastHitObject = hit.collider.gameObject;
                // Increment the reflections counter
                reflections++;
            }
            else
            {
                keepReflecting = false;
            }

            // Add the next point to the list of beam reflection points
            reflectionPoints.Add(nextPoint);
            reflectionHitObjects.Add(hit.transform.gameObject);

        } while (keepReflecting && reflections < maxReflections && reflect && (reflectionMaterial == null || (FindMeshRenderer(hit.collider.gameObject) != null && FindMeshRenderer(hit.collider.gameObject).sharedMaterial == reflectionMaterial)));

        // Set the positions of the vertices of the line renderer beam
        //beamLR.SetVertexCount(reflectionPoints.Count);

        beamLR.positionCount = reflectionPoints.Count;

        for (int i = 0; i < reflectionPoints.Count; i++)
        {
            beamLR.SetPosition(i, reflectionPoints[i]);

            // Muzzle reflection effects
            if (i > 0)     // Doesn't make the FX on the first iteration since that is handled later.  This is so that the FX at the muzzle point can be parented to the weapon
            {
                GameObject muzfx_r = muzzleEffects[0];
                if (muzfx_r != null)
                {
                    Instantiate(muzfx_r, reflectionPoints[i], muzzleEffectPosition.rotation);
                }
            }
        }

        GameObject muzfx = muzzleEffects[0];
        if (muzfx != null)
        {
            GameObject mfxGO = Instantiate(muzfx, muzzleEffectPosition.position, muzzleEffectPosition.rotation) as GameObject;
            mfxGO.transform.parent = raycastStartSpot;
        }
    }

    public void FireBeam()
    {
        if (CurrentAmmo <= 0)
        {
            DryFire();
            beaming = false;
            return;
        }

        Debug.Log("Fire beam!~");

        CurrentAmmo--;

        //Recoil();

        revolvingHolder.Revolve();

        GetComponent<AudioSource>().PlayOneShot(fireSound);

        // Damage
        //LastHitObject.SendMessageUpwards("ChangeHealth", damage, SendMessageOptions.DontRequireReceiver);

        if (reflectionPoints != null)
        {

            foreach (GameObject hitPoint in reflectionHitObjects)
            {

                if (hitPoint.CompareTag("ENEMY"))
                {
                    Debug.Log("으악");
                    var controller2 = hitPoint.GetComponentInParent<EnemyHealth>();
                    controller2.CurHP = controller2.CurHP - 50f;
                    //  enemyCon.enmeyHealth = enemyCon.enmeyHealth - 50f;
                    Debug.Log(controller2.CurHP);
                }

                if (hitPoint.CompareTag("ENEMYHEAD"))
                {
                    Debug.Log("머리으악");
                    var controller2 = hitPoint.GetComponentInParent<EnemyHealth>();
                    controller2.CurHP = controller2.CurHP - 100;
                }
            }
        }

        // Place the bullet hole in the scene
        if (bulletHole != null)
            Instantiate(bulletHole, reflectionPoints[reflectionPoints.Count - 1], Quaternion.FromToRotation(Vector3.up, -reflectionPoints[reflectionPoints.Count - 1].normalized));


        StopBeam();
    }

    public void StopBeam()
    {
        reflectionPoints.Clear();
        reflectionHitObjects.Clear();


        // Remove the visible beam effect GameObject
        if (beamGO != null)
        {
            Destroy(beamGO);
        }
    }

    // Find a mesh renderer in a specified gameobject, it's children, or its parents
    MeshRenderer FindMeshRenderer(GameObject go)
    {
        MeshRenderer hitMesh;

        // Use the MeshRenderer directly from this GameObject if it has one
        if (go.GetComponent<Renderer>() != null)
        {
            hitMesh = go.GetComponent<MeshRenderer>();
        }

        // Try to find a child or parent GameObject that has a MeshRenderer
        else
        {
            // Look for a renderer in the child GameObjects
            hitMesh = go.GetComponentInChildren<MeshRenderer>();

            // If a renderer is still not found, try the parent GameObjects
            if (hitMesh == null)
            {
                GameObject curGO = go;
                while (hitMesh == null && curGO.transform != curGO.transform.root)
                {
                    curGO = curGO.transform.parent.gameObject;
                    hitMesh = curGO.GetComponent<MeshRenderer>();
                }
            }
        }

        return hitMesh;
    }
}
