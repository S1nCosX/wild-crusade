using System.Runtime.CompilerServices;

public abstract class AWeaponState : AState<TWeaponStateMachine, AWeaponState>;

public class TWeaponStateReadyToFire() : AWeaponState{
    public override void Process(double delta) {
    }

    public override void Exit(){}
}

public class TWeaponStateReloading() : AWeaponState{
    double TimePassed;
    public override void Enter(TWeaponStateMachine stateMachine)
    {
        base.Enter(stateMachine);
        TimePassed = 0;
    }

    public override void Process(double delta) {
        TimePassed += delta;
        if (TimePassed >= stateMachine.weapon.ReloadTime) {
            stateMachine.ChangeState(new TWeaponStateReadyToFire());
        }
    }

    public override void Exit() {
        stateMachine.weapon.Reload(); 
    }
}

public class TWeaponStateFired() : AWeaponState {
    double TimePassed;
    public override void Enter(TWeaponStateMachine stateMachine)
    {
        base.Enter(stateMachine);
        TimePassed = 0;
        stateMachine.weapon.AddBullesInPool();
    }

    public override void Process(double delta) {
        TimePassed += delta;
        if (TimePassed >= 1 / stateMachine.weapon.FireRate) {
            stateMachine.ChangeState(new TWeaponStateReadyToFire());
        }
    }

    public override void Exit() {}
}