using System;
using MyRpg.Core.Models;

namespace MyRpg.Core.Components
{
    public interface IProgressComponent : IBase
    {
        public bool IsCasting();
        
        void ServerStartProgress(float duration, Action onCompletion, string text, bool isInterruptible);
        bool ServerInterrupt();

        public Progress GetProgress();
        void ServerCancel();
    }
}