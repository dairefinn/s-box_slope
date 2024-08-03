using Sandbox;
using Sandbox.Citizen;

public sealed class PlayerMovement : Component
{
	// Movement properties
	[Property] public float GroundControl { get; set;} = 4.0f;
	[Property] public float AirControl { get; set;} = 0.1f;
	[Property] public float MaxForce { get; set;} = 50f;
	[Property] public float Speed { get; set;} = 160f;
	[Property] public float RunSpeed { get; set;} = 290f;
	[Property] public float CrouchSpeed { get; set;} = 90f;
	[Property] public float JumpForce { get; set;} = 400f;

	// Object references
	[Property] public GameObject Head { get; set;}
	[Property] public GameObject Body { get; set;}

	// Member variables
	public Vector3 WishVelocity = Vector3.Zero;
	public bool IsCrouching = false;
	public bool IsRunning = false;
	private CharacterController characterController;
	private CitizenAnimationHelper animationHelper;

	protected override void OnAwake()
	{
		characterController = Components.Get<CharacterController>();
		animationHelper = Components.Get<CitizenAnimationHelper>();
	}

	protected override void OnUpdate()
	{
		IsCrouching = Input.Down("Crouch");
		IsRunning = Input.Down("Run");
	}

	protected override void OnFixedUpdate()
	{
		BuildWishVelocity();
		Move();
	}

	void BuildWishVelocity() {
		WishVelocity = 0;

		var rot = Head.Transform.Rotation;
		if (Input.Down("Forward")) WishVelocity += rot.Forward;
		if (Input.Down("Backward")) WishVelocity -= rot.Forward;
		if (Input.Down("Left")) WishVelocity -= rot.Right;
		if (Input.Down("Right")) WishVelocity += rot.Right;

		WishVelocity = WishVelocity.WithZ(0);
		if (!WishVelocity.IsNearZeroLength) WishVelocity = WishVelocity.Normal;

		if (IsCrouching) WishVelocity *= CrouchSpeed;
		else if (IsRunning) WishVelocity *= RunSpeed;
		else WishVelocity *= Speed;
	}

	void Move() {
		var gravity = Scene.PhysicsWorld.Gravity;

		if (characterController.IsOnGround) {
			characterController.Velocity = characterController.Velocity.WithZ(0);
			characterController.Accelerate(WishVelocity);
			characterController.ApplyFriction(GroundControl);
		} else {
			characterController.Velocity += gravity * Time.Delta * 0.5f;
			characterController.Accelerate(WishVelocity.ClampLength(MaxForce));
			characterController.ApplyFriction(AirControl);
		}

		characterController.Move();

		if (!characterController.IsOnGround) {
			characterController.Velocity += gravity * Time.Delta * 0.5f;
		} else {
			characterController.Velocity = characterController.Velocity.WithZ(0);
		}
	}
}
