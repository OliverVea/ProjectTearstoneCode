using System.Collections.Generic;
using MyRpg.Core.Models;

namespace MyRpg.Core.Modifiers
{
    public interface ISecondaryStatModifierProvider
    {
        IEnumerable<float> GetRelativeModifiers(SecondaryStat primaryStat);
        IEnumerable<float> GetAbsoluteModifiers(SecondaryStat primaryStat);
    }
}