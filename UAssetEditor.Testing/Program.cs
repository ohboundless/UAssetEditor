using System.Data;
using System.Diagnostics;
using System.IO.Enumeration;
using UAssetEditor;
using UAssetEditor.Unreal.Properties.Types;
using UAssetEditor.Binary;
using UAssetEditor.Compression;
using UAssetEditor.Encryption.Aes;
using UAssetEditor.Unreal.Assets;
using UAssetEditor.Unreal.Containers;
using UAssetEditor.Unreal.Misc;
using UAssetEditor.Unreal.Names;
using UAssetEditor.Unreal.Objects;
using UAssetEditor.Unreal.Objects.IO;
using UAssetEditor.Unreal.Readers.IoStore;
using UAssetEditor.Unreal.Versioning;
using UAssetEditor.Utils;
using UsmapDotNet;

Logger.StartLogger();

// Initialize Oodle
UnrealFileSystem.InitializeOodle("oo2core_9_win64.dll");

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