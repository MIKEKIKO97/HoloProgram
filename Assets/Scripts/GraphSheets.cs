using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using CodeMonkey.Utils;

public class GraphSheets : MonoBehaviour
{
    
    //URL SHEETS
    string DataURL = "https://script.google.com/macros/s/AKfycbzBLtQP557b3qgq6_OOwWAzzCf9GEfeoIvD006OzGBqNzRsm-2oHbw-B6c_SyDskgsq/exec";
    string DataURL1 = "https://script.google.com/macros/s/AKfycbx0acjTLhRdIUIQZecLGNr1Y-b6go2bcptfyc5Pa471jWn18fTEMh1Wj913y74fuG2KKA/exec";

    //Variables for elements of graph
    [SerializeField] private Sprite circleSprite;
    private RectTransform graphContainer;

    void Start()
    {
 
    }

    //-------------------------------------------------------------Show Graph-------------------------------------------------------------------------------
    public void SetGraph(List<int> values)
    {
        graphContainer = transform.Find("graphContainer").GetComponent<RectTransform>();
        //List<int> valueList = new List<int> { 257, 259, 258, 259, 261 };
        ShowGraph(values);
        Debug.Log("exito2");
    }
    private GameObject CreateCircle(Vector2 anchoredPosition)
    {
        GameObject gameObject = new GameObject("circle", typeof(Image));
        gameObject.transform.SetParent(graphContainer, false);
        gameObject.GetComponent<Image>().sprite = circleSprite;
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(0.05f, 0.05f);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        return gameObject;
    }

    private void ShowGraph(List<int> valueList)
    {
        float graphHeight = graphContainer.sizeDelta.y;
        float yMaximum = 300f;
        float xSize = 0.1f;

        GameObject lastCircleGameObject = null;
        for (int i = 0; i < valueList.Count; i++)
        {
            float xPosition = xSize + i * xSize;
            float yPosition = (valueList[i] / yMaximum) * graphHeight;
            GameObject circleGameObject = CreateCircle(new Vector2(xPosition, yPosition));
            if (lastCircleGameObject != null)
            {
                CreateDotConnection(lastCircleGameObject.GetComponent<RectTransform>().anchoredPosition, circleGameObject.GetComponent<RectTransform>().anchoredPosition);
            }
            lastCircleGameObject = circleGameObject;
        }
    }

    private void CreateDotConnection(Vector2 dotPositionA, Vector2 dotPositionB)
    {
        GameObject gameObject = new GameObject("dotConnection", typeof(Image));
        gameObject.transform.SetParent(graphContainer, false);
        gameObject.GetComponent<Image>().color = new Color(1, 1, 1, .5f);
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        Vector2 dir = (dotPositionB - dotPositionA).normalized;
        float distance = Vector2.Distance(dotPositionA, dotPositionB);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        rectTransform.sizeDelta = new Vector2(distance, 0.051f);
        rectTransform.anchoredPosition = dotPositionA + dir * distance * .5f;
        rectTransform.localEulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVectorFloat(dir));
    }

    //-------------------------------------------------------------Get Sheets Data-------------------------------------------------------------------------------
    public void GetPieceData()
    {
        StartCoroutine(GetData(DataURL, DataURL1));
    }

    //-------------------------------------------------------------Deseralize Sheets Data-------------------------------------------------------------------------------
    IEnumerator GetData(string url, string url1)
    {
        //-----------Get Data-----------------------------------------------
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();
        UnityWebRequest request1 = UnityWebRequest.Get(url1);
        yield return request1.SendWebRequest();

        //-----------Get JsonArray------------------------------------------
        var jsonData = request.downloadHandler.text;
        var jsonData1 = request1.downloadHandler.text;

        //Check if there is a error----------------------------------------------------------------------------------
        if (request.result != UnityWebRequest.Result.Success || request1.result != UnityWebRequest.Result.Success)
        {
            //error
            Debug.Log("error");
        }
        else
        {
            //Deserialize the JsonArray, us list because the []
            DataDes data = JsonUtility.FromJson<DataDes>(jsonData);
            DataDest Datat = JsonUtility.FromJson<DataDest>(jsonData1);

            //Length of deseialized data
            int datlen = Datat.ResultArray.Count;

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
            //int[] hotendval = new int[datlen];
            List<int> graphvalues = new List<int>();

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

            for (int i = 0; i < datlen; i++)
            {
                //hotendval[i] = (int)hotendvalues[i];
                graphvalues.Add((int)hotendvalues[i]);
            }

            SetGraph(graphvalues);

            Debug.Log("exito");

        }
        request.Dispose();
    }

    //---------------Json Array-------------------------------//
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
