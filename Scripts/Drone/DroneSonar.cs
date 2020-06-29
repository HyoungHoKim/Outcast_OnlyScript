using System.Collections;
using UnityEngine;

public class DroneSonar : MonoBehaviour
{
	public enum ScanMode { SCAN_DIR = 0, SCAN_SPH };
	[Header("Parameters")]
	public Scanner.ScannerObject.FxType m_FxType = Scanner.ScannerObject.FxType.FT_None;
	public ScanMode m_ScanMode = ScanMode.SCAN_DIR;
	public GameObject m_Emitter;
	public Vector4 m_Dir = new Vector4(1, 0, 0, 0);
	[Range(0.1f, 2f)] public float m_Amplitude = 1f;
	[Range(1f, 16f)] public float m_Exp = 3f;
	[Range(8f, 64f)] public float m_Interval = 20f;
	[Range(1f, 32f)] public float m_Speed = 10f;
	[Header("Internal")]
	public Scanner.ScannerObject[] m_Fxs;

	[Header("Control")]
	public Transform myTr;
	public Transform playerTr;
	public float maxHeight = 2.5f;
	public float dampSpeed = 7f;
	public bool isSonar = false;

	void Start()
	{
		m_Fxs = GameObject.FindObjectsOfType<Scanner.ScannerObject>();
		for (int i = 0; i < m_Fxs.Length; i++)
			m_Fxs[i].Initialize();

		StartCoroutine(MovePosition());
	}


	void Update()
	{
		for (int i = 0; i < m_Fxs.Length; i++)
		{
			m_Fxs[i].ApplyFx(m_FxType);
			m_Fxs[i].UpdateSelfParameters();
			if (ScanMode.SCAN_DIR == m_ScanMode)
			{
				m_Fxs[i].ApplyDirectionalScan(m_Dir);
				m_Fxs[i].SetMaterialsVector("_LightSweepVector", m_Dir);
			}
			else if (ScanMode.SCAN_SPH == m_ScanMode)
			{
				m_Fxs[i].ApplySphericalScan();
				m_Fxs[i].SetMaterialsVector("_LightSweepVector", m_Emitter.GetComponent<Transform>().position);
			}
			m_Fxs[i].SetMaterialsFloat("_LightSweepAmp", m_Amplitude);
			m_Fxs[i].SetMaterialsFloat("_LightSweepExp", m_Exp);
			m_Fxs[i].SetMaterialsFloat("_LightSweepInterval", m_Interval);
			m_Fxs[i].SetMaterialsFloat("_LightSweepSpeed", m_Speed);
		}
	}

	public IEnumerator StateCheck()
	{
		if(isSonar)
			m_FxType = Scanner.ScannerObject.FxType.FT_TransparencyStripe;
		else
			m_FxType = Scanner.ScannerObject.FxType.FT_None;
		yield return null;
	}	
	IEnumerator MovePosition()
	{
		while (true)
		{
			if (isSonar)
			{
				myTr.position = Vector3.Lerp(myTr.position
										   , new Vector3
											(playerTr.position.x
										   , playerTr.position.y + maxHeight
										   , playerTr.position.z)
										   , Time.deltaTime * dampSpeed);

				myTr.LookAt(playerTr);
			}
			yield return null;
		}
	}
}
