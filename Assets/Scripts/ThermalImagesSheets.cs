using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;

public class ThermalImagesSheets : MonoBehaviour
{
    //Variables UI
    public RawImage image_drive;
    public TMP_Text Texthotend, Textzone1;

    //Url AppScript
    string ImageURL = "https://script.google.com/macros/s/AKfycbxj9WNlhVwo18eQvyonxAJ14_zhTowipn_i6xx9OZzxxQ9TagU7iTX9uWaXBG-UEH4/exec";
    string DataURL = "https://script.google.com/macros/s/AKfycbwr7V7MRURC8Fn8KUIi_BplpxJJQ8FKhq6CVb18VbYpIHa1r7CSOtQcUO__5RK3bWeLuA/exec";



    // Start is called before the first frame update
    void Start()
    {

    }

    public void StartImages()
    {
        StartCoroutine(GetData(ImageURL, DataURL));
    }

    //---------------------------------------------------------------------------------Get URLs from Drive-----------------------------------------------------------------------------------------------//

    //Connect with App Script
    IEnumerator GetData(string url1, string url2)
    {
        //Request Json Array of url image and video
        UnityWebRequest request = UnityWebRequest.Get(url1);
        yield return request.SendWebRequest();
        var imageJSON = request.downloadHandler.text;

        //Request Json Array of url Data
        UnityWebRequest requestdata = UnityWebRequest.Get(url2);
        yield return requestdata.SendWebRequest();
        var dataJSON = requestdata.downloadHandler.text;

        if (request.result != UnityWebRequest.Result.Success || requestdata.result != UnityWebRequest.Result.Success)
        {
            //There is an error
            Debug.Log("Erro0imgr");
        }

        else
        {
            //Success
            Debug.Log("Successimg");
            Debug.Log(imageJSON);

            //Deserealize the JSON Array
            ImagenDeserealize ImgVidUrl = JsonUtility.FromJson<ImagenDeserealize>(imageJSON);
            DataDes DataSUrl = JsonUtility.FromJson<DataDes>(dataJSON);
            //var ImgVidUrl = JsonConvert.DeserializeObject<ImagenDeserealize>(imageJSON);

            //Length of deseialized data
            int len = ImgVidUrl.ImgArray.Count;
            int lendata = DataSUrl.ResultArray.Count;
            Debug.Log(len);
            Debug.Log(lendata);
            //Create an array with length of deseialized data
            string[] texturl = new string[len];
            string[] texthotend = new string[lendata];
            string[] textzone1 = new string[lendata];
            string[] textzone2 = new string[lendata];
            string[] textzone3 = new string[lendata];


            //Get all the URLs of the array Image and Data
            for (int i = 0; i < len; i++)
            {
                texturl[i] = ImgVidUrl.ImgArray[i].Images;
            }
            //Debug.Log(texturl[0]);

            for (int k = 0; k < lendata; k++)
            {
                texthotend[k] = DataSUrl.ResultArray[k].HotEndTemperature;
                textzone1[k] = DataSUrl.ResultArray[k].PieceZone1;
                textzone2[k] = DataSUrl.ResultArray[k].PieceZone2;
                //textzone3[k] = DataSUrl.ResultArray[k].PieceZone3;
            }


            //Send each URL to download image
            for (int j = 0; j < lendata; j++)
            {
                StartCoroutine(GetImage(texturl[j]));
                //Wait # seconds
                yield return new WaitForSeconds(3);
                Texthotend.text = texthotend[j];
                Textzone1.text = textzone1[j];
            }
        }

    }

    //----------------------------------------------------------------------------JSON Data-----------------------------------------------------------------------------------------------//
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

    //----------------------------------------------------------------------------JSON Image -----------------------------------------------------------------------------------------------//
    //Deseralaize JSON Array to an object
    [System.Serializable]
    public class ImagenDeserealize
    {
        public List<ListUrlImagen> ImgArray;
    }

    [System.Serializable]
    public class ListUrlImagen
    {
        public string Images;
    }


    //----------------------------------------------------------------------------Download Images and Send it to UI Texture-----------------------------------------------------------------------------------------------//
    //Download images from drive
    IEnumerator GetImage(string urlim)
    {

        //Debug.Log(urlim);
        //Get each Image Url
        UnityWebRequest requestr = UnityWebRequestTexture.GetTexture(urlim);
        yield return requestr.SendWebRequest();
        image_drive.texture = DownloadHandlerTexture.GetContent(requestr);
        
    }
}
   
