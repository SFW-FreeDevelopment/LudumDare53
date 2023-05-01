using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Proyecto26;
using UnityEngine;

namespace LD53.Clients
{
    public class ApiClient
    {
        var baseURL = "https://ludumdare53api.azurewebsites.net/";
        public static void ProcessGameResults(GameResults gameResults, Action<Player> successCallback)
        {
            var url = baseURL + "/processGameResults";
            Debug.Log(url);
            var json = JsonConvert.SerializeObject(gameResults);
            Debug.Log(json);
            RestClient.Post(url, json)
                .Then(response =>
                {
                    Debug.Log("Request successful");
                    var deserializedPlayer = JsonConvert.DeserializeObject<Player>(response.Text);
                    successCallback(deserializedPlayer);
                    Debug.Log(JsonConvert.SerializeObject(deserializedPlayer, Formatting.Indented));
                })
                .Catch(error =>
                {
                    Debug.Log("Request failed");
                    Debug.Log(error?.InnerException?.Message ?? "");
                    Debug.Log($"Could not fetch the player.{Environment.NewLine}{error?.Message}");
                });
        }
        
        public static void FetchTop10(Action<List<object>> successCallback)
        {
            var url = baseURL + "/players/getTopTen";
            Debug.Log(url);
        
            RestClient.Get(url)
                .Then(response =>
                {
                    Debug.Log("Request successful");
                    var deserializedPlayers = JsonConvert.DeserializeObject<List<Player>>(response.Text);
                    successCallback(deserializedPlayers);
                    Debug.Log(JsonConvert.SerializeObject(deserializedPlayers, Formatting.Indented));
                })
                .Catch(error =>
                {
                    Debug.Log("Request failed");
                    Debug.Log(error?.InnerException?.Message ?? "");
                    Debug.Log($"Could not fetch the players.{Environment.NewLine}{error?.Message}");
                });
        }
    }
}