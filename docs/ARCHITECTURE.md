# Memory Insights Architecture

## Goals

Memory Insights is split into layers so we can keep read-only QOL behavior separate from any future developer/testing tools that alter gameplay.

### Public/QOL mod

The main mod should:

- read the local hero and equipped Memories;
- inspect live Memory/Essence state;
- calculate and display information;
- avoid mutating combat state;
- avoid automatically casting skills, spawning entities, modifying progression, or bypassing multiplayer safeguards.

### Future DevTools companion

Gameplay-mutating experiments should live in a separate mod/assembly, tentatively `MemoryInsights.DevTools`, and be intended for single-player or consenting private-lobby testing. Examples include spawning a training target, invoking a Memory through the normal command path, and recording observed damage for calculator validation.

## Current v0.2 layers

```text
src/
├── MemoryInsights.cs
├── runtime/
│   ├── EquippedMemoryProvider.cs
│   └── MemorySnapshot.cs
└── ui/
    └── DiagnosticOverlay.cs
```

### `MemoryInsightsMod`

Owns lifecycle only. It creates service/controller objects and Unity components, then cleans them up on unload.

### `runtime/`

Contains game-facing read-only adapters. The first one resolves `DewPlayer.local.hero` and the equipped Q/W/E/R `SkillTrigger` objects via `hero.Skill.TryGetSkill(...)`.

### `ui/`

Contains presentation code. v0.2 intentionally uses a small IMGUI diagnostic overlay because it gives us immediate visibility into runtime state before we patch real Memory tooltips.

## Planned layers

```text
src/
├── calculations/
│   ├── ScalingEvaluator.cs
│   ├── MemoryCalculator.cs
│   └── CalculationResult.cs
├── data/
│   └── MemoryDatabase.cs
├── patch/
│   └── MemoryTooltipPatch.cs
├── runtime/
│   ├── EquippedMemoryProvider.cs
│   ├── LocalHeroProvider.cs
│   └── MemoryContext.cs
└── ui/
    ├── DiagnosticOverlay.cs
    └── TooltipInsightsView.cs
```

## Design rule

Harmony patches, when we need them, should be thin interception points. They should forward context to calculation/UI code instead of containing business logic.

## Validation strategy

For simple Memories we eventually want four-way agreement:

1. authored/static data;
2. game-native live scaling evaluation;
3. Memory Insights' displayed result;
4. observed damage against a controlled test target.

If those disagree, the discrepancy becomes a research task rather than something we hide with guessed formulas.
