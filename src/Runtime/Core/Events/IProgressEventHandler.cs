using System;
using MyRpg.Core.Models;

namespace MyRpg.Core.Events
{
    public interface IProgressEventHandler
    {
        void RegisterOnProgressStarted(Action<Progress> callback);
        void UnregisterOnProgressStarted(Action<Progress> callback);
        
        void RegisterOnProgress(Action<Progress> callback);
        void UnregisterOnProgress(Action<Progress> callback);
        
        void RegisterOnProgressCompleted(Action<Progress> callback);
        void UnregisterOnProgressCompleted(Action<Progress> callback);
        
        void RegisterOnProgressInterrupted(Action<Progress> callback);
        void UnregisterOnProgressInterrupted(Action<Progress> callback);
    }
}