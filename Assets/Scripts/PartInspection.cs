using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class PartInspection : MonoBehaviour
{

    //Create variable for TEXT MESH PRO
    public TMP_Text TextCountourCoincidence, TextMaximumHeight, TextHeightDifference, TextPrismaticHeight, TextCylindricalHeight, TextAreaPixels, TextAreaMM;
    public TMP_Text Texthotend, Textzone1, Textzone2;

    //URL SHEETS  (inspection/thermal)
    string DataURL = "https://script.google.com/macros/s/AKfycbw0VZAH7AvHRlYr8oyspY-jI0lvG0S2jriK6-HcRQZ8bHj96ObEGujwd1KHcUVZ5f8z/exec";
    string DataURL1 = "https://script.google.com/macros/s/AKfycbwr7V7MRURC8Fn8KUIi_BplpxJJQ8FKhq6CVb18VbYpIHa1r7CSOtQcUO__5RK3bWeLuA/exec";

    void Start()
    {

    }

    //-------------------------------------------------------------Get Sheets Data-------------------------------------------------------------------------------
    public void GetPieceData()
    {
        StartCoroutine(GetData(DataURL, DataURL1));
    }

    //-------------------------------------------------------------Set Text in UI-------------------------------------------------------------------------------

    public void SetText(string text1, string text2, string text3, string text4, string text5, string text6, string text7, string text8, string text9, string text10)
    {

        float TextCount = float.Parse(text1);
        float TextMax = float.Parse(text2);
        float TextHeigth = float.Parse(text3);
        float TextPris = float.Parse(text4);
        float TextCyl = float.Parse(text5);
        float TextAreaP = float.Parse(text6);
        float TextAreaM = float.Parse(text7);


        if (TextCount >= 98.5)
        {
            TextCountourCoincidence.text = text1;
            TextCountourCoincidence.color = Color.green;
        }
        else
        {
            TextCountourCoincidence.text = text1;
            TextCountourCoincidence.color = Color.red;
        }

        if (TextMax >= 14.7 && TextMax <= 15.3)
        {
            TextMaximumHeight.text = text2;
            TextMaximumHeight.color = Color.green;
        }
        else
        {
            TextMaximumHeight.text = text2;
            TextMaximumHeight.color = Color.red;
        }

        if (TextHeigth >= -0.7 && TextHeigth <= 0.3)
        {
            TextHeightDifference.text = text3;
            TextHeightDifference.color = Color.green;
        }
        else
        {
            TextHeightDifference.text = text3;
            TextHeightDifference.color = Color.red;
        }

        if (TextPris >= 14.7 && TextPris <= 15.3)
        {
            TextPrismaticHeight.text = text4;
            TextPrismaticHeight.color = Color.green;
        }
        else
        {
            TextPrismaticHeight.text = text4;
            TextPrismaticHeight.color = Color.red;
        }

        if (TextAreaP >= 14.7 && TextAreaP <= 15.3)
        {
            TextCylindricalHeight.text = text5;
            TextCylindricalHeight.color = Color.green;
        }
        else
        {

            TextCylindricalHeight.text = text5;
            TextCylindricalHeight.color = Color.red;
        }

        if (TextCyl >= 65637 && TextCyl <= 66963)
        {
            TextAreaPixels.text = text6;
            TextAreaPixels.color = Color.green;
        }
        else
        {

            TextAreaPixels.text = text6;
            TextAreaPixels.color = Color.red;
        }

        if (TextAreaM >= 495 && TextAreaM <= 505)
        {
            TextAreaMM.text = text7;
            TextAreaMM.color = Color.green;
        }
        else
        {

            TextAreaMM.text = text7;
            TextAreaMM.color = Color.red;
        }

        Texthotend.text = text8;
        Textzone1.text = text9;
        Textzone2.text = text10;
    }


    //-------------------------------------------------------------Deseralize Sheets Data-------------------------------------------------------------------------------
    IEnumerator GetData(string url, string url1)
    {
        //Get Data-----------------------------------------------
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();
        UnityWebRequest request1 = UnityWebRequest.Get(url1);
        yield return request1.SendWebRequest();
      
        //Get JsonArray------------------------------------------
        var jsonData = request.downloadHandler.text;
        var jsonData1 = request1.downloadHandler.text;

        //Check if there is a error----------------------------------------------------------------------------------
        if (request.result != UnityWebRequest.Result.Success || request1.result != UnityWebRequest.Result.Success)
        {
            //error
            Debug.Log("error");
            TextCountourCoincidence.text = request.error;
        }
        else
        {
            //success
            Debug.Log(jsonData);
            Debug.Log(jsonData1);

            //Deserialize the JsonArray, us list because the []
            DataDes data = JsonUtility.FromJson<DataDes>(jsonData);
            DataDest Datat = JsonUtility.FromJson<DataDest>(jsonData1);

            //Length of deseialized data
            int datlen = Datat.ResultArray.Count;
            Debug.Log(datlen);

            //Create an array with length of deseialized data
            string[] textsco = new string[3];
            string[] textshei = new string[3];
            string[] textsdif = new string[3];
            string[] textspris = new string[3];
            string[] textscyl = new string[3];
            string[] textspix = new string[3];
            string[] textsmm = new string[3];

            //Create an array with length of deseialized data
            string[] texthotend = new string[datlen];
            string[] textzone1 = new string[datlen];
            string[] textzone2 = new string[datlen];
            string[] textzone3 = new string[datlen];

            float[] hotendvalues = new float[datlen];
            int[] hotendval = new int[datlen];

            //Set values in array-----------------------------------------------------
            for (int i = 0; i < 3; i++)
            {
                textsco[i] = data.ResultArray[i].CountourCoincidence;
                textshei[i] = data.ResultArray[i].MaximumHeight;
                textsdif[i] = data.ResultArray[i].HeightDifference;
                textspris[i] = data.ResultArray[i].PrismaticHeight;
                textscyl[i] = data.ResultArray[i].CylindricalHeight;
                textspix[i] = data.ResultArray[i].AreaPixels;
                textsmm[i] = data.ResultArray[i].AreaMM;
            }


            for (int k = 0; k < datlen; k++)
            {
                texthotend[k] = Datat.ResultArray[k].HotEndTemperature;
                textzone1[k] = Datat.ResultArray[k].PieceZone1;
                textzone2[k] = Datat.ResultArray[k].PieceZone2;
                textzone3[k] = Datat.ResultArray[k].PieceZone3;
                hotendvalues[k] = float.Parse(Datat.ResultArray[k].HotEndTemperature);
            }

            for (int i=0; i < datlen; i++)
            {
                hotendval[i] = (int)hotendvalues[i];
            }
         
            //Send Data to fuction--------------------------------------------------------------------------
            SetText(textsco[2], textshei[2], textsdif[2], textspris[2], textscyl[2], textspix[2], textsmm[2], texthotend[datlen - 1], textzone1[datlen - 1], textzone2[datlen - 1]);
            //Graph(hotendval);

        }
        request.Dispose();
    }

    //-----------------------------------Json array----------------------------------//
    [System.Serializable]
    public class DataDes
    {
        public List<RArray> ResultArray;
    }

    [System.Serializable]
    public class RArray
    {
        public string CountourCoincidence;
        public string MaximumHeight;
        public string HeightDifference;
        public string PrismaticHeight;
        public string CylindricalHeight;
        public string AreaPixels;
        public string AreaMM;
    }

    //-----------------------------------Json array----------------------------------//
    [System.Serializable]
    public class DataDest
    {
        public List<RArrayt> ResultArray;
        
    }

    [System.Serializable]
    public class RArrayt
    {
        public string HotEndTemperature;
        public string PieceZone1;
        public string PieceZone2;
        public string PieceZone3;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
