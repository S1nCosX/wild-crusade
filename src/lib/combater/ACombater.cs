using Godot;

public abstract partial class ACombater : CharacterBody3D
{
    protected int ID_;
	protected float SPEED = 5f;
	protected float JUMP_VELOSITY = 4.5f;
    protected float MAX_HEALTH = 100;
	protected const float GRAVITY = 9.8F;
    protected float Health_ = 100;
    SignalBus SignalBus_;
    public override void _Ready()
    {
        base._Ready();
        SignalBus_ = GetNode<SignalBus>("/root/SignalBus");
    }

    public virtual void Heal(float ammount) {
        Health_ += ammount;
        if (Health_ > MAX_HEALTH){
            Health_ = MAX_HEALTH;
            SignalBus_.EmitSignalOverhealedEventHandler(this);
        }
        SignalBus_.EmitSignalHealedEventHandler(this, Health_);
    }
    public virtual void TakeDamage(float ammount) {
        Health_ -= ammount;
        if (Health_ <= 0) {
            SignalBus_.EmitSignalDiedEventHandler(this);
            QueueFree();
        }
        SignalBus_.EmitSignalDamagedEventHandler(this, Health_);
    }
}
