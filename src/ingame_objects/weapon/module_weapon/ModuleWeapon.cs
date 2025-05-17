using System.Collections.Generic;
using Godot;

public partial class ModuleWeapon : AWeapon {
    
    public override void _Ready()
    {
        base._Ready();
        FirePattern_.Add(Vector3.Zero);
    }
}