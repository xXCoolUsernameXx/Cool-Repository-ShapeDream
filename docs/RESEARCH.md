# Shape of Dreams Modding Research Notes

This file records concrete API/runtime findings that have been verified against public source or the official template. It is intentionally a notebook, not a promise that every API will remain stable across game updates.

## Verified: mod project layout

The official Shape of Dreams template targets `netstandard2.1`, resolves the game root with `GameDir = ..\..` when the project lives directly under `Shape of Dreams\Mods\<ModName>`, and loads the built DLL through `about/metadata.json`.

Reference:
- https://github.com/LizardSmoothie/ShapeOfDreamsModDocs

## Verified: local hero and equipped Memories

Open-source mods use:

```csharp
Hero hero = DewPlayer.local?.hero;
hero.Skill.TryGetSkill(HeroSkillLocation.Q, out SkillTrigger skill);
```

Known `HeroSkillLocation` values used by public mods include Q, W, E, and R.

This is the basis of `EquippedMemoryProvider`.

References:
- https://github.com/chengchulian/DewMod/blob/master/DewGoldenBurstAutoTarget/src/util/GoldenBurstSkillProvider.cs
- https://github.com/chengchulian/DewMod/blob/master/DewGemSlotCount/src/patch/HeroSkill_Patch.cs

## Verified: world Actor discovery

`DewRoomGuidance` reads the local hero, obtains `NetworkedManagerBase<ActorManager>.softInstance`, iterates `actorManager.allActors`, and recognizes runtime objects including:

- `Shrine`
- `Gem`
- `SkillTrigger`
- `Artifact`
- `PropEntity`
- `Monster`

This demonstrates a read-only object-discovery pattern that does not need Harmony.

Reference:
- https://github.com/chengchulian/DewMod/blob/master/DewRoomGuidance/src/ui/RoomGuidanceOverlay.cs

## Verified: game-side entity spawning

`DewBootcamp` requires server authority, resolves a resource, then uses `Dew.SpawnEntity(...)`.

Observed pattern:

```csharp
Dew.SpawnEntity(
    DewResources.GetByType<Mon_RedGiant>(),
    spawnPosition,
    Quaternion.identity,
    serverActor,
    ownerPlayer,
    ambientLevel);
```

Before spawning it checks `GameManager.isServer` and obtains the current `ZoneManager`, `Room`, server actor, and ambient level.

This is a candidate foundation for a future training-dummy/dev-tools companion, not the public read-only mod.

Reference:
- https://github.com/chengchulian/DewMod/blob/master/DewBootcamp/src/DewBootcamp.cs

## Verified: normal skill command path

`DewGoldenBurstAutoTarget` retrieves a ready `SkillTrigger`, builds a `CastInfo`, and invokes:

```csharp
hero.Control.CmdCast(
    skill,
    skill.currentConfigIndex,
    castInfo,
    allowMoveToCast: false,
    skipRangeCheck: false);
```

Depending on the skill config it may then call:

```csharp
hero.Control.CmdAttack(null, doChase: false);
```

The cast helper handles `CastMethodType.Point`, `Target`, `Cone`, `Arrow`, `None`, and falls back to `skill.GetPredictedCastInfoToTarget(...)`.

This is useful for future automated validation/testing because it goes through the game's command path rather than simulating keyboard input.

References:
- https://github.com/chengchulian/DewMod/blob/master/DewGoldenBurstAutoTarget/src/util/GoldenBurstCaster.cs
- https://github.com/chengchulian/DewMod/blob/master/DewGoldenBurstAutoTarget/src/controller/GoldenBurstAutoTargetController.cs

## Verified: target discovery pattern

`DewGoldenBurstAutoTarget` uses `DewPhysics.OverlapCircleAllEntities(...)`, the skill config's `effectiveRange`, `config.CheckRange(...)`, and live status checks to choose a valid target.

Reference:
- https://github.com/chengchulian/DewMod/blob/master/DewGoldenBurstAutoTarget/src/util/GoldenBurstTargetSelector.cs

## Verified: native ModConfig usage

Open-source mods declare configuration classes derived from `ModConfig`. Ordinary public fields such as booleans and `KeyCode` values are surfaced by the game's configuration UI, with attributes such as `LabelText`, `Description`, and `Header` used to annotate fields.

References:
- https://github.com/chengchulian/DewMod/blob/master/DewBootcamp/src/config/PluginConfig.cs
- https://github.com/chengchulian/DewMod/blob/master/DewRoomGuidance/src/config/PluginConfig.cs

## Verified: public mod metadata pattern

Community mods commonly use an `about/metadata.json` with an id, name, author, version, supported versions, and an `assemblies` array pointing to the built DLL.

Reference:
- https://github.com/0x2E757/Shape-of-Dreams-mods/blob/main/mods/AreMyGemsCompatible/about/metadata.json

## Important boundary

Research findings from gameplay-altering mods are useful because they reveal game architecture. The main Memory Insights mod should still remain read-only. Gameplay mutation should be isolated in a separate future developer/testing mod so the public QOL code has a clear boundary.

## Next research questions

- Which tooltip class/method receives an equipped or hovered `SkillTrigger`?
- How does the game expose authored tooltip/scaling variables at runtime?
- Which API gives the most faithful current evaluated value for a `ScalingValue`?
- How should next-level evaluation be performed without omitting Essence/runtime modifiers?
- Where are final outgoing-damage processors applied after raw Memory scaling?
- Which event/data type is best for recording observed damage on a training target?
