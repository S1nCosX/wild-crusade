using Godot;

public class TWeaponStateMachine :  AStateMachine<TWeaponStateMachine, AWeaponState> {
    public AWeapon weapon;
    public TWeaponStateMachine(AWeapon weapon) {
        this.weapon = weapon;
    }
}