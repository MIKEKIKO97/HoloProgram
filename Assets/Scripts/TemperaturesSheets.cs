using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class TemperaturesSheets : MonoBehaviour
{
    //----------------------------------------Variables and Url-------------------------------------------------//
    //public TMP_Text[] Texthotend, Textzone1, Textzone2, Textzone3;
    public TMP_Text Textpromedioh,Textpromedioz;
    string DataURL = "https://script.google.com/macros/s/AKfycbwr7V7MRURC8Fn8KUIi_BplpxJJQ8FKhq6CVb18VbYpIHa1r7CSOtQcUO__5RK3bWeLuA/exec";
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void GetTemps()
    {
        StartCoroutine(GetData(DataURL));
    }

    //--------------------------------------------------Set Data in UI------------------------------------------------//
    public void SetText(string[] text1, string[] text2, string[] text3, int len, float promedioh, float promedioz)
    {
        /*for (int i = 0; i < 3; i++)
        {
            Texthotend[i].text = text1[len - (3 - i)];
            Textzone1[i].text = text2[len - (3 - i)];
            Textzone2[i].text = text3[len - (3 - i)];
        }*/

        Textpromedioh.text = promedioh.ToString();
        Textpromedioz.text = promedioz.ToString();

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
            Debug.Log("Error");
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
            string[] texthotend = new string[lendata];
            string[] textzone1 = new string[lendata];
            string[] textzone2 = new string[lendata];
            string[] textzone3 = new string[lendata];

            float promehotend = 0;
            float promzone1 = 0;

            //float TextMax = float.Parse(text2);


            for (int k = 0; k < lendata; k++)
            {
                texthotend[k] = DataSUrl.ResultArray[k].HotEndTemperature;
                textzone1[k] = DataSUrl.ResultArray[k].PieceZone1;
                textzone2[k] = DataSUrl.ResultArray[k].PieceZone2;
                textzone3[k] = DataSUrl.ResultArray[k].PieceZone3;

                float hotend = float.Parse(texthotend[k]);
                promehotend += hotend;
                float zone1 = float.Parse(textzone1[k]);
                promzone1 += zone1;
            }
            float promediohotend = promehotend / lendata;
            float promediozone1 = promzone1/ lendata;

            SetText(texthotend, textzone1, textzone2, lendata, promediohotend, promediozone1);
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
        public string HotEndTemperature;
        public string PieceZone1;
        public string PieceZone2;
        public string PieceZone3;
    }
}
