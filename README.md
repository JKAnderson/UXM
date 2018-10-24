
# UXM 2.1.2
Enables file modding for DS2, SotFS, and DS3 by unpacking game archives and patching the executable to load loose files instead.  
Requires [.NET 4.7.2](https://www.microsoft.com/net/download/thank-you/net472) - Windows 10 users should already have this.  

# Usage
When first starting the app, the default Steam location of DS3 will be selected. If the game you want is installed somewhere else, use the Browse button to correct it.

The Unpack button will extract all files from the game archives; any files that are already present will be skipped, so if you need to unmodify any file you can simply delete it and unpack again without having to sit through the entire process.

The Patch button will modify the executable to use those extracted files instead of looking in the archives. Patching the executable without also unpacking the archives will crash the game, so make sure you do both.

Finally, the Restore button will restore the original executable and delete the extracted files, and the Abort button will cancel any operation in progress.

# Bannability
UXM only edits data within the executable, not code, so anticheat should have no effect on it. File mods in general have never been grounds for a ban in any of the Souls games, but mods that alter your save may not be safe, so please consult your mod author's advice and play offline if using anything dubious.

# Credits
[BinderTool](https://github.com/Atvaark/BinderTool) by Atvaark

[Costura.Fody](https://github.com/Fody/Costura) by Simon Cropp, Cameron MacFarland

[Octokit](https://github.com/octokit/octokit.net) by GitHub

[Semver](https://github.com/maxhauser/semver) by Max Hauser

# Changelog
### 2.1.2
* Fix the update link in the app not actually doing anything

### 2.1.1
* Fix DLC2 files still being loaded from archive (just click Patch again to fix it)

### 2.1
* Fix keybinding menu being broken because of a false positive
* Identify all but 1 file in DS3 (and support release version better)

### 2.0
* U3M has been mostly rewritten and is now UXM
* DS2 and SotFS are supported
* Performance improvements (20-30% faster unpacking)
* Patching and restoring now also display progress and are abortable
* Fixed some misidentified files in DS3 and identified 1 new one