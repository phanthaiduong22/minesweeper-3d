using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeText : MonoBehaviour
{
	public MeshRenderer meshRender;
	public TextMesh textMesh;
	public Camera cameraToFollow;
	public float DistanceFromCamera;
	public Transform bannerLookTarget;

	void Start()
	{
		bannerLookTarget = Camera.main.transform;
	}

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
		textMesh.characterSize = 50f;
		Color color = ChooseColor(cntBombs);
		if (color != Color.clear)
			textMesh.color = color;
	}

	Color ChooseColor(int number)
    {
		switch (number)
        {
			case 1:
				return Color.red;
			case 2:
				return Color.blue;
			case 3:
				return Color.green;
			case 4:
				return Color.cyan;
			case 5:
				return Color.magenta;
			case 6:
				return Color.yellow;
			default:
				return Color.clear;
			
        }
    }
}
