using Godot;
using Godot.Collections;

public partial class Hud : CanvasLayer {
    ProgressBar HealthBar_;
    Label CurrentHealthLabel_;
    Label MaxHealthLabel_;
    float CurrentHealth_;
    float MaxHealth_;
    RichTextLabel CoughtSignals_;
    SignalBus SignalBus_;

    public override void _Ready()
    {
        SignalBus_ = GetNode<SignalBus>("/root/SignalBus");
        base._Ready();
        HealthBar_ = GetNode<ProgressBar>("HealthBar/Bar/HealtBar");
        CurrentHealthLabel_ = GetNode<Label>("HealthBar/HealthInNums/Current");
        MaxHealthLabel_ = GetNode<Label>("HealthBar/HealthInNums/Max");
        CoughtSignals_ = GetNode<RichTextLabel>("Signals"); 
        SignalBus_.Connect(SignalBus.SignalName.Damaged, new Callable(this, nameof(OnAnySignal)));
        SignalBus_.Connect(SignalBus.SignalName.Healed, new Callable(this, nameof(PlayerHealedSignal)));
        SignalBus_.Connect(SignalBus.SignalName.Damaged, new Callable(this, nameof(PlayerDamagedSignal)));
    }

    void OnAnySignal(Node3D node, float ammount)
    {
        string Text = "(";
        Text += node.ToString() + ",";
        Text += ammount.ToString() + ",";
        Text += ")\n";
        CoughtSignals_.Text += Text;
    }
    void PlayerHealedSignal(Node3D node, float ammount) {
        if (node is Player)
            CurrentHealth_ = ammount;
    }
    void PlayerDamagedSignal(Node3D node, float ammount) {
        if (node is Player)
            CurrentHealth_ = ammount;
    }
}
