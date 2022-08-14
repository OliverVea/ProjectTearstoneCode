using UnityEngine;

namespace MyRpg.Core.Components
{
    public interface IChargeComponent : IBase
    {
        public void ServerSetCharge(GameObject target, string effectId);
        public bool InCharge { get; }
    }
}