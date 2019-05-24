using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour 
{
	[SerializeField] Button startGameButton;
	[SerializeField] Button quitGameButton;
	public string levelName;

	void Start()
	{
		startGameButton.onClick.AddListener (StartGame);
		startGameButton.onClick.AddListener (QuitGame);
	}

	public void StartGame()
	{
		SceneManager.LoadScene (levelName);
	}

	public void QuitGame()
	{
		Application.Quit();
	}
}