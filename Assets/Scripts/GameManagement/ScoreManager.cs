using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;

namespace Team17.StreetHunt
{
    public class ScoreManager : Entity
    {
        [Header("Saving parameters")]
        [Tooltip("Uncheck if you don't want the game to save, should be checked if the game is built.")]
        [SerializeField] private bool shouldSave = false;
        [Tooltip("This name has to be chosen once and never changed afterwards. It directs to this boss stats for the player.")]
        [SerializeField] private string scoreFileName;
        [Header("Score parameters")]
        [SerializeField] private int scorePerBallLeft = 100;
        [SerializeField] private ScoreHit[] scoreHits;
        [Header("Display parameters")]
        [SerializeField] private GameObject scoreCanvas;
        [SerializeField] private TextMeshProUGUI livesLeftValue;
        [SerializeField] private TextMeshProUGUI livesLeftScore;
        [SerializeField] private TextMeshProUGUI hitsValue;
        [SerializeField] private TextMeshProUGUI hitsName;
        [SerializeField] private TextMeshProUGUI hitsScore;
        [SerializeField] private TextMeshProUGUI finalScore;



        private PlayerBossData currentData;
        private string dataPath;

        protected override void Start()
        {
            base.Start();
            dataPath = Path.Combine(Application.persistentDataPath, scoreFileName + ".tds");
            LoadBossStats();
        }

        public override void OnBossHurt(int powerGroupIndex, float hitPower)
        {
            base.OnBossHurt(powerGroupIndex, hitPower);
            AddHit(powerGroupIndex);
        }

        public override void OnLevelEnd()
        {
            base.OnLevelEnd();
            SetValues();
            scoreCanvas.SetActive(true);
            if(shouldSave)
            {
                if (CalculateScore() > currentData.FinalScore)
                {
                    Debug.Log("New high score! saving...");
                    currentData.FinalScore = CalculateScore();
                    currentData.HitsData = scoreHits;
                    currentData.BallsLeft = GameManager.state.LivesLeft;
                    SaveBossStats();
                }
                else
                {
                    Debug.Log("No new high score");
                }
            }
        }

        private void AddHit(int powerGroupIndex)
        {
            if (powerGroupIndex > scoreHits.Length - 1) return;
            scoreHits[powerGroupIndex].Count++;
        }

        private int CalculateScore()
        {
            int score = 0;

            for (int i = 0; i < scoreHits.Length; i++)
            {
                score += scoreHits[i].Count * scoreHits[i].ScoreValue;
            }

            score += GameManager.state.LivesLeft * scorePerBallLeft;

            return score;
        }

        #region Displaying Score

        private void SetValues()
        {
            ClearTexts();

            float score = 0;
            livesLeftValue.text = GameManager.state.LivesLeft.ToString();
            livesLeftScore.text = (GameManager.state.LivesLeft * scorePerBallLeft).ToString("F0");
            score += GameManager.state.LivesLeft * scorePerBallLeft;

            for (int i = 0; i < scoreHits.Length; i++)
            {
                hitsValue.text += scoreHits[i].Count.ToString() + "\n \n";
                hitsName.text += scoreHits[i].Name + "\n \n";
                hitsScore.text += (scoreHits[i].Count * scoreHits[i].ScoreValue).ToString() + "\n \n";
                score += scoreHits[i].Count * scoreHits[i].ScoreValue;
            }

            finalScore.text = score.ToString();

        }

        private void ClearTexts()
        {
            livesLeftScore.text = "";
            livesLeftScore.text = "";
            hitsValue.text = "";
            hitsName.text = "";
            hitsScore.text = "";
            finalScore.text = "";
        }

        /// <summary>
        /// Testing method, do not use at runtime.
        /// </summary>
        [ContextMenu("Test score menu")]
        public void TestFinalScoreDisplay()
        {
            for (int i = 0; i < scoreHits.Length; i++)
            {
                scoreHits[i].Count = Random.Range(0, 11);
            }
            SetValues();
            scoreCanvas.SetActive(true);
        }

        #endregion

        #region Saving

        private void SaveBossStats()
        {
            if (!shouldSave) return;
            string jsonString = JsonUtility.ToJson(currentData);

            using (StreamWriter streamWriter = File.CreateText(dataPath))
            {
                streamWriter.Write(jsonString);
            }
        }

        private void LoadBossStats()
        {
            if (!shouldSave) return;
            if (!File.Exists(dataPath)) // no file found
            {
                // save
                Debug.Log("No saved file found for this boss, creating one...");
                currentData = new PlayerBossData();
                SaveBossStats();
            }
            else // load file
            {
                Debug.Log("File found for this boss");
                using (StreamReader streamReader = File.OpenText(dataPath))
                {
                    string jsonString = streamReader.ReadToEnd();
                    currentData = JsonUtility.FromJson<PlayerBossData>(jsonString);
                }
                Debug.Log("Last score : " + currentData.FinalScore + " with " + currentData.BallsLeft + " balls left");
            }
        }

        [ContextMenu("Delete saved file")]
        public void EraseBossStats()
        {
            if (!shouldSave) return;
            if (File.Exists(dataPath))
            {
                File.Delete(dataPath);
            }
        }

        #endregion

        public ScoreHit[] ScoreHits { get => scoreHits; set => scoreHits = value; }
    }

    [System.Serializable]
    public class ScoreHit
    {
        [SerializeField] private string name;
        [SerializeField] private int count;
        [SerializeField] private int scoreValue;

        public string Name { get => name; set => name = value; }
        public int Count { get => count; set => count = value; }
        public int ScoreValue { get => scoreValue; set => scoreValue = value; }
    }

    [System.Serializable]
    public class PlayerBossData
    {
        [SerializeField] private int finalScore = 0;
        [SerializeField] private ScoreHit[] hitsData;
        [SerializeField] private int ballsLeft = 4;

        public int FinalScore { get => finalScore; set => finalScore = value; }
        public ScoreHit[] HitsData { get => hitsData; set => hitsData = value; }
        public int BallsLeft { get => ballsLeft; set => ballsLeft = value; }
    }
}
