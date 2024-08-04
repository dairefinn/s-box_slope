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
	[Sync] public bool IsCrouching { get; set; } = false;
	[Sync] public bool IsRunning { get; set; } = false;
	[Sync] Angles targetAngle { get; set; } = Angles.Zero;
	private CharacterController characterController;
	private CitizenAnimationHelper animationHelper;
	private SkinnedModelRenderer BodyRenderer { get; set;}

	
	public static PlayerMovement Local {
		get {
			if (!_local.IsValid()) {
				_local = Game.ActiveScene.GetAllComponents<PlayerMovement>().FirstOrDefault(x => x.Network.IsOwner);
			}
			return _local;
		}
	}
	private static PlayerMovement _local = null;

	protected override void OnAwake()
	{
		characterController = Components.Get<CharacterController>();
		animationHelper = Components.Get<CitizenAnimationHelper>();
		BodyRenderer = Body.Components.Get<SkinnedModelRenderer>();
	}

	protected override void OnUpdate()
	{
		if (!Network.IsProxy) {
			UpdateCrouch();
			IsRunning = Input.Down("Run");
			if (Input.Pressed("Jump")) Jump();
			targetAngle = new Angles(0, Head.Transform.Rotation.Yaw(), 0).ToRotation();
		}

		RotateBody();
		UpdateAnimations();
	}

	protected override void OnFixedUpdate()
	{
		if (Network.IsProxy) return; // Don't move if it's another player
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

	void RotateBody() {
		if (Body is null) return;

		float rotateDiff = Body.Transform.Rotation.Distance(targetAngle);

		if(rotateDiff > 50f || characterController.Velocity.Length > 10f) {
			Body.Transform.Rotation = Rotation.Lerp(Body.Transform.Rotation, targetAngle, Time.Delta * 2f);
		}
	}

	void Jump() {
		if (!characterController.IsOnGround) return;

		characterController.Punch(Vector3.Up * JumpForce);
		animationHelper?.TriggerJump();
	}

	void UpdateAnimations() {
		// This is used to hide the player's own body in a first-person game
		// BodyRenderer.RenderType = Network.IsProxy ? ModelRenderer.ShadowRenderType.On : ModelRenderer.ShadowRenderType.ShadowsOnly;
		if (animationHelper is null) return;

		animationHelper.WithWishVelocity(WishVelocity);
		animationHelper.WithVelocity(characterController.Velocity);
		animationHelper.AimAngle = targetAngle;
		animationHelper.IsGrounded = characterController.IsOnGround;
		animationHelper.WithLook(targetAngle.Forward, 1f, 0.75f, 0.5f);
		animationHelper.MoveStyle = CitizenAnimationHelper.MoveStyles.Run;
		animationHelper.DuckLevel = IsCrouching ? 1f : 0f;
	}
	
	void UpdateCrouch() {
		if (characterController is null) return;

		if (Input.Pressed("Crouch") && !IsCrouching) {
			IsCrouching = true;
			characterController.Height /= 2f;
		}

		if (Input.Released("Crouch") && IsCrouching) {
			IsCrouching = false;
			characterController.Height *= 2f;
		}
	}
}
