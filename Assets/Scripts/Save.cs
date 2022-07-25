using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System;
/// <summary>
/// Скрипт, который занимается сохранением и считыванием таблицы рекордов с файла
/// </summary>
public class Save : MonoBehaviour
{
    public static Save Instance;
    public event Action OnSaveDataEnd = delegate { };

    [Serializable]
    public class SaveData
    {
        public string Date;
        public string Score;
    }

    [Serializable]
    private class SavedDataWrapper
    {
        public List<SaveData> SavedDatas;
    }

    private string FilePath => Path.Combine(Application.persistentDataPath, "data.csv");

    private long currentScore;

    [SerializeField]
    private int listLimit;

    public List<SaveData> SavedDatas {get; private set;}

    public int CurrentRecordPos {get; private set;}


    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        SavedDatas = new List<SaveData>();
        LoadFromFile();
        DontDestroyOnLoad(gameObject);

    }
    private void OnEnable()
    {
        TryManager.OnEndGame += EndGameAction;
        ScoreManager.OnValueChanged += SetCurrentScore;
    }

    private void OnDisable()
    {
        TryManager.OnEndGame -= EndGameAction;
        ScoreManager.OnValueChanged -= SetCurrentScore;
    }

    private void SetCurrentScore(long score) 
    {
        currentScore = score;
    }

    private void EndGameAction()
    {
        var NewRecord = new SaveData
        {
            Date = DateTime.Now.ToString("MM/dd/yyyy HH:mm"),
            Score = currentScore.ToString()
        };

        InsertNewRecord(NewRecord);
        CheckTail();
        SaveToFile();
        OnSaveDataEnd();
    }

    private void InsertNewRecord(SaveData NewRecord)
    {
        for (int i = SavedDatas.Count - 1; i >= 0; i--)
        {
            if (long.Parse(SavedDatas[i].Score) >= long.Parse(NewRecord.Score))
            {
                SavedDatas.Insert(i + 1, NewRecord);
                CurrentRecordPos = i + 2;
                return;
            }
        }
        SavedDatas.Insert(0, NewRecord);
        CurrentRecordPos = 1;
    }

    private void CheckTail()
    {
        while (SavedDatas.Count > listLimit)
        {
            SavedDatas.Remove(SavedDatas[SavedDatas.Count - 1]);
        }
    }

    private SavedDataWrapper GetWrapper()
    {
        return (new SavedDataWrapper
        {
            SavedDatas = SavedDatas
        });
    }

    private void LoadFromFile()
    {
        if (!File.Exists(FilePath))
        {
            return;
        }

        var binaryFormatter = new BinaryFormatter();
        using (FileStream fileStream = File.Open(FilePath, FileMode.OpenOrCreate))
        {
            var wrapper = (SavedDataWrapper)binaryFormatter.Deserialize(fileStream);
            SavedDatas = wrapper.SavedDatas;
        }
    }

    private void SaveToFile()
    {
        var wrapper = GetWrapper();

        var binaryFormatter = new BinaryFormatter();
        using (FileStream fileStream = File.Open(FilePath, FileMode.OpenOrCreate))
        {
            binaryFormatter.Serialize(fileStream, wrapper);
        }

    }
}

