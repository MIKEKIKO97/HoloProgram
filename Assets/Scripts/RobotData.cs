using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;


public class RobotData : MonoBehaviour
{
    //----------------------------------------Variables and Url-------------------------------------------------//
    public TMP_Text TextX, TextY, TextZ;
    string DataURL = "https://script.google.com/macros/s/AKfycbzW1FDpw3-zujxHINDrbOzRTw2B-gJk7KnDCr2_ZLq0Ojnng8rcqmGT8pMmYFBdwsxP/exec";
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("Lauchgetdata", 0.1f, 3.0f);
    }

    void Lauchgetdata ()
    {
        StartCoroutine(GetData(DataURL));
    }

    //----------------------------------Get and deserialize Json-----------------------------------------------------//
    IEnumerator GetData(string url)
    {
        //Request Json Array of url Data
        UnityWebRequest requestdata = UnityWebRequest.Get(url);
        yield return requestdata.SendWebRequest();
        var dataJSON = requestdata.downloadHandler.text;

        if (requestdata.result != UnityWebRequest.Result.Success)
        {
            //There is an error
            Debug.Log("Error" + requestdata.error);
        }

        else
        {
            //Success
            Debug.Log("Success");
            //Debug.Log(imageJSON);
            Debug.Log(dataJSON);

            //Deserealize the JSON Array
            DataDes DataSUrl = JsonUtility.FromJson<DataDes>(dataJSON);

            //Length of deseialized data
            int lendata = DataSUrl.ResultArray.Count;


            //Create an array with length of deseialized data
            string text1 = DataSUrl.ResultArray[0].X;
            string text2 = DataSUrl.ResultArray[0].Y;
            string text3 = DataSUrl.ResultArray[0].Z;

            TextX.text = text1;
            TextY.text = text2;
            TextZ.text = text3;
            yield return new WaitForSeconds(6);
        }
    }

    //Deseralaize JSON Array to an object
    [System.Serializable]
    public class DataDes
    {
        public List<RArray> ResultArray;

    }

    [System.Serializable]
    public class RArray
    {
        public string X;
        public string Y;
        public string Z;
    }
}
