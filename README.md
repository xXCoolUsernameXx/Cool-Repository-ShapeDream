# Memory Insights for Shape of Dreams

Memory Insights is an experimental client-side quality-of-life mod for **Shape of Dreams**. Its goal is to make Memories and Essences easier to understand by exposing live values the game already knows: equipped Memories, current character context, upgrade previews, compatibility notes, and eventually calculated damage/effect breakdowns.

The public mod is intended to stay read-only with respect to combat state. Separate developer tooling may be added later for single-player/private-lobby testing such as spawning training targets or invoking skills through the game's normal command path.

## Current milestone

The repository is being rebuilt around a small, testable v0.2 foundation:

- load directly from the game's `Mods` folder;
- resolve `DewPlayer.local.hero`;
- resolve equipped Q/W/E/R Memories as `SkillTrigger` objects;
- show a lightweight diagnostic overlay;
- keep research notes on verified game APIs before adding tooltip calculation logic.

## Install / build

Clone or extract this repository directly under the game Mods folder, for example:

```text
D:\Games\SteamStuff\steamapps\common\Shape of Dreams\Mods\MemoryInsights
```

Then run:

```text
build.bat
```

The project assumes it is two directories below the game root, matching the official Shape of Dreams mod template (`GameDir = ..\..`).

After a successful build, enable **Memory Insights** in the in-game Mod Manager.

## Research references

We are using these as reference material while keeping our implementation independent:

- Official mod documentation: https://github.com/LizardSmoothie/ShapeOfDreamsModDocs
- 0x2E757 mod collection: https://github.com/0x2E757/Shape-of-Dreams-mods
- chengchulian DewMod collection: https://github.com/chengchulian/DewMod
- Plantim community database/wiki tooling: https://github.com/Plantim/shape-of-dreams-wiki

See `docs/RESEARCH.md` for concrete findings and `docs/ARCHITECTURE.md` for the intended project boundaries.

## Status

Early development. Expect breaking changes while the runtime object model and tooltip pipeline are being mapped.
