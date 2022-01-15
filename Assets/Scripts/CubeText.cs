using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeText : MonoBehaviour
{
	// Start is called before the first frame update
	public MeshRenderer meshRender;
	public TextMesh textMesh;
	public Camera cameraToFollow;
	// public float DistanceFromCamera;

	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		// transform.position = CameraToFollow.transform.position + CameraToFollow.transform.forward * DistanceFromCamera;
		// transform.LookAt(Camera.main.transform);
		/*transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward,
			Camera.main.transform.rotation * Vector3.up);*/
		this.transform.LookAt(cameraToFollow.transform);
	}

	public void MeshRenderEnable()
	{
		meshRender.enabled = true;
	}
	public void MeshRenderDisable()
	{
		meshRender.enabled = false;
	}
	public void ChangeTextMesh(int cntBombs)
	{
		if (cntBombs >= 1)
		{
			textMesh.text = cntBombs.ToString();
		}
		else
		{
			textMesh.text = "";
			MeshRenderDisable();
		}
		// transform.localEulerAngles = new Vector3(180, 180, 180);
		// transform.Rotate(Vector3.up - Vector3(0, 180, 0));
		textMesh.characterSize = 50f;
	}
}
