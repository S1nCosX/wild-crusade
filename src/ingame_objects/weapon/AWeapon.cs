using System.Linq;
using System.Net.Quic;
using Godot;
using Godot.Collections;

public abstract partial class AWeapon : Node3D {
//    Array <IEffect> Effects_;
    protected TWeaponStateMachine StateMachine_;
    protected SignalBus SignalBus_;

    protected ItemProvider ItemProvider_;
    protected Dictionary<EModuleType, Module> Modules_;
    
    public string Name;
    public float ReloadTime;
    public float FireRate;
    protected int CurrentMag_;
    protected int MagCapascity_;
    protected int CurrentAmmo_;
    protected float AmmoCapasity_;
    protected float Damage_;

    protected double BulletLiveTime_;
    protected Bullet BaseBullet;
    protected Array<Bullet> BulletsPool_;
    protected Array <Vector3> FirePattern_;
    private PhysicsRayQueryParameters3D query;
    protected Rid Owner_;

    public override void _Ready()
    {
        base._Ready();
        BulletsPool_ = new Array<Bullet>();
        FirePattern_ = new Array<Vector3>();
        Modules_ = new Dictionary<EModuleType, Module>();
        StateMachine_ = new TWeaponStateMachine(this);
        StateMachine_.ChangeState(new TWeaponStateReadyToFire());
        query = new PhysicsRayQueryParameters3D();
        ItemProvider_ = GetNode<ItemProvider>("/root/ItemProvider");
        SignalBus_ = GetNode<SignalBus>("/root/SignalBus");
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        StateMachine_.Process(delta);
        ProcessBullets(delta);
    }

    public void Fire(Vector3 startLocation, Vector3 direction)
    {
        if (StateMachine_.CurrentState is TWeaponStateReadyToFire)
        {
            StateMachine_.ChangeState(new TWeaponStateFired());
            BaseBullet.SetStartLocation_(startLocation);
            BaseBullet.Direction = direction;
        }
        SignalBus_.EmitSignalCurrentMagEventHandler(CurrentMag_);
    }

    public virtual void Reload()
    {
        if (CurrentAmmo_ > MagCapascity_)
        {
            CurrentAmmo_ -= MagCapascity_;
            CurrentMag_ = MagCapascity_;
        }
        else
        {
            CurrentMag_ = CurrentAmmo_;
            CurrentAmmo_ = 0;
        }
        SignalBus_.EmitSignalCurrentMagEventHandler(CurrentMag_);
        
        SignalBus_.EmitSignalCurrentMagEventHandler(CurrentAmmo_);
    }

    public void AddBullesInPool() {
        foreach (Vector3 patternVector in FirePattern_) {
            Bullet bufferBullet = new Bullet(BaseBullet);
            bufferBullet.Direction = (bufferBullet.Direction + patternVector).Normalized();
            BulletsPool_.Add(bufferBullet);
        }
    }

    void ProcessBullets(double delta) {
        PhysicsDirectSpaceState3D DirectSpace = GetWorld3D().DirectSpaceState;

        foreach (Bullet bullet in BulletsPool_) {
            if (bullet.LiveTime >= BulletLiveTime_) {
                BulletsPool_.Remove(bullet);
                continue;
            }
            query.From = bullet.GetCurrentLocation();
            bullet.Process(delta);
            query.To = bullet.GetCurrentLocation();
            Dictionary colided = DirectSpace.IntersectRay(query);
            if (colided.ToArray().Length > 0 && (Node3D)colided["collider"] is ACombater) {
                ((ACombater) colided["collider"]).TakeDamage(Damage_);
                BulletsPool_.Remove(bullet);
            }
            
        }
    }

    protected void FillStatsFromModules() {
        ReloadTime = Modules_[EModuleType.MAGAZINE].ReloadTimeMod;
        FireRate = Modules_[EModuleType.TRIGGER].FireRate;
        MagCapascity_ = Modules_[EModuleType.MAGAZINE].MagazineCapasityMod * Modules_[EModuleType.AMMO_TYPE].MagazineCapasityMod;
        AmmoCapasity_ = MagCapascity_ * Modules_[EModuleType.AMMO_TYPE].AmmoCapasityMod;
        Damage_ = Modules_[EModuleType.AMMO_TYPE].DamageMod * Modules_[EModuleType.TRIGGER].DamageMod * Modules_[EModuleType.BAREL].DamageMod ;
        BaseBullet = new Bullet(
            Vector3.Zero,
            Vector3.Zero,
            Modules_[EModuleType.AMMO_TYPE].BulletSpeedMod * Modules_[EModuleType.GUNSIGHT].BulletSpeedMod * Modules_[EModuleType.BAREL].BulletSpeedMod
        );
        BulletLiveTime_ = Modules_[EModuleType.AMMO_TYPE].BulletLiveTimeMod * Modules_[EModuleType.GUNSIGHT].BulletLiveTimeMod * Modules_[EModuleType.BAREL].BulletLiveTimeMod;
        FirePattern_ = Modules_[EModuleType.BAREL].FirePattern;
    }

    public void addOwner(Rid Owner) {
        Owner_ = Owner;
        query.Exclude = new Array<Rid>{Owner_};
    }
}