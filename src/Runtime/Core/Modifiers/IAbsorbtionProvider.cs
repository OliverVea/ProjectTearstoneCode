namespace MyRpg.Core.Modifiers
{
    public interface IAbsorptionProvider
    {
        /// <param name="damage">The damage which can be absorbed by the provider.</param>
        /// <returns>The remaining unabsorbed damage.</returns>
        float ServerAbsorbDamage(float damage);

        bool HasAbsorption();
    }
}