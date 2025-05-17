using System;
using System.Collections.Generic;
using Godot;
using Godot.Collections;

[GlobalClass]
public partial class ItemProvider : Node {
    public Godot.Collections.Dictionary<int, Godot.Collections.Dictionary<EModuleType, int>> Weapons;
    public Godot.Collections.Dictionary<int, string> WeaponNames;
    public Godot.Collections.Dictionary<int, Module> Barrels;
    public Godot.Collections.Dictionary<int, Module> Magazines;
    public Godot.Collections.Dictionary<int, Module> Triggers;
    public Godot.Collections.Dictionary<int, Module> Gunsights;
    public Godot.Collections.Dictionary<int, Module> AmmoTypes;

    string BasePath = "res://json_data/";

    public override void _Ready()
    {
        base._Ready();
        Weapons = new Godot.Collections.Dictionary<int, Godot.Collections.Dictionary<EModuleType, int>>();
        FillWeapons();
        Barrels = new Godot.Collections.Dictionary<int, Module>();
        FillModules(EModuleType.BAREL);
        Magazines = new Godot.Collections.Dictionary<int, Module>();
        FillModules(EModuleType.MAGAZINE);
        Triggers = new Godot.Collections.Dictionary<int, Module>();
        FillModules(EModuleType.TRIGGER);
        Gunsights = new Godot.Collections.Dictionary<int, Module>();
        FillModules(EModuleType.GUNSIGHT);
        AmmoTypes = new Godot.Collections.Dictionary<int, Module>();
        FillModules(EModuleType.AMMO_TYPE);
    }

    void FillModules(EModuleType moduleType) {
        string subPath = GetPathByModuleType(moduleType);
        Dictionary parsedJson = Json.ParseString(FileAccess.GetFileAsString(BasePath + subPath)).AsGodotDictionary();
        
        foreach (KeyValuePair<Variant, Variant> IDValue in parsedJson) {
            Module moduleToAdd = new Module(IDValue.Value.AsGodotDictionary());
            GetDictionaryByType(moduleType).Add((int) IDValue.Key, moduleToAdd);
        }
    }

    void FillWeapons() {
        string subPath = "weapons.json";
        Dictionary parsedJson = Json.ParseString(FileAccess.GetFileAsString(BasePath + subPath)).AsGodotDictionary();
        
        foreach (KeyValuePair<Variant, Variant> IDValue in parsedJson) {
            Godot.Collections.Dictionary<EModuleType, int> modules = new Godot.Collections.Dictionary<EModuleType, int>();
            foreach (KeyValuePair<Variant, Variant> typeID in IDValue.Value.AsGodotDictionary()) {
                EModuleType type = GetModuleTypeFromString((string) typeID.Key);
                int id = (int) typeID.Value;
                modules.Add(type, id);
            }
            Weapons.Add((int) IDValue.Key, modules);
            WeaponNames.Add((int) IDValue.Key, IDValue.Value.AsGodotDictionary()["Name"].AsString());
        }
    }

    string GetPathByModuleType(EModuleType moduleType) {
        switch(moduleType) {
            case EModuleType.BAREL : return "barels.json";
            case EModuleType.MAGAZINE : return "magazines.json";
            case EModuleType.GUNSIGHT : return "gunsights.json";
            case EModuleType.TRIGGER : return "triggers.json";
            case EModuleType.AMMO_TYPE : return "ammo_types.json";
        }
        return "";
    }

    Godot.Collections.Dictionary<int, Module> GetDictionaryByType(EModuleType moduleType) {
        switch(moduleType) {
            case EModuleType.BAREL : return Barrels;
            case EModuleType.MAGAZINE : return Magazines;
            case EModuleType.GUNSIGHT : return Gunsights;
            case EModuleType.TRIGGER : return Triggers;
            case EModuleType.AMMO_TYPE : return AmmoTypes;
        }
        return null;
    }

    public EModuleType GetModuleTypeFromString(string moduleType) {
        switch (moduleType) {
            case "Barel_ID" : return EModuleType.BAREL;
            case "Magazine_ID" : return EModuleType.MAGAZINE;
            case "Trigger_ID" : return EModuleType.TRIGGER;
            case "Gunsight_ID" : return EModuleType.GUNSIGHT;
            case "AmmoType_ID" : return EModuleType.AMMO_TYPE;
        }
        return EModuleType.NONE;
    }
}