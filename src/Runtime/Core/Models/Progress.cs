using System;

namespace MyRpg.Core.Models
{
    [Serializable]
    public class Progress
    {
        public float progress;
        public float finishTime;
        public string text;
        public bool isInterruptible;

        public bool IsFinished => progress >= finishTime;
    }
}