# UAssetEditor
An Unreal Engine UAsset API for Fortnite

## Features
- Mount I/O containers (.utoc) and read packages compressed with Oodle
- Deserialize unversioned uassets with the provided mappings
- Modify uasset data/ properties
- Serialize the modified asset back into a uasset file
- Switching engine version and mappings to allow porting assets to different Fortnite versions (WIP documentation)
- Automatic object property remapping (when switching Fortnite versions)

## How to use
```csharp

// Create UnrealFileSystem
var system = new UnrealFileSystem(@"S:\Fortnite\FortniteGame\Content\Paks", EGame.GAME_UE5_LATEST);
system.AesKeys.Add(new FGuid(), new FAesKey("0x03C8AAEDE702DB50231125AF91F24EF9171723274AC73DFBE06C95FF9AE911D6"));

// Start a stopwatch
var sw = Stopwatch.StartNew();

// Mount files
system.Initialize(loadInParallel: false);

// Stop the stopwatch
sw.Stop();

Console.WriteLine($"Mounted {system.MountedFilesCount} containers in {sw.ElapsedMilliseconds}ms");

// Load mappings
system.LoadMappings("++Fortnite+Release-42.30-CL-58557680_zs.usmap");

// Read the asset
if (!system.TryExtractAndRead("FortniteGame/Content/Athena/Items/Weapons/WID_Assault_Auto_Athena_C_Ore_T02.uasset", out var asset))
    throw new ApplicationException("Could not extract and read asset!");

// Get the property
var rowName = asset["WID_Assault_Auto_Athena_C_Ore_T02"]["WeaponStatHandle"]["RowName"].GetValue<FName>();
Console.WriteLine($"StatTable row name is {rowName}");

// Set the new value
rowName.Name = "Assault_Auto_Athena_C_Ore_T03";

// Write
var writer = new Writer();
asset.WriteAll(writer);

File.WriteAllBytes("WID_Assault_Auto_Athena_C_Ore_T02.uasset", writer.ToArray());

// Create a new ZenAsset with the asset we just serialized
var testAsset = new ZenAsset("WID_Assault_Auto_Athena_C_Ore_T02.uasset");

// Set the GlobalReader instance
var globalToc = system.GetGlobalReader();
testAsset.Initialize(globalToc!);

// Set mappings
testAsset.Mappings = system.Mappings;

// Test if it reads our asset properly
testAsset.ReadAll();
```

## TODOs
- Export Types (Blueprints, etc.)
- Paks and versioned uassets
- Desktop app
- Documentation (wip)

### Special thanks to
This project was heavily based on [CUE4Parse](https://github.com/FabianFG/CUE4Parse) with the goal of recreating Unreal Engine's asset reading and writing functionality as an easy to use API
