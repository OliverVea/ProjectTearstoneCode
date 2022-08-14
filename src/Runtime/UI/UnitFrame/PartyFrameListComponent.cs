using System.Collections.Generic;
using System.Linq;
using MyRpg.Core.Events;
using MyRpg.Core.Events.EventData;
using MyRpg.Core.Helpers;
using MyRpg.Core.UI;
using TMPro;
using UnityEngine;

namespace MyRpg.UI.UnitFrame
{
    public class PartyFrameListComponent : MonoBehaviour
    {
        private string OnPartyChangedId => nameof(PartyFrameListComponent) + ToString() + nameof(OnPartyChanged);

        [SerializeField] private GameObject partyFramePrefab;
        private List<GameObject> _partyFrameObjects = new List<GameObject>();
        
        private void OnEnable()
        {
            GenericEventHandler.Register<PartyChangedEvent>(OnPartyChanged, OnPartyChangedId);
        }

        private void OnDisable()
        {
            GenericEventHandler.Unegister<PartyChangedEvent>(OnPartyChangedId);
        }

        private void OnPartyChanged(EventContent<PartyChangedEvent> eventContent)
        {
            foreach (var partyFrameObject in _partyFrameObjects) Destroy(partyFrameObject);
            _partyFrameObjects = new List<GameObject>();

            var partyMembers = eventContent.Data.PartyMembers.Where(x => !LocalPlayerHelper.IsLocalPlayer(x)).ToArray();
            
            for (var i = 0; i < partyMembers.Length; i++)
            {
                var player = partyMembers[i];
                var partyFrameObject = Instantiate(partyFramePrefab, transform);
                partyFrameObject.GetComponentInChildren<TMP_Text>().SetText("F" + (i + 1));
                foreach (var componentToInitialize in partyFrameObject.GetComponentsInChildren<IFixedTargetComponent>())
                    componentToInitialize.Initialize(player);
                _partyFrameObjects.Add(partyFrameObject);
            }
        }
    }
}