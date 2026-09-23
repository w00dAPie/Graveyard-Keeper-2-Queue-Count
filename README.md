# GK2 Queue Count

A small BepInEx mod for Graveyard Keeper 2 that displays the remaining
number of crafting operations directly on the world crafting indicator.

## Current status

Work in progress.

The mod currently adds a queue counter to the active crafting icon while
keeping Graveyard Keeper 2's original output count visible.

Example:

- `×7` = 7 crafts remaining
- `28` = 28 output items remaining

## Requirements

- Graveyard Keeper 2
- BepInEx 5

## Installation

Copy `GK2QueueCount.dll` to:

`BepInEx/plugins/GK2QueueCount/`

## Development

The project references assemblies from the local Graveyard Keeper 2
installation. Game assemblies and BepInEx binaries are not included in
this repository.