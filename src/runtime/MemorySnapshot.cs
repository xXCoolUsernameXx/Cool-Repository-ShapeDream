namespace MemoryInsights.runtime;

internal readonly struct MemorySnapshot
{
    public HeroSkillLocation Slot { get; }
    public string TypeName { get; }
    public string UnityName { get; }
    public int ConfigIndex { get; }
    public bool CanCast { get; }
    public bool Exists { get; }

    public MemorySnapshot(
        HeroSkillLocation slot,
        string typeName,
        string unityName,
        int configIndex,
        bool canCast)
    {
        Slot = slot;
        TypeName = typeName;
        UnityName = unityName;
        ConfigIndex = configIndex;
        CanCast = canCast;
        Exists = true;
    }

    private MemorySnapshot(HeroSkillLocation slot)
    {
        Slot = slot;
        TypeName = string.Empty;
        UnityName = string.Empty;
        ConfigIndex = -1;
        CanCast = false;
        Exists = false;
    }

    public static MemorySnapshot Empty(HeroSkillLocation slot) => new MemorySnapshot(slot);
}
