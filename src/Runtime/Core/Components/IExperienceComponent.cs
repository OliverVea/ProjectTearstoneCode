namespace MyRpg.Core.Components
{
    public interface IExperienceComponent
    {
        int GetCurrentLevel();
        float AwardFlatExperience(float experiencePoints);
        float AwardKillExperience(float experiencePoints, int monsterLevel);
    }
}