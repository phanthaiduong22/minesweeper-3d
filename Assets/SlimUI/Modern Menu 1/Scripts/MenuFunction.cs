using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuFunction : MonoBehaviour
{
	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}

	public void LoadGameEasy()
	{
		SetEasyDifficulty();
		SceneManager.LoadScene(1);
	}
	public void LoadGameMedium()
	{
		SetMediumDifficulty();
		SceneManager.LoadScene(1);
	}
	public void LoadGameHard()
	{
		SetHardDifficulty();
		SceneManager.LoadScene(1);
	}

	// #region Difficulty
	public void SetEasyDifficulty()
	{
		GameValues.Difficulty = GameValues.Difficulties.Easy;
	}
	public void SetMediumDifficulty()
	{
		GameValues.Difficulty = GameValues.Difficulties.Medium;
	}
	public void SetHardDifficulty()
	{
		GameValues.Difficulty = GameValues.Difficulties.Hard;
	}
	// #endregion
}
