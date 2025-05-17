using Godot;
using Godot.Collections;

public partial class Player : ACombater
{
	private Camera3D Camera_;
	private Node3D Head_;
	private PlayerInputHandler InputHandler_;
	private Vector2 CameraRotation_;
	private Vector3 Velocity_;
	private Node3D WeaponSlot_;
	private Array<AWeapon> Weapons_;
	private int UsingSlot = 0;
	protected SignalBus SignalBus_;

	public override void _Ready() {
		//TODELETE
		Input.MouseMode = Input.MouseModeEnum.Captured;
		base._Ready();
		Head_ = GetNode<Node3D>("Head");
		Camera_ = Head_.GetNode<Camera3D>("Camera");
		WeaponSlot_ = GetNode<Node3D>("Weapon");
		InputHandler_ = new PlayerInputHandler();
		Weapons_ = new Array<AWeapon>();
		Weapons_.Add(new EnemyWeapon());
		SignalBus_ = GetNode<SignalBus>("/root/SignalBus");
		SignalBus_.Connect(SignalBus.SignalName.GiveModule, new Callable(this, nameof(PutModuleIntoInventory)));
		EquipWeapon(0);
	}

	public override void _Process(double delta)
	{
		base._Process(delta);
		InputHandler_.Process();
		RotateCamera();
		FireProcess();
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		base._UnhandledKeyInput(@event);
		InputHandler_.SetMouseVelosity(@event);
	}

	void RotateCamera()
	{
		CameraRotation_.X += InputHandler_.input.CameraRotation.X;
		CameraRotation_.Y += InputHandler_.input.CameraRotation.Y;
		CameraRotation_.X = Mathf.Clamp(CameraRotation_.X, Mathf.DegToRad(-60), Mathf.DegToRad(60));
		Head_.Rotation = new Vector3(0, CameraRotation_.Y, 0);
		Camera_.Rotation = new Vector3(CameraRotation_.X, 0, 0);
	}

	void FireProcess()
	{
		if (InputHandler_.input.Fired)
		{
			WeaponSlot_.GetNode<AWeapon>("Weapon")?.Fire(Camera_.GlobalTransform.Origin, -Camera_.GlobalTransform.Basis.Z.Normalized());
		}
	}

	private Vector3 UpdateVelocity(double delta)
	{
		Vector3 direction = (Head_.Basis *
				   new Vector3(
					   InputHandler_.input.MovementDirection.X,
					   0,
					   InputHandler_.input.MovementDirection.Y
				   )
			   ).Normalized();

		if (direction != Vector3.Zero)
		{
			Velocity_.X = direction.X * SPEED;
			Velocity_.Z = direction.Z * SPEED;
		}
		else
		{
			Velocity_.X = Mathf.MoveToward(Velocity_.X, 0, SPEED);
			Velocity_.Z = Mathf.MoveToward(Velocity_.Z, 0, SPEED);
		}

		Velocity_.Y = IsOnFloor() && InputHandler_.input.Jumped ? JUMP_VELOSITY : Velocity_.Y;
		Velocity_.Y -= IsOnFloor() ? 0 : GRAVITY * (float)delta;
		return Velocity_;
	}

	public override void _PhysicsProcess(double delta)
	{
		Velocity = UpdateVelocity(delta);
		MoveAndSlide();
	}

	void SwapWeapon()
	{
		UsingSlot = (UsingSlot + 1) % Weapons_.Count;
		EquipWeapon(UsingSlot);
	}

	void EquipWeapon(int slot)
	{
		if (WeaponSlot_.GetChildCount() != 0)
			WeaponSlot_.GetNode<AWeapon>("Weapon")?.QueueFree();

		if (Weapons_[slot] is EnemyWeapon) {
			PackedScene weapon = (PackedScene)ResourceLoader.Load("res://src/ingame_objects/weapon/enemy_weapon.tscn");
			var weaponInstance = (EnemyWeapon)weapon.Instantiate();
			WeaponSlot_.AddChild(weaponInstance);
			weaponInstance.SetID(0);
			weaponInstance.Name = "Weapon";
			weaponInstance.addOwner(GetRid());
		}
		else if (Weapons_[slot] is ModuleWeapon) {
			PackedScene weapon = (PackedScene)ResourceLoader.Load("res://src/ingame_objects/weapon/module_weapon.tsnc");
			var weaponInstance = (ModuleWeapon)weapon.Instantiate();
			weaponInstance.Name = "Weapon";
			WeaponSlot_.AddChild(weaponInstance);
		}
	}

	void PutModuleIntoInventory(Module module) {
		
	}
}

