﻿using System.Collections;
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
        [SerializeField] private float incSpeed = 5f;
        [SerializeField] private float incWait = 0.01f;
        [Space(5f)]
        [SerializeField] private float scoreLineSpacing = 50f;
        [SerializeField] private GameObject originalHitsValueText;
        [SerializeField] private GameObject originalHitsNameText;
        [SerializeField] private GameObject originalHitsScoreText;
        [Space(5f)]
        [SerializeField] private TextMeshProUGUI livesLeftValue;
        [SerializeField] private TextMeshProUGUI livesLeftScore;
        [SerializeField] private TextMeshProUGUI[] hitsValue;
        [SerializeField] private TextMeshProUGUI[] hitsName;
        [SerializeField] private TextMeshProUGUI[] hitsScore;
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
            Debug.Log("hit received");
            if (powerGroupIndex > scoreHits.Length - 1) return;
            Debug.Log("hit added");
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

        /// <summary>
        /// Building method, do not use at runtime.
        /// </summary>
        [ContextMenu ("Organize displays")]
        public void OrganizeDsiplays()
        {

            hitsValue = new TextMeshProUGUI[scoreHits.Length];
            hitsName = new TextMeshProUGUI[scoreHits.Length];
            hitsScore = new TextMeshProUGUI[scoreHits.Length];

            hitsValue[0] = originalHitsValueText.GetComponent<TextMeshProUGUI>();
            hitsName[0] = originalHitsNameText.GetComponent<TextMeshProUGUI>();
            hitsScore[0] = originalHitsScoreText.GetComponent<TextMeshProUGUI>();

            for (int i = 1; i < scoreHits.Length; i++)
            {
                hitsValue[i] = GameObject.Instantiate(hitsValue[0], new Vector3(hitsValue[0].rectTransform.position.x, hitsValue[0].rectTransform.position.y - (i * scoreLineSpacing), 0f), Quaternion.identity, scoreCanvas.transform);
                hitsName[i] = GameObject.Instantiate(hitsName[0], new Vector3(hitsName[0].rectTransform.position.x, hitsName[0].rectTransform.position.y - (i * scoreLineSpacing), 0f), Quaternion.identity, scoreCanvas.transform);
                hitsScore[i] = GameObject.Instantiate(hitsScore[0], new Vector3(hitsScore[0].rectTransform.position.x, hitsScore[0].rectTransform.position.y - (i * scoreLineSpacing), 0f), Quaternion.identity, scoreCanvas.transform);
            }

        }

        private IEnumerator DynamicScoreDisplay()
        {
            float scoreInc = 0f;
            // livesleftValue and score
            livesLeftValue.text = (4 - GameManager.state.LivesLeft).ToString();
            while (scoreInc < 4 * scorePerBallLeft)
            {
                scoreInc += Time.deltaTime * incSpeed;
                livesLeftScore.text = scoreInc.ToString("F0");
                yield return new WaitForSeconds(incWait);
            }
            scoreInc = 4 * scorePerBallLeft;
            livesLeftScore.text = scoreInc.ToString("F0");
            yield return new WaitForSeconds(incWait);

            //Hits scores
            scoreInc = 0f;
            for (int i = 0; i < scoreHits.Length; i++)
            {
                hitsValue[i].text = scoreHits[i].Count.ToString();
                while(scoreInc < scoreHits[i].Count * scoreHits[i].ScoreValue)
                {
                    scoreInc += Time.deltaTime * incSpeed;
                    hitsScore[i].text = scoreInc.ToString("F0");
                    yield return new WaitForSeconds(incWait);
                }
                scoreInc = 0f;
                scoreInc = scoreHits[i].Count * scoreHits[i].ScoreValue;
                hitsScore[i].text = scoreInc.ToString("F0");
                yield return new WaitForSeconds(incWait);
            }

            // final score
            float score = CalculateScore();
            scoreInc = 0f;

            while(scoreInc < score)
            {
                scoreInc += scoreInc += Time.deltaTime * incSpeed;
                finalScore.text = scoreInc.ToString("F0");
                yield return new WaitForSeconds(incWait);
            }
            finalScore.text = score.ToString();
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
            scoreCanvas.SetActive(true);
            StartCoroutine(DynamicScoreDisplay());
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
        [SerializeField] private string name = "hit";
        [SerializeField] private int count = 0;
        [SerializeField] private int scoreValue = 10;

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

        public PlayerBossData()
        {
            hitsData = new ScoreHit[4];
        }

        public int FinalScore { get => finalScore; set => finalScore = value; }
        public ScoreHit[] HitsData { get => hitsData; set => hitsData = value; }
        public int BallsLeft { get => ballsLeft; set => ballsLeft = value; }
    }
}
