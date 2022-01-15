using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeText : MonoBehaviour
{
	// Start is called before the first frame update
	public MeshRenderer meshRender;
	public TextMesh textMesh;
	public Camera cameraToFollow;
	public float DistanceFromCamera;

	public Transform bannerLookTarget;

	void Start()
	{
		bannerLookTarget = Camera.main.transform;
	}

	// Update is called once per frame
	void Update()
	{
		Vector3 v = Camera.main.transform.position - transform.position;
		v.x = v.z = 0.0f;
		transform.LookAt(Camera.main.transform.position - v);
		transform.rotation = (Camera.main.transform.rotation); // Take care about camera rotation

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
