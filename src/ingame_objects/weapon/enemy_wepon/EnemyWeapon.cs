using Godot;
using Godot.Collections;

public partial class EnemyWeapon : AWeapon {
    int ID_;
    public override void _Ready() {
        base._Ready();
    }

    public override void Reload()
    {
        base.Reload();
        if (CurrentMag_ == 0)
            SignalBus_.EmitSignalGiveModuleEventHandler(Modules_[GetTypeByInt(new RandomNumberGenerator().RandiRange(1, 5))]);
    }

    public void SetID(int ID) {
        ID_ = ID;
        Dictionary<EModuleType, int> modules = ItemProvider_.Weapons[ID_];
        Modules_.Add(EModuleType.BAREL, ItemProvider_.Barrels[modules[EModuleType.BAREL]]);
        Modules_.Add(EModuleType.MAGAZINE, ItemProvider_.Magazines[modules[EModuleType.MAGAZINE]]);
        Modules_.Add(EModuleType.TRIGGER, ItemProvider_.Triggers[modules[EModuleType.TRIGGER]]);
        Modules_.Add(EModuleType.GUNSIGHT, ItemProvider_.Gunsights[modules[EModuleType.GUNSIGHT]]);
        Modules_.Add(EModuleType.AMMO_TYPE, ItemProvider_.AmmoTypes[modules[EModuleType.AMMO_TYPE]]);
        FillStatsFromModules();
        CurrentAmmo_ = 0;
    }

    public EModuleType GetTypeByInt(int num) {
        switch (num) {
            case 1: return EModuleType.BAREL;
            case 2: return EModuleType.MAGAZINE;
            case 3: return EModuleType.TRIGGER;
            case 4: return EModuleType.GUNSIGHT;
            case 5: return EModuleType.AMMO_TYPE;
        }
        return EModuleType.NONE;
    }
}