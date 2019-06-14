using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;

namespace Team17.StreetHunt
{
    public class MainMenuSaveLoader : MonoBehaviour
    {
        [SerializeField] private string[] fileNames;

        [SerializeField] private TextMeshProUGUI highScoreText;
        [SerializeField] private TextMeshProUGUI smallHitsText;
        [SerializeField] private TextMeshProUGUI mediumHitsText;
        [SerializeField] private TextMeshProUGUI bigHitsText;
        [SerializeField] private TextMeshProUGUI epicHitsText;
        [SerializeField] private TextMeshProUGUI usedBallsText;

        private string dataPath;
        private PlayerBossData loadedData;

        public void DisplayDataFromSave(int index)
        {
            dataPath = Path.Combine(Application.persistentDataPath, fileNames[index] + ".tds");
            if (!File.Exists(dataPath)) // no file found
            {
                // save
                Debug.Log("No saved file found for " + fileNames[index] + ", creating one...");
                SaveBossStats();
            }
            else
            {
                // load
                Debug.Log("File found for " + fileNames[index]);
                LoadBossStats();
            }

            highScoreText.text = loadedData.FinalScore.ToString();
            smallHitsText.text = loadedData.HitsData[0].Count.ToString();
            mediumHitsText.text = loadedData.HitsData[1].Count.ToString();
            bigHitsText.text = loadedData.HitsData[2].Count.ToString();
            epicHitsText.text = loadedData.HitsData[3].Count.ToString();
            usedBallsText.text = "x " + (5 - loadedData.BallsLeft).ToString();
        }

        private void LoadBossStats()
        {
            using (StreamReader streamReader = File.OpenText(dataPath))
            {
                string jsonString = streamReader.ReadToEnd();
                loadedData = JsonUtility.FromJson<PlayerBossData>(jsonString);
            }
        }

        private void SaveBossStats()
        {
            PlayerBossData newData = new PlayerBossData();
            loadedData = newData;
            string jsonString = JsonUtility.ToJson(newData);

            using (StreamWriter streamWriter = File.CreateText(dataPath))
            {
                streamWriter.Write(jsonString);
            }
        }

        [ContextMenu("Delete saved file")]
        public void EraseBossStats()
        {
            for (int i = 0; i < fileNames.Length; i++)
            {
                string path = Path.Combine(Application.persistentDataPath, fileNames[i] + ".tds");
                if (File.Exists(path))
                {
                    Debug.Log("Deleted " + fileNames[i]);
                    File.Delete(path);
                }
            }
        }
    }
}
