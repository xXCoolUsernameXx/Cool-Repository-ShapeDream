using UnityEngine;
using MemoryInsights.runtime;
using MemoryInsights.ui;

namespace MemoryInsights;

public sealed class MemoryInsightsMod : ModBehaviour
{
    public static MemoryInsightsMod Instance { get; private set; }

    private DiagnosticOverlay _overlay;
    private EquippedMemoryProvider _memoryProvider;

    private void Awake()
    {
        Instance = this;

        _memoryProvider = new EquippedMemoryProvider();
        _overlay = gameObject.AddComponent<DiagnosticOverlay>();
        _overlay.Initialize(_memoryProvider);

        Debug.Log($"[MemoryInsights] loaded: {mod.metadata.id}");
    }

    private void OnDestroy()
    {
        if (_overlay != null)
        {
            Destroy(_overlay);
            _overlay = null;
        }

        _memoryProvider = null;

        if (Instance == this)
        {
            Instance = null;
        }

        Debug.Log("[MemoryInsights] unloaded");
    }
}
