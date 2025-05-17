using Godot;
using Godot.Collections;

[Tool]
public partial class Module : Resource {
    [Export] int ID_;
    public int ID => ID_;
    [Export] EModuleType ModuleType_;
    public EModuleType ModuleType => ModuleType_;
    [Export] float ReloadTimeMod_;
    public float ReloadTimeMod => ReloadTimeMod_;
    [Export] float FireRate_;
    public float FireRate => FireRate_;
    [Export] float BulletSpeedMod_;
    public float BulletSpeedMod => BulletSpeedMod_;
    [Export] float BulletLiveTimeMod_;
    public float BulletLiveTimeMod => BulletLiveTimeMod_;
    [Export] int MagazineCapasityMod_;
    public int MagazineCapasityMod => MagazineCapasityMod_;
    [Export] int AmmoCapasityMod_;
    public int AmmoCapasityMod => AmmoCapasityMod_;
    [Export] Array<Vector3> FirePattern_;
    public Array<Vector3> FirePattern => FirePattern_;
    [Export] float DamageMod_;
    public float DamageMod => DamageMod_;

    public Module() {
        FirePattern_ = new Array<Vector3>();
    }

    public Module(Dictionary data) {
        FirePattern_ = new Array<Vector3>();
        GetFromJson(data);
    }

    Module GetFromJson(Dictionary data) {
        ReloadTimeMod_ = (float) data["ReloadTimeMod"];
        FireRate_ = (float) data["FireRate"];
        BulletSpeedMod_ = (float) data["BulletSpeedMod"];
        BulletLiveTimeMod_ = (float) data["BLTMod"];
        MagazineCapasityMod_ = (int) data["MagCapMod"];
        AmmoCapasityMod_ = (int) data["AmmoCapMod"];
        DamageMod_ = (int) data["DamageMod"];

        Array<Dictionary> FirePatternArray = (Array<Dictionary>) data["FirePattern"];
        if (FirePatternArray is not null) {
            foreach (Dictionary fireVector in FirePatternArray) {
                Vector3 direction = new Vector3(
                        (float) fireVector["X"],
                        (float) fireVector["Y"],
                        (float) fireVector["Z"]
                        );
                FirePattern_.Add(direction);
            }
        }
        return this;
    }

    string GetContainerNameByType(EModuleType type) {
        switch (type) {
            case EModuleType.BAREL: return "Barels";
            case EModuleType.MAGAZINE: return "Magazines";
            case EModuleType.TRIGGER: return "Triggers";
            case EModuleType.GUNSIGHT: return "Gunsights";
            case EModuleType.AMMO_TYPE: return "AmmoTypes";
        }
        return "";
    }

    public override string ToString()
    {
        string output = base.ToString() + $" {ReloadTimeMod_}, {FireRate_}, {BulletSpeedMod_}, {BulletLiveTimeMod_}, {MagazineCapasityMod_}, {AmmoCapasityMod_}, {DamageMod_}\n";
        return output;
    }
}