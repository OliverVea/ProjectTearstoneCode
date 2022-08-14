using System.Collections.Generic;
using UnityEngine;

namespace MyRpg.Core.Components
{
    public interface IThreatComponent : IBase
    {
        void ServerAddThreat(GameObject target, float threat);
        void ServerDropThreat(GameObject target);
        void ServerTaunt(GameObject owner);
        void ServerPull(GameObject target);
        List<GameObject> GetAllTargets();

    }
}