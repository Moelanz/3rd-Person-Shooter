using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZombieScoreManager : MonoBehaviour
{
    public class ScoreField
    {
        public Player player;
        public ZombieScoreUI zombieUI;

        public ScoreField(Player _player, ZombieScoreUI _zombieUI)
        {
            player = _player;
            zombieUI = _zombieUI;

            player.PlayerScore.OnScoreChange += OnScoreChange;
        }

        private void OnScoreChange()
        {
            zombieUI.playerScore.text = player.PlayerScore.CurrentScore.ToString();
        }
    }

    [SerializeField] RectTransform scorePanel;
    [SerializeField] GameObject playerScorePrefab;
    [SerializeField] Color[] playerColors;

    private Player[] players;
    private List<ScoreField> playerList = new List<ScoreField>();

	// Use this for initialization
	void Start()
    {
		players = FindObjectsOfType<Player>();

        CreatePlayerScores();
	}

    void CreatePlayerScores()
    {
        for (int i = 0; i < players.Length; i++)
        {
            GameObject playerScore = (GameObject) Instantiate(playerScorePrefab, scorePanel.transform);
            playerScore.transform.SetParent(scorePanel.transform);
            playerScore.name = players[i].gameObject.name;

            ZombieScoreUI zombieUI = GetComponentInChildren<ZombieScoreUI>();

            if(zombieUI != null)
            {
                zombieUI.playerName.text = players[i].gameObject.name;
                //zombieUI.playerName.color = playerColors[i];

                zombieUI.playerScore.text = players[i].PlayerScore.CurrentScore.ToString();
            }

            ScoreField player = new ScoreField(players[i], zombieUI);

            playerList.Add(player);
        }
    }
}
