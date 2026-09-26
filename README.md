
# GK2 Queue Count

A small BepInEx mod for Graveyard Keeper 2 that displays the remaining
number of crafting operations directly on the world crafting indicator.

The mod adds a queue counter to the active crafting icon while keeping
Graveyard Keeper 2's original output count visible.

Example:

- `×7` = 7 crafts remaining
- `28` = 28 output items remaining

## Features

- Displays the remaining number of crafting operations
- Keeps the vanilla output count visible
- Integrates with the existing world crafting indicator
- Designed to stay visually close to the vanilla UI

## Requirements

- Graveyard Keeper 2
- BepInEx 5.4.23.5

## Installation

1. Install BepInEx 5.4.23.5.
2. Extract the archive into your Graveyard Keeper 2 installation directory.

The DLL should end up here:

`BepInEx/plugins/GK2QueueCount/GK2QueueCount.dll`

## Development

The project references assemblies from the local Graveyard Keeper 2
installation. Game assemblies and BepInEx binaries are not included in
the repository.

## License

MIT
