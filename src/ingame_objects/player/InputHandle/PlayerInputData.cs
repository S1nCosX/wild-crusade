using Godot;

public class PlayerInputData {
    public Vector2 MovementDirection;
    public Vector2 CameraRotation;
    public bool Jumped;
    public bool Fired;

    public PlayerInputData(
        Vector2 MovementDirection,
        Vector2 CameraRotation,
        bool Jumped,
        bool Fired
    ) {
        this.MovementDirection = MovementDirection;
        this.CameraRotation = CameraRotation;
        this.Jumped = Jumped;
        this.Fired = Fired;
    }
}