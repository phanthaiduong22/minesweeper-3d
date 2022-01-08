using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeText : MonoBehaviour
{
	// Start is called before the first frame update
	public MeshRenderer meshRender;
	public TextMesh textMesh;

	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

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
	}
}
