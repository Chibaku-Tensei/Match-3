using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class UIManager : MonoBehaviour
    {
        private static UIManager _instance;

        private int _score = 0;
        [SerializeField] private GameObject replayPanel;
        [SerializeField] private Text scoreText;

        public static UIManager Instance => _instance;

        private void Awake()
        {
            _instance = this;
        }

        public void PauseButtonClick()
        {
            replayPanel.SetActive(true);
        }

        public void ReplayButtonClick()
        {
            Board.Instance.StartBoard();
            replayPanel.SetActive(false);
            scoreText.text = "0";
            _score = 0;
        }

        public void Scored(int score)
        {
            _score += score;
            scoreText.text = $"{_score}";
        }
    }
}

