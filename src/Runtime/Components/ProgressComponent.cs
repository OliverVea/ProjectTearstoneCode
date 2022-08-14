using System;
using System.Linq;
using Mirror;
using MyRpg.Core.Components;
using MyRpg.Core.Events;
using MyRpg.Core.Models;
using MyRpg.Core.Modifiers;
using UnityEngine;

namespace MyRpg.Components
{
    public partial class ProgressComponent : NetworkBehaviour, IProgressComponent
    {
        private Progress _progress;
        private Action _onCompletion;
        
        private IDeathEventHandler _deathEventHandler;
        
        private ICharacterStatusProvider[] _characterStatusProviders;
        
        public bool IsCasting() => _progress != null;

        private void Awake()
        {
            _deathEventHandler = GetRequiredComponent<IDeathEventHandler>();
            _characterStatusProviders = GetComponents<ICharacterStatusProvider>();
        }

        private void OnEnable()
        {
            MovementEventHandler.RegisterOnMovement(OnMovement);
            _deathEventHandler.RegisterOnDeath(OnDeath);
        }

        private void OnDisable()
        {
            MovementEventHandler.UnregisterOnMovement(OnMovement);
            _deathEventHandler.UnregisterOnDeath(OnDeath);
        }

        [ServerCallback]
        private void Update()
        {
            if (IsStunned()) ServerCancel();
            if (!IsCasting()) return;

            _progress.progress += Time.deltaTime;

            if (_progress.IsFinished) FinishProgress();
            else
            {
                ServerOnProgress(_progress);
                BroadcastOnProgress(_progress);
            }
        }

        private void FinishProgress()
        {
            _onCompletion();
            _progress = null;
            ServerOnProgressCompleted(_progress);
            BroadcastOnProgressCompleted(_progress);
        }

        private bool IsStunned()
        {
            return _characterStatusProviders.Any(x => !x.GetCharacterStatus().CanCast);
        }


        [Server]
        public void ServerStartProgress(float duration, Action onCompletion, string text, bool isInterruptible)
        {
            if (IsCasting()) return;

            _progress = new Progress
            {
                finishTime = duration,
                text = text,
                isInterruptible = isInterruptible
            };

            _onCompletion = onCompletion;
            
            ServerOnProgressStarted(_progress);
            BroadcastOnProgressStarted(_progress);
        }

        [Server]
        public bool ServerInterrupt()
        {
            if (!IsCasting()) return false;
            if (!_progress.isInterruptible) return false;
            ServerCancel();
            return true;
        }

        public Progress GetProgress()
        {
            return _progress;
        }

        [Server]
        public void ServerCancel()
        {
            if (!IsCasting()) return;
            var interruptedProgress = _progress;
            _progress = null;
            
            ServerOnProgressInterrupted(interruptedProgress);
            BroadcastOnProgressInterrupted(interruptedProgress);
        }

        private void OnMovement(GameObject source, Vector2 movement)
        {
            if (source != gameObject || !isServer || movement.magnitude == 0) return;
            ServerCancel();
        }

        private void OnDeath()
        {
            if (!isServer) return;
            ServerCancel();
        }

        public void ServerInitialize(Character character)
        {
            
        }
    }
}