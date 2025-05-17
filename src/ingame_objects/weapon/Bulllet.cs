using Godot;

[Tool]
public partial class Bullet : Resource{
    [Export] Vector3 CurrentLocation_;
    [Export] float Speed_;
    [Export] double LiveTime_;
    public double LiveTime => LiveTime_;
    [Export] public Vector3 Direction;
    
    public Bullet(Vector3 startLocation, Vector3 direction, float speed) {
        CurrentLocation_ = startLocation;
        Direction = direction.Normalized();
        Speed_ = speed;
        LiveTime_ = 0;
    }
    public Bullet(Bullet bullet)
    {
        CurrentLocation_ = bullet.CurrentLocation_;
        Direction = bullet.Direction;
        Speed_ = bullet.Speed_;
        LiveTime_ = 0;
    }


    public Vector3 Process(double delta) {
        LiveTime_ -= delta;
        CurrentLocation_ += Direction * Speed_ * (float) delta;
        return CurrentLocation_;
    }

    public Vector3 GetCurrentLocation() {
        return CurrentLocation_;
    }

    public void SetStartLocation_(Vector3 startLocation) {
        CurrentLocation_ = startLocation;
    }
}