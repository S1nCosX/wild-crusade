using Godot;

[GlobalClass]
public partial class SignalBus : Node
{
    //AcombaterSignals
    [Signal]
    public delegate void DiedEventHandler(Node3D died);
    [Signal]
    public delegate void HealedEventHandler(Node3D healed, float newHealth);
    [Signal]
    public delegate void DamagedEventHandler(Node3D damaged, float newHealth);
    [Signal]
    public delegate void OverhealedEventHandler(Node3D overhealed);

    public void EmitSignalDiedEventHandler(Node3D died) => EmitSignal(SignalName.Died, died);
    public void EmitSignalHealedEventHandler(Node3D healed, float newHealth) => EmitSignal(SignalName.Healed, healed, newHealth);
    public void EmitSignalDamagedEventHandler(Node3D damaged, float newHealth) => EmitSignal(SignalName.Damaged, damaged, newHealth);
    public void EmitSignalOverhealedEventHandler(Node3D overhealed) => EmitSignal(SignalName.Overhealed, overhealed);

    //AWeapon
    [Signal]
    public delegate void CurrentMagEventHandler(int value);
    public void EmitSignalCurrentMagEventHandler(int value) => EmitSignal(SignalName.CurrentMag, value);
    [Signal]
    public delegate void CurrentAmmoEventHandler(int value);
    public void EmitSignalCurrentAmmoEventHandler(int value) => EmitSignal(SignalName.CurrentAmmo, value);

    //EnemyWeapon
    [Signal]
    public delegate void GiveModuleEventHandler(Module module);
    public void EmitSignalGiveModuleEventHandler(Module module) => EmitSignal(SignalName.GiveModule, module);

    //Player
    [Signal]
    public delegate void SwapWeaponEventHandler(AWeapon weapon);
    public void EmitSignalSwapWeaponEventHandler(AWeapon weapon) => EmitSignal(SignalName.SwapWeapon, weapon);
}