using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;

    #region 데이터 생성
    public SoundData soundData = new SoundData();
    public KeyData keyData = new KeyData();
    #endregion

    #region 경로 지정
    private string _path;
    private string _soundFileName = "/SoundData.json";
    private string _keyFileName = "/KeyData.json";
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

            keyData.up = KeyCode.W;
            keyData.down = KeyCode.S;
            keyData.left = KeyCode.A;
            keyData.right = KeyCode.D;
            keyData.dash = KeyCode.Space;
            keyData.inven = KeyCode.E;
            keyData.action = KeyCode.F;
            keyData.map = KeyCode.M;
            SaveKey();
        }
        #endregion
        #region 있으면 불러오기
        else
        {
            LoadOption();
            LoadKey();
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

    #region 키 데이터
    public void SaveKey()
    {
        string data = JsonUtility.ToJson(keyData);
        File.WriteAllText(_path + _keyFileName, data);
    }

    public void LoadKey()
    {
        string data = File.ReadAllText(_path + _keyFileName);
        keyData = JsonUtility.FromJson<KeyData>(data);
    }
    #endregion

    public void DataClear()
    {
        soundData = new SoundData();
        keyData = new KeyData();
        SaveOption();
        SaveKey();
    }

    public bool GetDir()
    {
        return Directory.Exists(_path);
    }
}
