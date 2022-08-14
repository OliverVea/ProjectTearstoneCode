using System.Collections.Generic;
using MyRpg.Core.Models;

namespace MyRpg.Core.Modifiers
{
    public interface IPrimaryStatModifierProvider
    {
        IEnumerable<float> GetRelativeModifiers(PrimaryStat primaryStat);
        IEnumerable<float> GetAbsoluteModifiers(PrimaryStat primaryStat);
    }
}