using System.Linq;
using System.Threading.Tasks;
using Mirror;
using MyRpg.Characters;
using MyRpg.Core;
using MyRpg.Core.Constants;
using MyRpg.Core.Events;
using MyRpg.Core.Events.EventData;
using MyRpg.Core.Models;
using MyRpg.Core.Network;
using MyRpg.Network.Messages;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MyRpg.Network
{
    public class MyNetworkManager : NetworkManager, IMyNetworkManager
    {
        [Header("Custom Attributes")] 
        [Scene] [SerializeField] private string initialGameScene;

        private ISelectedCharacterComponent _characterComponent;

        public override void Awake()
        {
            base.Awake();

            _characterComponent = GetComponent<ISelectedCharacterComponent>();
            
            //GenericEventHandler.Register<PlayerJoinedEvent>(data => Debug.Log(data.ToString()), ToString());
        }

        public override void OnStartServer()
        {
            base.OnStartServer();

            NetworkServer.RegisterHandler<CreateCharacterMessage>(OnCreateCharacter);
        }

        public bool StartGame()
        {
            if (!_characterComponent.IsCharacterSelected())
            {
                Debug.LogError("Cannot start game with invalid character.");
                return false;
            }
            
            StartHost();
            
            while (!NetworkServer.active || !NetworkClient.active) Task.Delay(50).Wait();

            clientLoadedScene = false;
            ServerChangeScene(initialGameScene);

            return true;
        }

        public bool JoinGame(string hostAddress)
        {
            if (!_characterComponent.IsCharacterSelected())
            {
                Debug.LogError("Cannot join game with invalid character.");
                return false;
            }

            networkAddress = hostAddress;
            StartClient();

            return true;
        }

        public void ResetLevel()
        {
            clientLoadedScene = false;
            ServerChangeScene(SceneManager.GetActiveScene().name);
            FinishLoadScene();
        }

        public override void OnClientConnect()
        {
            base.OnClientConnect();

            //CreateCharacter();
        }

        public override void OnClientSceneChanged()
        {
            base.OnClientSceneChanged();
            
            CreateCharacter();
        }

        private void CreateCharacter()
        {
            var characterId = PlayerPrefs.GetString(ConstantValues.PrefKeys.SelectedCharacter);
            var characters = CharacterLoader.LoadCharacters();
            var character = characters.FirstOrDefault(x => x.characterId == characterId);

            if (!character.IsValid)
                return;
            
            var createCharacterMessage = new CreateCharacterMessage { Character = character };

            NetworkClient.Send(createCharacterMessage);
        }

        private void OnCreateCharacter(NetworkConnectionToClient conn, CreateCharacterMessage message)
        {
            var prefab = LookupComponent.GetCharacterPrefab(message.Character.characterClass);
            
            var startPos = GetStartPosition();
            
            var playerObject = startPos != null
                ? Instantiate(prefab, startPos.position, startPos.rotation)
                : Instantiate(prefab);

            var components = playerObject.GetComponentsInChildren<IBase>();
            foreach (var component in components) component.ServerInitialize(message.Character);
            
            NetworkServer.AddPlayerForConnection(conn, playerObject);

            GenericEventHandler.Invoke(null, new PlayerJoinedEvent { CharacterId = message.Character.characterId });
        }
    }
}