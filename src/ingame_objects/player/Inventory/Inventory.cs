using Godot;
using Godot.Collections;

public class Inventory
{
    Array<Module> OwenedModules_;
    Array<Artifact> OwenedArtifacts_;

    public Inventory()
    {
        OwenedArtifacts_ = new Array<Artifact>();
        OwenedModules_ = new Array<Module>();
    }
}