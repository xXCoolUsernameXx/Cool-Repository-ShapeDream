namespace MemoryInsights.runtime;

internal sealed class EquippedMemoryProvider
{
    public Hero LocalHero => DewPlayer.local?.hero;

    public bool TryGetMemory(HeroSkillLocation slot, out SkillTrigger skill)
    {
        skill = null;

        Hero hero = LocalHero;
        if (hero == null || hero.IsNullInactiveDeadOrKnockedOut())
        {
            return false;
        }

        return hero.Skill.TryGetSkill(slot, out skill) && skill != null;
    }

    public MemorySnapshot Snapshot(HeroSkillLocation slot)
    {
        if (!TryGetMemory(slot, out SkillTrigger skill))
        {
            return MemorySnapshot.Empty(slot);
        }

        return new MemorySnapshot(
            slot,
            skill.GetType().Name,
            skill.name,
            skill.currentConfigIndex,
            skill.CanBeCast());
    }
}
