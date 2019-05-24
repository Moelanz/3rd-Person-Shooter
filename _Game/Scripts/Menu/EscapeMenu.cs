using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EscapeMenu : MonoBehaviour 
{
	[SerializeField] GameObject EscapeMenuPanel;
	[SerializeField] Button yesGameButton;
	[SerializeField] Button noGameButton;

	void Start()
	{
		EscapeMenuPanel.SetActive (false);
		yesGameButton.onClick.AddListener (OnYesClicked);
		noGameButton.onClick.AddListener (OnNoClicked);
	}

	void Update()
	{
		if (GameManager.Instance.InputController.escape)
		{
			EscapeMenuPanel.SetActive (!EscapeMenuPanel.activeSelf);
			GameManager.Instance.isPaused = EscapeMenuPanel.activeSelf;

			Cursor.visible = EscapeMenuPanel.activeSelf;
			Cursor.lockState = EscapeMenuPanel.activeSelf ? CursorLockMode.Confined : CursorLockMode.Locked;
		}
	}

	void OnYesClicked()
	{
		SceneManager.LoadScene ("MainMenu");
	}

	void OnNoClicked()
	{
		EscapeMenuPanel.SetActive (false);
	}
}
