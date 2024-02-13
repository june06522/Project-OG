using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;

    #region 데이터 생성
    public SoundData soundData = new SoundData();
    #endregion

    #region 경로 지정
    private string _path;
    private string _soundFileName = "/SoundData.json";
    #endregion

    private void Awake()
    {
        #region 예외처리
        if (Instance == null)
        {
            Instance = FindObjectOfType<DataManager>();
            DontDestroyOnLoad(this);
        }
        else
            Destroy(gameObject);

        #endregion

        _path = Path.Combine(Application.persistentDataPath, "save");

        JsonLoad();
    }

    #region 데이터 관리
    public void JsonLoad()
    {
        #region 경로가 없으면 생성
        if (!GetDir())
        {
            Directory.CreateDirectory(_path);
            soundData.MasterSoundVal = 0.5f;
            soundData.BGMSoundVal = 0.5f;
            soundData.SFXSoundVal = 0.5f;
            SaveOption();
        }
        #endregion
        #region 있으면 불러오기
        else
        {
            LoadOption();
        }
        #endregion
    }
    #endregion

    #region 옵션 데이터
    public void SaveOption()
    {
        string data = JsonUtility.ToJson(soundData);
        File.WriteAllText(_path + _soundFileName, data);
    }

    public void LoadOption()
    {
        string data = File.ReadAllText(_path + _soundFileName);
        soundData = JsonUtility.FromJson<SoundData>(data);
    }
    #endregion

    public void DataClear()
    {
        soundData = new SoundData();
        SaveOption();
    }

    public bool GetDir()
    {
        return Directory.Exists(_path);
    }
}
