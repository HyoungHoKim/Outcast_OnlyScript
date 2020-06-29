using System.Collections;
using System.Collections.Generic;
//using System.Runtime.Remoting.Metadata.W3cXsd2001;
using UnityEngine;
using Valve.VR;

// 이동 : world Transform 
// 라인 그리기 : local Transform

public class MyTeleport : MonoBehaviour
{
    public GameObject positionMarker;
    private Material MatEnable;
    private Material MatInvaild;

    public Transform lHandPos; // 각 컨트롤러 Transform 
    public Transform rHandPos;
    
    private Vector3 groundPos;
    private Vector3 EndPos;

    public float angle = 45f; // 포물선 각도 
    public float strength = 10f; // 클수록 세게 빠르게 나감, 각 vertex사이의 간격이 줄어듬 
    int maxVertexcount = 100; // 최대 vertex 값
    private float vertexDelta = 0.05f; // 델타 값
    private Vector3 velocity;
    private Vector3 lastNormal;
    public float Speed = 10;

    private LineRenderer arcRenderer;
    public Material lineMaterial;

    private List<Vector3> vertexList = new List<Vector3>();
    private List<Vector3> lineVertexList = new List<Vector3>();

    private bool Detected = false;
    private bool groundDetected = false;
    private bool displayActive = false;

    private bool isLeft = false;
    private bool moving = false;

    private AudioSource telpoSound;
    public AudioClip teleport_waiting;
    public AudioClip teleport_move;
    public AudioClip teleport_waiting_start;

    public SteamVR_Action_Boolean TeleportAction = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("Teleport");

    private void Start()
    {
        EndPos = this.transform.position;
        telpoSound = GetComponent<AudioSource>();

        MatEnable = Resources.Load<Material>("TeleportPointHighlighted");
        MatInvaild = Resources.Load<Material>("TeleportPointInvalid");
    }

    void Update()
    {
        if (displayActive)
        {
            UpdatePath();
        }

        if (TeleportAction.GetStateDown(SteamVR_Input_Sources.LeftHand) 
            && !TeleportAction.GetStateDown(SteamVR_Input_Sources.RightHand)) {

            ToggleDisplay(true);
            ToggleSound();
            isLeft = true;
        }

        if (!TeleportAction.GetStateDown(SteamVR_Input_Sources.LeftHand)
            && TeleportAction.GetStateDown(SteamVR_Input_Sources.RightHand))
        {
            ToggleSound();
            ToggleDisplay(true);
            isLeft = false;
        }

        if (TeleportAction.GetStateUp(SteamVR_Input_Sources.Any))
        {
            if (telpoSound.isPlaying)
            {
                telpoSound.loop = false;
                telpoSound.Stop();
            }
            telpoSound.PlayOneShot(teleport_move);

            EndPos = groundPos;

            ToggleDisplay(false);
            isLeft = false;
            moving = true;
        }

        Teleport();
    }

    
    public void Teleport()
    { 
        if (moving && groundDetected)
        {
            this.transform.position = Vector3.Lerp(this.transform.position, (EndPos + lastNormal * 0.1f), Time.deltaTime * Speed);

            if (Vector3.Magnitude(EndPos + (lastNormal * 0.1f) - this.transform.position) < 0.5f)
            {
                moving = false;
            }
        }
    }

    // Active Teleporter Arc Path
    public void ToggleDisplay(bool active)
    {
        arcRenderer.enabled = active;
        positionMarker.SetActive(active);
        displayActive = active;
    }

    public void ToggleSound()
    {
        telpoSound.PlayOneShot(teleport_waiting_start);
        telpoSound.clip = teleport_waiting;
        telpoSound.Play();
        telpoSound.loop = true;
    }

    private void Awake()
    {
        createLineRenderer();
        arcRenderer.enabled = false;
        positionMarker.SetActive(false);
    }

    void createLineRenderer()
    {
        arcRenderer = this.gameObject.AddComponent<LineRenderer>();
        arcRenderer.useWorldSpace = false;
        arcRenderer.receiveShadows = false;

        arcRenderer.startWidth = 0.03f;
        arcRenderer.endWidth = 0.005f;

        arcRenderer.material = lineMaterial;
    }

    private void UpdatePath()
    {
        Transform startPos = isLeft == true ? lHandPos : rHandPos;

        Detected = false;

        vertexList.Clear();
        lineVertexList.Clear();

        velocity = Quaternion.AngleAxis(-angle, startPos.right) * startPos.forward * strength;

        RaycastHit hit;

        Vector3 pos = startPos.position;
        Vector3 linePos = startPos.localPosition;

        vertexList.Add(pos);

        while (!Detected && vertexList.Count < maxVertexcount)
        {
            Vector3 newPos = pos + velocity * vertexDelta
                + 0.5f * Physics.gravity * vertexDelta * vertexDelta;
            Vector3 newLinePos = linePos + velocity * vertexDelta
                + 0.5f * Physics.gravity * vertexDelta * vertexDelta;

            vertexList.Add(newPos);
            lineVertexList.Add(newLinePos);

            velocity += Physics.gravity * vertexDelta;
  
            if (Physics.Linecast(pos, newPos, out hit))
            {
                if (hit.transform.gameObject.tag == "GROUND"
                    && hit.transform.position.y < this.transform.position.y + 1
                    && hit.transform.position.y > this.transform.position.y - 1)
                {
                    groundDetected = true;
                }
                else groundDetected = false;
                
                Detected = true;
                groundPos = hit.point;
                lastNormal = hit.normal;
            }

            pos = newPos;
            linePos = newLinePos;
        }

        if (Detected)
        {

            positionMarker.SetActive(true);
            positionMarker.transform.position = groundPos + (lastNormal * 0.01f);
            positionMarker.transform.rotation = Quaternion.LookRotation(lastNormal);
            positionMarker.transform.Rotate(90.0f, 0, 0);

            if (groundDetected)
            {
                positionMarker.GetComponent<MeshRenderer>().material = MatEnable;
            }
            else
            {
                groundPos = this.transform.position;
                positionMarker.GetComponent<MeshRenderer>().material = MatInvaild;
            }
        }

        arcRenderer.positionCount = lineVertexList.Count;
        arcRenderer.SetPositions(lineVertexList.ToArray());
    }

}
