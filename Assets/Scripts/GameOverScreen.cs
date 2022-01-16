using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}
	public void SetUp()
	{
		gameObject.SetActive(true);
	}

	public void RestartButton()
	{
		print("clicking restart button");	
		gameObject.SetActive(false);
		SceneManager.LoadScene(1);
	}

	public void MenuButton()
	{
		SceneManager.LoadScene(0);
	}
}
