using System;
using System.Collections.Generic;
using LudumDare53.API.Models;
using Newtonsoft.Json;
using Proyecto26;
using UnityEngine;

namespace LD53.Clients
{
    public class ApiClient
    {
        private const string BaseURL = "https://ludumdare53api.azurewebsites.net";
        
        public static void ProcessGameResults(PlayerDto gameResults, Action<PlayerDto> successCallback)
        {
            const string url = BaseURL + "/processGameResults";
            Debug.Log(url);
            var json = JsonConvert.SerializeObject(gameResults);
            Debug.Log(json);
            RestClient.Post(url, json)
                .Then(response =>
                {
                    Debug.Log("Request successful");
                    var deserializedPlayer = JsonConvert.DeserializeObject<PlayerDto>(response.Text);
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
        
        public static void FetchTop10(Action<List<PlayerDto>> successCallback)
        {
            const string url = BaseURL + "/players/getTopTen";
            Debug.Log(url);
        
            RestClient.Get(url)
                .Then(response =>
                {
                    Debug.Log("Request successful");
                    var deserializedPlayers = JsonConvert.DeserializeObject<List<PlayerDto>>(response.Text);
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