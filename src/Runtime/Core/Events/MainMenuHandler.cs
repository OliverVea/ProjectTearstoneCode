using System;
using System.Linq;
using MyRpg.Core.Models;
using Steamworks;
using UnityEngine;

namespace MyRpg.Core.Events
{
    public static class MainMenuHandler
    {
        private static Action<GameObject, Character> _characterSelected = (a, b) => Debug.Log($"[Event] AllCharactersSelected({a}, {b})");
        private static Action<CSteamID> _startJoinGame = id => Debug.Log($"[Event] StartJoinGame({id})");
        private static Action _startHostGame = () => Debug.Log($"[Event] StartHostGame()");
        private static Action<string> _setLobbyType = s => Debug.Log($"[Event] SetLobbyType({s})");

        private static Action<LobbyInformation[]> _lobbyList = lobbyList =>
        {
            var lobbies = string.Join(", ", lobbyList.Select(x => x.lobbyName));
            Debug.Log($"[Event] LobbyList({lobbies})");
        };

        public static void RegisterOnCharacterSelected(Action<GameObject, Character> callback)
            => _characterSelected += callback;
        
        public static void UnregisterOnCharacterSelected(Action<GameObject, Character> callback)
            => _characterSelected -= callback;
            
        public static void InvokeCharacterSelected(GameObject source, Character character)
            => _characterSelected?.Invoke(source, character);
        
        public static void RegisterOnStartJoinGame(Action<CSteamID> callback)
            => _startJoinGame += callback;

        public static void UnregisterOnStartJoinGame(Action<CSteamID> callback)
            => _startJoinGame -= callback;

        public static void InvokeStartJoinGame(CSteamID lobbySteamId)
            => _startJoinGame?.Invoke(lobbySteamId);
        
        public static void RegisterOnStartHostGame(Action callback)
            => _startHostGame += callback;

        public static void UnregisterOnStartHostGame(Action callback)
            => _startHostGame -= callback;

        public static void InvokeStartHostGame()
            => _startHostGame?.Invoke();
        
        public static void RegisterOnSetLobbyType(Action<string> callback)
            => _setLobbyType += callback;

        public static void UnregisterOnSetLobbyType(Action<string> callback)
            => _setLobbyType -= callback;

        public static void InvokeSetLobbyType(string s)
            => _setLobbyType?.Invoke(s);
        
        public static void RegisterOnLobbyList(Action<LobbyInformation[]> callback)
            => _lobbyList += callback;

        public static void UnregisterOnLobbyList(Action<LobbyInformation[]> callback)
            => _lobbyList -= callback;

        public static void InvokeLobbyList(LobbyInformation[] lobbies)
            => _lobbyList?.Invoke(lobbies);
    }
}