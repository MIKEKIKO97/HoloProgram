using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;


public class InspectionSheets : MonoBehaviour
{
    //-----------------------------------------------------------Variables and URL Data-----------------------------------------------------------------------------------//
    public TMP_Text TextCountourCoincidence, TextMaximumHeight, TextHeightDifference, TextPrismaticHeight, TextCylindricalHeight, TextAreaPixels, TextAreaMM, TextsCountourCoincidence1, TextsMaximumHeight1, TextsHeightDifference1, TextsPrismaticHeight1, TextsCylindricalHeight1, TextsAreaPixels1, TextsAreaMM1;
    string DataURL = "https://script.google.com/macros/s/AKfycbw0VZAH7AvHRlYr8oyspY-jI0lvG0S2jriK6-HcRQZ8bHj96ObEGujwd1KHcUVZ5f8z/exec";

    void Start()
    {

    }

    //-------------------------------------------------------------GetData--------------------------------------------------------------------------//
    public void GetPieceData()
    {
        StartCoroutine(GetData(DataURL));
    }

    //---------------------------------------------------------Set Data en UI-----------------------------------------------------------------------------//
    public void SetText(string text1, string text2, string text3, string text4, string text5, string text6, string text7)
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

        if (TextCyl >= 14.7 && TextCyl <= 15.3)
        {
            TextCylindricalHeight.text = text5;
            TextCylindricalHeight.color = Color.green;
        }
        else
        {
            TextCylindricalHeight.text = text5;
            TextCylindricalHeight.color = Color.red;
        }

        if (TextAreaP >= 65637 && TextAreaP <= 66963)
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

    }

    //---------------------------------------------------------Set Data en UI-----------------------------------------------------------------------------//
    public void SetText3(string[] text1, string[] text2, string[] text3, string[] text4, string[] text5, string[] text6, string[] text7)
    {
        int C1 = 0;
        int C2 = 0;
        int C3 = 0;
        int C4 = 0;
        int C5 = 0;
        int C6 = 0;
        int C7 = 0;

        Debug.Log("#numer");

        for (int j=0; j < 10; j++)
        {
            float TextCount = float.Parse(text1[j]);
            if (TextCount >= 98.5)
            {
                C1++;
            }

            float TextMax = float.Parse(text2[j]);
            if (TextMax >= 14.7 && TextMax <= 15.3)
            {
                C2++;
            }

            float TextHeigth = float.Parse(text3[j]);
            if (TextHeigth >= -0.7 && TextHeigth <= 0.3)
            {
                C3++;
            }

            float TextPris = float.Parse(text4[j]);
            if (TextPris >= 14.7 && TextPris <= 15.3)
            {
                C4++;
            }

            float TextCyl = float.Parse(text5[j]);
            if (TextCyl >= 14.7 && TextCyl <= 15.3)
            {
                C5++;
            }

            float TextAreaP = float.Parse(text6[j]);
            if (TextAreaP >= 65637 && TextAreaP <= 66963)
            {
                C6++;
            }

            float TextAreaM = float.Parse(text7[j]);
            if (TextAreaM >= 495 && TextAreaM <= 505)
            {
                C7++;
            }

        }
        TextsCountourCoincidence1.text = C1.ToString();
        TextsMaximumHeight1.text = C2.ToString();
        TextsHeightDifference1.text = C3.ToString();
        TextsPrismaticHeight1.text = C4.ToString();
        TextsCylindricalHeight1.text = C5.ToString();
        TextsAreaPixels1.text = C6.ToString();
        TextsAreaMM1.text = C7.ToString();
    }

    //-----------------------------------------------------------------Get Data and Deserialize Json-----------------------------------------------------------------------------//
    IEnumerator GetData(string url)
    {
        //--------------Get JsonArray----------------------
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();
        var jsonData = request.downloadHandler.text;

        if (request.result != UnityWebRequest.Result.Success)
        {
            //error
            TextCountourCoincidence.text = request.error;
            Debug.Log("error");
        }
        else
        {
            //success
            Debug.Log(jsonData);

            //Deserialize the JsonArray, us list because the []
            DataDes data = JsonUtility.FromJson<DataDes>(jsonData);
            int len = data.ResultArray.Count;
            //Create an array with length of deseialized data
            string[] textsco = new string[len];
            string[] textshei = new string[len];
            string[] textsdif = new string[len];
            string[] textspris = new string[len];
            string[] textscyl = new string[len];
            string[] textspix = new string[len];
            string[] textsmm = new string[len];



            for (int i = 0; i < len; i++) 
            {
                textsco [i] = data.ResultArray[i].CountourCoincidence;
                textshei [i] = data.ResultArray[i].MaximumHeight;
                textsdif [i] = data.ResultArray[i].HeightDifference;
                textspris [i] = data.ResultArray[i].PrismaticHeight;
                textscyl [i] = data.ResultArray[i].CylindricalHeight;
                textspix [i] = data.ResultArray[i].AreaPixels;
                textsmm [i] = data.ResultArray[i].AreaMM;

            }
            Debug.Log(textsmm[len-1]);

            //----Call functions-----------------------//
            SetText(textsco[len-1], textshei[len-1], textsdif[len-1], textspris[len-1], textscyl[len-1], textspix[len-1], textsmm[len-1]);
            SetText3(textsco, textshei, textsdif, textspris, textscyl, textspix, textsmm);
        }
    }

    //------------------------------Json--------------------------//
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

    // Update is called once per frame
    void Update()
    {

    }

}
