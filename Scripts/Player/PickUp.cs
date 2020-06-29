using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

// 다른 손에 주고 있는 총 그립 시 뺏긴 손 pose 안돌아옴, 하이라이트 문제 

public class PickUp : MonoBehaviour
{
    public SteamVR_Action_Boolean ClimbAction = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("GrabPinch");
    public SteamVR_Action_Boolean GripAction = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("GrabGrip");

    public GameObject body;
    private Vector3 handPrevPos;
    private SteamVR_Input_Sources inputSource;
    private Rigidbody bodyRb;
    private Collider handCollider;

    public bool canGrip;
    private GameObject attachObject;

    //HightLight 
    public bool highlightOnHover = true;
    private MeshRenderer[] highlightRenderers;
    private MeshRenderer[] existingRenderers;
    private List<GameObject> highlightHolder;
    private int highlightHolderIndex = 0;
    private SkinnedMeshRenderer[] highlightSkinnedRenderers;
    private SkinnedMeshRenderer[] existingSkinnedRenderers;
    private static Material highlightMat;
    [Tooltip("An array of child gameObjects to not render a highlight for. Things like transparent parts, vfx, etc.")]
    public GameObject[] hideHighlight;

    public bool isHovering { get; protected set; }
    public bool isGrabbing;
    public Animator playerAnim;
    private SummonAmmo summonAmmo;

    //Pistol Grab 
    public Transform pistolAttachPos;
    public Transform BasicMagPos;
    public Transform KnifePos;
    private Transform attachPos;

    // Start is called before the first frame update
    void Start()
    {
        canGrip = false;
        isGrabbing = false;

        handPrevPos = this.transform.localPosition;
        inputSource = this.GetComponent<SteamVR_Behaviour_Pose>().inputSource;
        bodyRb = body.GetComponent<Rigidbody>();

        handCollider = this.GetComponent<SphereCollider>();

        highlightHolder = new List<GameObject>();
        highlightMat = (Material)Resources.Load("SteamVR_HoverHighlight", typeof(Material));
        if (highlightMat == null)
            Debug.LogError("<b>[SteamVR Interaction]</b> Hover Highlight Material is missing. Please create a material named 'SteamVR_HoverHighlight' and place it in a Resources folder", this);
        summonAmmo = this.GetComponent<SummonAmmo>();
    }

    // Update is called once per frames
    void Update()
    {
        if (attachObject == null) return;

        ClimbPull();

        ReleaseObject();

        GripGroup();

    }

    private void ClimbPull()
    {
        if (attachObject.tag == "CLIMB" || attachObject.tag == "GROUND")
        {
            if (canGrip && ClimbAction.GetState(inputSource))
            {
                bodyRb.isKinematic = true;
                bodyRb.useGravity = false;
                body.transform.position += (handPrevPos - this.transform.localPosition);

            }
            else if (ClimbAction.GetStateUp(inputSource))
            {
                bodyRb.isKinematic = false;
                bodyRb.useGravity = true;
                bodyRb.velocity += (handPrevPos - this.transform.localPosition) / Time.deltaTime;
            }

            handPrevPos = this.transform.localPosition;
        }
    }

    #region Grip

    private void GripGroup()
    {
        if (attachObject == null) return;

        if (attachObject.tag == "PISTOL") PistolGrab();
        else if (attachObject.tag == "MAGAZINE" || attachObject.tag == "MAGAZINE_REFLECT") MagazineGrab();
        else if (attachObject.tag == "KNIFE") KnifeGrab();
        else if (attachObject.tag == "ENEMY") EnemyGrab();
    }


    private void GripObject()
    {
        if (canGrip && !isGrabbing && GripAction.GetStateDown(inputSource))
        {
            //summonAmmo.SummonAmmoItem();
           
            AttachOnHand();

            GrabAnim();

            hightLightRemove();

        }
    }

    public void DroneAttach(GameObject flyingGO)
    {
        if (flyingGO != null)
        {
            attachObject = flyingGO;
        }
        else return;

        if (attachObject.tag == "PISTOL")
        {
            attachPos = pistolAttachPos;

            DroneAttachObject();
        }
        else if (attachObject.tag == "MAGAZINE" || attachObject.tag == "MAGAZINE_REFLECT" || attachObject.tag == "BOMB")
        {
            attachPos = BasicMagPos;

            DroneAttachObject();
        }
        else if (attachObject.tag == "KNIFE")
        {
            canGrip = true;

            KnifeGrab();
        }

    }

    private void DroneAttachObject()
    {
        AttachOnHand();

        GrabAnim();

        hightLightRemove();
    }

    private void GrippingObject()
    {
        if (canGrip && !isGrabbing && GripAction.GetStateDown(inputSource))
        {
            if(attachObject.tag == "ENEMY") AttachOnHand();
            else AttachJustPosOnHand();
            
            GrabAnim();

        }
        else if (GripAction.GetState(inputSource))
        {
            //if(attachObject.tag == "ENEMY") //AttachOnHand();
            //else 
            
            AttachJustPosOnHand();
            
            hightLightRemove();

            GrabAnim();
        }
        else if (attachObject != null && isGrabbing && GripAction.GetStateUp(inputSource))
        {
            ReleaseAnim();
    
            if (GetComponent<FixedJoint>())
            {
                GetComponent<FixedJoint>().connectedBody = null;
                Destroy(GetComponent<FixedJoint>());
            }
            attachObject.GetComponent<Rigidbody>().velocity = this.GetComponent<SteamVR_Behaviour_Pose>().GetVelocity();
            attachObject.GetComponent<Rigidbody>().angularVelocity = this.GetComponent<SteamVR_Behaviour_Pose>().GetAngularVelocity();
            attachObject.GetComponent<Rigidbody>().useGravity = true;

            handCollider.enabled = true;
            canGrip = true;
            isGrabbing = false;

            attachObject.transform.parent = null;
            attachObject = null;
        }
    }

    private void EnemyGrab()
    {
        Debug.Log("Enemy Grap!!");

        GrippingObject();
    }

    private void PistolGrab()
    {
            //Debug.Log("Pistol Grap!!");

            attachPos = pistolAttachPos;

            GripObject();

            if (isGrabbing && ClimbAction.GetStateDown(inputSource))
            {
                attachObject.GetComponent<Pistol_02>().Fire();
            }
    }

    private void MagazineGrab()
    {
        attachPos = BasicMagPos;

        GripObject();
    }

    private void KnifeGrab()
    {
        attachPos = KnifePos;

        GrippingObject();
    }

    private void GrabAnim()
    {
        playerAnim.SetBool("bLeftGrip", false);

        if (inputSource == SteamVR_Input_Sources.LeftHand && attachObject.tag == "PISTOL")
        {
            playerAnim.SetBool("bLeftPistolGrip", true);
        }
        else if (inputSource == SteamVR_Input_Sources.RightHand && attachObject.tag == "PISTOL")
        {
            playerAnim.SetBool("bRightPistolGrip", true);
        }

        if (inputSource == SteamVR_Input_Sources.LeftHand && attachObject.tag == "MAGAZINE")
        {
            playerAnim.SetBool("bLeftMagGrip", true);
        }
        else if (inputSource == SteamVR_Input_Sources.RightHand && attachObject.tag == "MAGAZINE")
        {
            playerAnim.SetBool("bRightMagGrip", true);
        }

        if (inputSource == SteamVR_Input_Sources.LeftHand && attachObject.tag == "KNIFE")
        {
            playerAnim.SetBool("bLeftKnifeGrip", true);
        }
        else if (inputSource == SteamVR_Input_Sources.RightHand && attachObject.tag == "KNIFE")
        {
            playerAnim.SetBool("bRightKnifeGrip", true);
        }
    }

    private void ReleaseAnim()
    {
        if (inputSource == SteamVR_Input_Sources.LeftHand && attachObject.tag == "PISTOL")
        {
            playerAnim.SetBool("bLeftPistolGrip", false);
        }
        else if (inputSource == SteamVR_Input_Sources.RightHand && attachObject.tag == "PISTOL")
        {
            playerAnim.SetBool("bRightPistolGrip", false);
        }

        if (inputSource == SteamVR_Input_Sources.LeftHand && attachObject.tag == "MAGAZINE")
        {
            playerAnim.SetBool("bLeftMagGrip", false);
        }
        else if (inputSource == SteamVR_Input_Sources.RightHand && attachObject.tag == "MAGAZINE")
        {
            playerAnim.SetBool("bRightMagGrip", false);
        }

        if (inputSource == SteamVR_Input_Sources.LeftHand && attachObject.tag == "KNIFE")
        {
            playerAnim.SetBool("bLeftKnifeGrip", false);
        }
        else if (inputSource == SteamVR_Input_Sources.RightHand && attachObject.tag == "KNIFE")
        {
            playerAnim.SetBool("bRightKnifeGrip", false);
        }
    }
    #endregion

    #region Attach&Detach :
    private void AttachOnHand()
    {
        isHovering = false;
        handCollider.enabled = false;
        canGrip = false;
        isGrabbing = true;

        attachObject.transform.position = attachPos.transform.position;
        attachObject.transform.rotation = attachPos.transform.rotation;
        attachObject.transform.parent = this.transform;

        if (this.GetComponent<FixedJoint>() == null)
        {
            var joint = AddFixedJoint();
            joint.connectedBody = attachObject.GetComponent<Rigidbody>();
        }
    }

    private void AttachJustPosOnHand()
    {
        isHovering = false;
        handCollider.enabled = false;
        canGrip = false;
        isGrabbing = true;

        attachObject.transform.position = attachPos.transform.position;
        attachObject.transform.rotation = attachPos.transform.rotation;
        attachObject.transform.parent = this.transform;
    }
    
    private void AttachEnemyOnHand()
    {
        isHovering = false;
        handCollider.enabled = false;
        canGrip = false;
        isGrabbing = true;

        attachObject.transform.position = attachPos.transform.position;
        attachObject.transform.rotation = attachPos.transform.rotation;
    }

    private FixedJoint AddFixedJoint()
    {
        FixedJoint fx = gameObject.AddComponent<FixedJoint>();
        fx.breakForce = 200000;
        fx.breakTorque = 200000;
        return fx;
    }

    public void ReleaseObject()
    {
        if (attachObject != null && isGrabbing && GripAction.GetStateDown(inputSource))
        {
            Debug.Log($"ReleaseObject : {attachObject.name}");

            ReleaseAnim();

            if (GetComponent<FixedJoint>())
            {
                GetComponent<FixedJoint>().connectedBody = null;
                Destroy(GetComponent<FixedJoint>());
            }

            attachObject.GetComponent<Rigidbody>().velocity = this.GetComponent<SteamVR_Behaviour_Pose>().GetVelocity();
            attachObject.GetComponent<Rigidbody>().angularVelocity = this.GetComponent<SteamVR_Behaviour_Pose>().GetAngularVelocity();
            attachObject.GetComponent<Rigidbody>().useGravity = true;

            handCollider.enabled = true;
            canGrip = true;
            isGrabbing = false;

            attachObject.transform.parent = null;
            attachObject = null;
            
        }
    }

    public void ReleaseMag()
    {
        if (attachObject != null && isGrabbing)
        {

            ReleaseAnim();

            attachObject.GetComponent<Rigidbody>().useGravity = true;
            attachObject.GetComponent<Rigidbody>().isKinematic = false;

            handCollider.enabled = true;
            isGrabbing = false;
            canGrip = true;

            attachObject.transform.parent = null;
            attachObject = null;

        }
    }
    #endregion

    #region HighLight :
    private void hightLightRemove()
    {
        if (highlightOnHover && highlightHolder != null)
        {
            foreach (GameObject holder in highlightHolder)
                Destroy(holder);

            highlightHolder.Clear();
        }
    }

    private bool ShouldIgnore(GameObject check)
    {
        for (int ignoreIndex = 0; ignoreIndex < hideHighlight.Length; ignoreIndex++)
        {
            if (check == hideHighlight[ignoreIndex])
                return true;
        }

        return false;
    }

    private bool ShouldIgnoreHighlight(Component component)
    {
        return ShouldIgnore(component.gameObject);
    }

    private void CreateHighlightRenderers()
    {
        if (attachObject.transform.parent != null)
            if (attachObject.transform.parent.GetComponent<PickUp>().isGrabbing == false)
                return;

        //SkinnedMeshRenderer
        existingSkinnedRenderers = attachObject.GetComponentsInChildren<SkinnedMeshRenderer>(true);
        highlightHolder.Add(new GameObject("Highlighter"));
        highlightSkinnedRenderers = new SkinnedMeshRenderer[existingSkinnedRenderers.Length];

        for (int skinnedIndex = 0; skinnedIndex < existingSkinnedRenderers.Length; skinnedIndex++)
        {
            SkinnedMeshRenderer existingSkinned = existingSkinnedRenderers[skinnedIndex];

            if (ShouldIgnoreHighlight(existingSkinned))
                continue;

            GameObject newSkinnedHolder = new GameObject("SkinnedHolder");
            newSkinnedHolder.transform.parent = highlightHolder[highlightHolder.Count - 1].transform;
            SkinnedMeshRenderer newSkinned = newSkinnedHolder.AddComponent<SkinnedMeshRenderer>();
            Material[] materials = new Material[existingSkinned.sharedMaterials.Length];
            for (int materialIndex = 0; materialIndex < materials.Length; materialIndex++)
            {
                materials[materialIndex] = highlightMat;
            }

            newSkinned.sharedMaterials = materials;
            newSkinned.sharedMesh = existingSkinned.sharedMesh;
            newSkinned.rootBone = existingSkinned.rootBone;
            newSkinned.updateWhenOffscreen = existingSkinned.updateWhenOffscreen;
            newSkinned.bones = existingSkinned.bones;

            highlightSkinnedRenderers[skinnedIndex] = newSkinned;
        }


        //MeshRenderer
        MeshFilter[] existingFilters = attachObject.GetComponentsInChildren<MeshFilter>(true);

        existingRenderers = new MeshRenderer[existingFilters.Length];
        highlightRenderers = new MeshRenderer[existingFilters.Length];

        for (int filterIndex = 0; filterIndex < existingFilters.Length; filterIndex++)
        {
            MeshFilter existingFilter = existingFilters[filterIndex];
            MeshRenderer existingRenderer = existingFilter.GetComponent<MeshRenderer>();

            if (existingFilter == null || existingRenderer == null || ShouldIgnoreHighlight(existingFilter))
                continue;

            GameObject newFilterHolder = new GameObject("FilterHolder");
            newFilterHolder.transform.parent = highlightHolder[highlightHolder.Count - 1].transform;
            MeshFilter newFilter = newFilterHolder.AddComponent<MeshFilter>();
            newFilter.sharedMesh = existingFilter.sharedMesh;
            MeshRenderer newRenderer = newFilterHolder.AddComponent<MeshRenderer>();

            Material[] materials = new Material[existingRenderer.sharedMaterials.Length];
            for (int materialIndex = 0; materialIndex < materials.Length; materialIndex++)
            {
                materials[materialIndex] = highlightMat;
            }
            newRenderer.sharedMaterials = materials;

            highlightRenderers[filterIndex] = newRenderer;
            existingRenderers[filterIndex] = existingRenderer;
        }
    }

    private void UpdateHighlightRenderers()
    {
        if (attachObject.transform.parent != null)
            if(attachObject.transform.parent.GetComponent<PickUp>().isGrabbing == false)
                return;

        if (highlightHolder == null)
            return;

        //Debug.Log($"HightLight Working : {attachObject.name}");

        for (int skinnedIndex = 0; skinnedIndex < existingSkinnedRenderers.Length; skinnedIndex++)
        {
            SkinnedMeshRenderer existingSkinned = existingSkinnedRenderers[skinnedIndex];
            SkinnedMeshRenderer highlightSkinned = highlightSkinnedRenderers[skinnedIndex];

            if (existingSkinned != null && highlightSkinned != null && canGrip == true)
            {
                highlightSkinned.transform.position = existingSkinned.transform.position;
                highlightSkinned.transform.rotation = existingSkinned.transform.rotation;
                highlightSkinned.transform.localScale = existingSkinned.transform.lossyScale;
                highlightSkinned.localBounds = existingSkinned.localBounds;
                highlightSkinned.enabled = isHovering && existingSkinned.enabled && existingSkinned.gameObject.activeInHierarchy;

                int blendShapeCount = existingSkinned.sharedMesh.blendShapeCount;
                for (int blendShapeIndex = 0; blendShapeIndex < blendShapeCount; blendShapeIndex++)
                {
                    highlightSkinned.SetBlendShapeWeight(blendShapeIndex, existingSkinned.GetBlendShapeWeight(blendShapeIndex));
                }
            }
            else if (highlightSkinned != null)
                highlightSkinned.enabled = false;

        }

        for (int rendererIndex = 0; rendererIndex < highlightRenderers.Length; rendererIndex++)
        {
            MeshRenderer existingRenderer = existingRenderers[rendererIndex];
            MeshRenderer highlightRenderer = highlightRenderers[rendererIndex];

            if (existingRenderer != null && highlightRenderer != null && canGrip == true)
            {
                highlightRenderer.transform.position = existingRenderer.transform.position;
                highlightRenderer.transform.rotation = existingRenderer.transform.rotation;
                highlightRenderer.transform.localScale = existingRenderer.transform.lossyScale;
                highlightRenderer.enabled = isHovering && existingRenderer.enabled && existingRenderer.gameObject.activeInHierarchy;
            }
            else if (highlightRenderer != null)
                highlightRenderer.enabled = false;
        }
    }
    #endregion

    #region Collision Trigger :
    private void OnTriggerEnter(Collider coll)
    {
        canGrip = true;
        isHovering = true;
        attachObject = coll.gameObject;
        
        //Debug.Log(attachObject.name);

        if (attachObject != null)
        {
            if (attachObject.tag == "PISTOL" || attachObject.tag == "MAGAZINE" 
                || attachObject.tag == "MAGAZINE_REFLECT" || attachObject.tag == "KNIFE"
                || attachObject.tag == "ENEMY")
            {
                CreateHighlightRenderers();
                UpdateHighlightRenderers();
            }
        }

        if (attachObject.tag == "ENEMY")
        {
            attachPos = pistolAttachPos;
        }
    }

    private void OnTriggerExit(Collider coll)
    {

        canGrip = false;
        isHovering = false;

        hightLightRemove();
    }
    #endregion
}
