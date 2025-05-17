using Godot;

public class PlayerInputHandler {
    private const float BASE_SENSOTIVITY = 0.01f;
    private float PersentageSensotivity_ = 1f;
    public PlayerInputData input;
    Vector2 LastMouseVelosity_;

    public void SetMouseVelosity(InputEvent @event) {
        if (@event is InputEventMouseMotion) 
            LastMouseVelosity_ = ((InputEventMouseMotion) @event).Relative * BASE_SENSOTIVITY * PersentageSensotivity_;
    }

    public void Process() {
        input = new PlayerInputData (
            Input.GetVector("left", "right", "forward", "backward"),
            new Vector2(-LastMouseVelosity_.Y, -LastMouseVelosity_.X),
            Input.IsActionPressed("jump"),
            Input.IsActionPressed("fire")
        );
        LastMouseVelosity_ = Vector2.Zero;
    }
}