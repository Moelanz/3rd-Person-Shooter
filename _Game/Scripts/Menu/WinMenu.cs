using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinMenu : MonoBehaviour 
{
	[SerializeField] GameObject winMenuPanel;
	[SerializeField] Button backButton;

	void Start()
	{
		winMenuPanel.SetActive (false);
		backButton.onClick.AddListener (OnBackClicked);

		GameManager.Instance.EventBus.AddListener ("GameWin", GameWin);
	}

	void OnBackClicked()
	{
		SceneManager.LoadScene ("MainMenu");
	}

	void GameWin()
	{
		GameManager.Instance.Timer.Add (() => 
		{
			GameManager.Instance.isPaused = true;
			winMenuPanel.SetActive (true);
		}, 4);
	}
}
