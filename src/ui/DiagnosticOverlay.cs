using MemoryInsights.runtime;
using UnityEngine;

namespace MemoryInsights.ui;

internal sealed class DiagnosticOverlay : MonoBehaviour
{
    private EquippedMemoryProvider _provider;
    private GUIStyle _boxStyle;
    private GUIStyle _titleStyle;
    private GUIStyle _lineStyle;

    public void Initialize(EquippedMemoryProvider provider)
    {
        _provider = provider;
    }

    private void OnGUI()
    {
        if (_provider == null)
        {
            return;
        }

        EnsureStyles();

        Hero hero = _provider.LocalHero;
        float width = 430f;
        float height = 170f;
        Rect rect = new Rect(18f, 18f, width, height);

        GUI.Box(rect, GUIContent.none, _boxStyle);
        GUI.Label(new Rect(rect.x + 12f, rect.y + 8f, width - 24f, 24f), "MEMORY INSIGHTS · DEBUG", _titleStyle);

        string heroText = hero == null
            ? "Hero: not available (enter a run)"
            : $"Hero: {hero.GetType().Name} | Unity name: {hero.name}";

        GUI.Label(new Rect(rect.x + 12f, rect.y + 34f, width - 24f, 22f), heroText, _lineStyle);

        DrawMemoryLine(rect.x + 12f, rect.y + 60f, _provider.Snapshot(HeroSkillLocation.Q));
        DrawMemoryLine(rect.x + 12f, rect.y + 84f, _provider.Snapshot(HeroSkillLocation.W));
        DrawMemoryLine(rect.x + 12f, rect.y + 108f, _provider.Snapshot(HeroSkillLocation.E));
        DrawMemoryLine(rect.x + 12f, rect.y + 132f, _provider.Snapshot(HeroSkillLocation.R));
    }

    private void DrawMemoryLine(float x, float y, MemorySnapshot snapshot)
    {
        string text;
        if (!snapshot.Exists)
        {
            text = $"{snapshot.Slot}: —";
        }
        else
        {
            string state = snapshot.CanCast ? "ready" : "not ready";
            text = $"{snapshot.Slot}: {snapshot.TypeName} | config {snapshot.ConfigIndex} | {state}";
        }

        GUI.Label(new Rect(x, y, 405f, 22f), text, _lineStyle);
    }

    private void EnsureStyles()
    {
        if (_boxStyle != null)
        {
            return;
        }

        _boxStyle = new GUIStyle(GUI.skin.box)
        {
            alignment = TextAnchor.UpperLeft
        };

        _titleStyle = new GUIStyle(GUI.skin.label)
        {
            fontSize = 14,
            fontStyle = FontStyle.Bold
        };

        _lineStyle = new GUIStyle(GUI.skin.label)
        {
            fontSize = 12
        };
    }
}
