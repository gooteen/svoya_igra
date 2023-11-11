using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;
using UnityEngine.Networking;

public class TestScript : MonoBehaviour
{
    public TMP_Text text;
    public VideoPlayer player;
    public Image image;

    void Start()
    {
        //ReadString();
        //ReadCSV();
        //LoadVideo();
        //string path = Application.dataPath + "/image.png";
        //StartCoroutine(DownloadImage("file://" + path));
        /*
        TestSerialization test = new TestSerialization();
        string saveFile = Application.dataPath + "/gamedata.json";
        string jsonString = JsonUtility.ToJson(test);
        Debug.Log(jsonString);
        File.WriteAllText(saveFile, jsonString);
        */
    }

    IEnumerator DownloadImage(string MediaUrl)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(MediaUrl);
        yield return request.SendWebRequest();
        if (request.isNetworkError || request.isHttpError)
        {
            // файл не найден
            Debug.Log(request.error);
        }
        else
        {
            Texture2D tex = ((DownloadHandlerTexture)request.downloadHandler).texture;
            Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(tex.width / 2, tex.height / 2));
            image.overrideSprite = sprite;
        }
    }

    void Update()
    {
        
    }

    public void LoadVideo()
    {
        string path = Application.dataPath + "/video.mp4";

        player.url = "file://" + path;
        player.Play();
    }

    public void ReadCSV()
    {
        string path = Application.dataPath + "/test.csv";
        Regex parser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
        if (File.Exists(path))
        {
            StreamReader reader = new StreamReader(path, System.Text.Encoding.Default);
            string var = reader.ReadToEnd();
            string[] list = parser.Split(var);
            for (int i = 1; i < list.Length; i++)
            {
                text.text = list[i];
                Debug.Log(list[i]);
            }
        }
        else
        {
            Debug.Log("Something went wrong");
        }
    }

    public void ReadString()
    {
        string path = Application.dataPath + "/test.txt";
        if (File.Exists(path))
        {
            StreamReader reader = new StreamReader(path);
            string var = reader.ReadToEnd();
            Debug.Log(var);
            text.text = var;
            reader.Close();
        } else
        {
            Debug.Log("Fuck");
        }

    }
}
