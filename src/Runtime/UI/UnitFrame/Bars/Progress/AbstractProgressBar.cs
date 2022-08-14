using MyRpg.Core.Components;
using MyRpg.Core.Events;
using UnityEngine;

namespace MyRpg.UI.UnitFrame.Bars.Progress
{
    public class AbstractProgressBar : Bar
    {
        private IProgressEventHandler _progressEventHandler;
        private IProgressComponent _progressComponent;

        protected override void SetTarget(GameObject target)
        {
            ClearTarget();

            if (target == null) return;
            
            _progressEventHandler = target.GetComponent<IProgressEventHandler>();
            _progressComponent = target.GetComponent<IProgressComponent>();

            Register();

            var isCasting = _progressComponent?.IsCasting() ?? false;
            Show(isCasting);
        }

        protected override void ClearTarget()
        {
            Show(false);
            Unregister();
            _progressEventHandler = null;
            _progressComponent = null;
        }

        private void OnDestroy()
        {
            ClearTarget();
        }


        private void Register()
        {
            if (_progressEventHandler == null) return;
            
            _progressEventHandler.RegisterOnProgress(OnProgress);
            _progressEventHandler.RegisterOnProgressStarted(OnProgressStarted);
            _progressEventHandler.RegisterOnProgressCompleted(OnProgressEnded);
            _progressEventHandler.RegisterOnProgressInterrupted(OnProgressEnded);
        }

        private void Unregister()
        {
            if (_progressEventHandler == null) return;
            
            _progressEventHandler.UnregisterOnProgress(OnProgress);
            _progressEventHandler.UnregisterOnProgressStarted(OnProgressStarted);
            _progressEventHandler.UnregisterOnProgressCompleted(OnProgressEnded);
            _progressEventHandler.UnregisterOnProgressInterrupted(OnProgressEnded);
        }

        private void OnProgressEnded(Core.Models.Progress progress)
        {
            Show(false);
        }

        private void OnProgressStarted(Core.Models.Progress progress)
        {
            Show(true);
            UpdateValue(progress.progress, progress.finishTime);
        }

        private void OnProgress(Core.Models.Progress progress)
        {
            Show(true);
            UpdateValue(progress.progress, progress.finishTime);
        }
    }
}