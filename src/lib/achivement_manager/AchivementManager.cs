using Godot;

[GlobalClass]
public partial class AchivementManager : Node {
    SignalBus SignalBus_;

    public override void _Ready()
    {
        base._Ready();
        SignalBus_ = GetNode<SignalBus>("/root/SignalBus");
    }
}