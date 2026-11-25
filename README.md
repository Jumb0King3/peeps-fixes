# Peeps Fixes

A small patch mod that fixes errors and bugs in other popular mods. Made for The Peeps server, but it works for anyone using the same mods.

**Each fix only switches on if the mod it fixes is installed**, so it's safe to use with any mod list and won't cause errors for mods you don't have.

### What it fixes

- **Legacy of the Phanerozoic: Tyrannosaurs**: fixes the egg error when cracking eggs.
- **Feverstone Wilds**: adds the missing "Center" attachment point to its creatures (stops the log errors) and adds the missing strike sounds.
- **Combat Overhaul + Overhaul lib legacy compat**: turns off the Armory crafting recipes that error when the Armory mod isn't installed, and turns off the vanilla club, which otherwise shows up as a broken item.
- **PetAI**: vegetables can be placed on the ground again.
- **Expanded Foods**: the large trough with eggshells and the small trough with peanuts now look right.

Install on both the server and clients. Found something else broken? [Open an issue](../../issues/new/choose).

## Download

ModDB (1-click install): https://mods.vintagestory.at/peepsfixes

## Report a problem

Please report bugs on the [Issues](../../issues) tab and include your game version, the mod version and your log. Security problems: see [SECURITY.md](SECURITY.md).

**Where to find your log**

The logs are in the Vintage Story data folder, in a folder called `Logs`:

| System | Data folder |
|---|---|
| Windows | `%appdata%\VintagestoryData` (paste that into the File Explorer address bar) |
| Linux | `~/.config/VintagestoryData` |
| Mac | `~/Library/Application Support/VintagestoryData` |

- **Playing on a server:** send `client-main.log` from your own game. If you run the server, also send `server-main.log` from the server's data folder.
- **Singleplayer:** `client-main.log` and `server-main.log` are both in the data folder above.
- Attach the file to the issue (drag it into the box), or paste just the lines with errors. Reproduce the problem first, then grab the log straight afterwards, so the error is near the end.
