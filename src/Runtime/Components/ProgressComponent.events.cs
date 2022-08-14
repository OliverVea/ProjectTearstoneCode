using System;
using Mirror;
using MyRpg.Core.Events;
using MyRpg.Core.Models;

namespace MyRpg.Components
{
    public partial class ProgressComponent : IProgressEventHandler
    {
        private event Action<Progress> ProgressStarted;
        private event Action<Progress> Progress;
        private event Action<Progress> ProgressCompleted;
        private event Action<Progress> ProgressInterrupted;
        
        public void RegisterOnProgressStarted(Action<Progress> callback)
            => ProgressStarted += callback;
        public void UnregisterOnProgressStarted(Action<Progress> callback)
            => ProgressStarted -= callback;
        
        public void RegisterOnProgress(Action<Progress> callback)
            => Progress += callback;
        public void UnregisterOnProgress(Action<Progress> callback)
            => Progress -= callback;
        
        public void RegisterOnProgressCompleted(Action<Progress> callback)
            => ProgressCompleted += callback;
        public void UnregisterOnProgressCompleted(Action<Progress> callback)
            => ProgressCompleted -= callback;
        
        public void RegisterOnProgressInterrupted(Action<Progress> callback)
            => ProgressInterrupted += callback;
        public void UnregisterOnProgressInterrupted(Action<Progress> callback)
            => ProgressInterrupted -= callback;

        [Server]
        private void ServerOnProgressStarted(Progress progress)
        {
            OnProgressStarted(progress);
        }

        [ClientRpc]
        private void BroadcastOnProgressStarted(Progress progress)
        {
            if (isServer) return;
            OnProgressStarted(progress);
        }

        private void OnProgressStarted(Progress progress)
        {
            var handler = ProgressStarted;
            handler?.Invoke(progress);
        }

        [Server]
        private void ServerOnProgress(Progress progress)
        {
            OnProgress(progress);
        }

        [ClientRpc]
        private void BroadcastOnProgress(Progress progress)
        {
            if (isServer) return;
            OnProgress(progress);
        }

        private void OnProgress(Progress progress)
        {
            var handler = Progress;
            handler?.Invoke(progress);
        }

        [Server]
        private void ServerOnProgressCompleted(Progress progress)
        {
            OnProgressCompleted(progress);
        }

        [ClientRpc]
        private void BroadcastOnProgressCompleted(Progress progress)
        {
            if (isServer) return;
            OnProgressCompleted(progress);
        }

        private void OnProgressCompleted(Progress progress)
        {
            var handler = ProgressCompleted;
            handler?.Invoke(progress);
        }

        [Server]
        private void ServerOnProgressInterrupted(Progress progress)
        {
            OnProgressInterrupted(progress);
        }

        [ClientRpc]
        private void BroadcastOnProgressInterrupted(Progress progress)
        {
            if (isServer) return;
            OnProgressInterrupted(progress);
        }

        private void OnProgressInterrupted(Progress progress)
        {
            var handler = ProgressInterrupted;
            handler?.Invoke(progress);
        }
    }
}