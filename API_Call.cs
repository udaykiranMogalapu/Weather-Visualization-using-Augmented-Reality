using Newtonsoft.Json;
using TMPro;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections.Generic;



public class API_Call : MonoBehaviour
{
    public class Clouds
    {
        public int all { get; set; }
    }

    public class Coord
    {
        public double lon { get; set; }
        public double lat { get; set; }
    }

    public class Main
    {
        public double temp { get; set; }
        public double feels_like { get; set; }
        public double temp_min { get; set; }
        public double temp_max { get; set; }
        public int pressure { get; set; }
        public int humidity { get; set; }
    }

    public class Root
    {
        public Coord coord { get; set; }
        public List<Weather> weather { get; set; }
        public string @base { get; set; }
        public Main main { get; set; }
        public int visibility { get; set; }
        public Wind wind { get; set; }
        public Clouds clouds { get; set; }
        public int dt { get; set; }
        public Sys sys { get; set; }
        public int timezone { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public int cod { get; set; }
    }

    public class Sys
    {
        public int type { get; set; }
        public int id { get; set; }
        public string country { get; set; }
        public int sunrise { get; set; }
        public int sunset { get; set; }
    }

    public class Weather
    {
        public int id { get; set; }
        public string main { get; set; }
        public string description { get; set; }
        public string icon { get; set; }
    }

    public class Wind
    {
        public double speed { get; set; }
        public int deg { get; set; }
    }
    


    public TextMeshProUGUI text1;
    public TextMeshProUGUI text3;
    public GameObject light_rain;
    public GameObject heavy_rain;
    public GameObject extreme_rain;
    public GameObject Sun;
    public GameObject Cloud;
    
    void Start()
    {
        StartCoroutine(GetRequest("https://api.openweathermap.org/data/2.5/weather?lat=16.475&lon=80.696&appid=291f12c4f3ca7a409a314ec6507ba770"));
      
    }
 

    IEnumerator GetRequest(String uri)
    {
        using(UnityWebRequest webRequest=UnityWebRequest.Get(uri))
        {
            yield return webRequest.SendWebRequest();

            switch(webRequest.result)
            {
                
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(String.Format("something went wrong: {0}",webRequest.error));
                    break;
                case UnityWebRequest.Result.Success:
                    Root root= JsonConvert.DeserializeObject<Root>(webRequest.downloadHandler.text);
                    text1.text="Location:"+root.name;
                    text3.text="Weather:"+root.weather[0].description;
                    if(root.weather[0].description=="light rain" || root.weather[0].description=="moderate rain")
                    {
                        Sun.SetActive(false);
                        Cloud.SetActive(true);
                       light_rain.SetActive(true);
                       heavy_rain.SetActive(false);
                       extreme_rain.SetActive(false);

                       
                    }
                    else if(root.weather[0].description=="heavy intensity rain" || root.weather[0].description=="very heavy rain")
                    {
                        Sun.SetActive(false);
                        Cloud.SetActive(false);
                       heavy_rain.SetActive(true);
                       light_rain.SetActive(false);
                       extreme_rain.SetActive(false);
                    }
                    else if(root.weather[0].description=="few clouds" || root.weather[0].description=="overcast clouds" || root.weather[0].description=="broken clouds")
                    {
                        Sun.SetActive(false);
                        Cloud.SetActive(true);
                       extreme_rain.SetActive(false);
                       heavy_rain.SetActive(false);
                        light_rain.SetActive(false);
                    }
                    else if(root.weather[0].description=="clear sky" || root.weather[0].description=="haze")
                    {
                        Sun.SetActive(true);
                        Cloud.SetActive(false);
                       extreme_rain.SetActive(false);
                       heavy_rain.SetActive(false);
                        light_rain.SetActive(false);
                    }
                    else{
                        Sun.SetActive(true);
                        Cloud.SetActive(true);
                       extreme_rain.SetActive(false);
                       heavy_rain.SetActive(false);
                        light_rain.SetActive(false);
                    }
                    break;

                default: Debug.LogError("Switch Case failed");
                    break;
            }
        }
    }

}