
--| U3M 1.0
--| By TKGP
--| https://www.nexusmods.com/darksouls3/mods/286
--| https://github.com/JKAnderson/U3M

A tool for Dark Souls 3 to enable file modding by unpacking .bdt archives and enabling loose file loading.
Requires .NET 4.7.2: https://www.microsoft.com/net/download/thank-you/net472
Windows 10 users should already have this.


--| Usage

When first starting the app, check the default game directory at the top and use the Browse button to correct it if necessary.

The Unpack button will extract all files from the Data and DLC archives; if you want to skip some for whatever reason you can uncheck them, but you will need to do them all if you intend to play with loose files. If you don't own the DLC, those archives will be skipped automatically.

The Patch button will modify DarkSoulsIII.exe to use those extracted files instead of looking in the archives. Patching the executable without also unpacking the archives will crash the game, so make sure you do both.

Finally, the Restore button will restore the original executable and delete the extracted files, and the Abort button will cancel an unpack in progress.


--| Bannability

U3M only edits data within the executable, not code, so anticheat should have no effect on it. File mods in general have never been grounds for a ban in any of the Souls games, but mods that alter your save may not be safe, so please consult your mod author's advice and play offline if using anything dubious.


--| Credits

BinderTool by Atvaark
https://github.com/Atvaark/BinderTool

Costura.Fody by Simon Cropp, Cameron MacFarland
https://github.com/Fody/Costura

Octokit by GitHub
https://github.com/octokit/octokit.net

Semver by Max Hauser
https://github.com/maxhauser/semver
