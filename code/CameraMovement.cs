public sealed class CameraMovement : Component
{
	[Property] public float Distance { get; set; } = 0f;
	
	public bool IsFirstPerson => Distance == 0f;
	private Vector3 CurrentOffset = Vector3.Zero;
	private CameraComponent Camera;

	private PlayerMovement Player { get; set; }
	private GameObject Body { get; set; }
	private GameObject Head { get; set; }

	protected override void OnAwake()
	{
		Camera = Components.Get<CameraComponent>();
	}

	protected override void OnUpdate()
	{
		if (Player is null) {
			GetPlayerReferences();
			return;
		}

		var eyeAngles = Head.Transform.Rotation.Angles();
		eyeAngles.pitch += Input.MouseDelta.y * 0.1f;
		eyeAngles.yaw -= Input.MouseDelta.x * 0.1f;
		eyeAngles.roll = 0f;
		eyeAngles.pitch = eyeAngles.pitch.Clamp( -89.9f, 89.9f );
		Head.Transform.Rotation = eyeAngles.ToRotation();

		var targetOffset = Vector3.Zero;
		if (Player.IsCrouching) targetOffset += Vector3.Down * 32f;
		CurrentOffset = Vector3.Lerp(CurrentOffset, targetOffset, Time.Delta * 10f);

		if(Camera is not null) {
			var camPos = Head.Transform.Position + CurrentOffset;

			if (!IsFirstPerson) {
				var camForward = eyeAngles.ToRotation().Forward;
				var camTrace = Scene.Trace.Ray(camPos, camPos - (camForward * Distance))
					.WithoutTags("player", "trigger")
					.Run();
				
				if (camTrace.Hit) {
					camPos = camTrace.HitPosition + camTrace.Normal;
				} else {
					camPos = camTrace.EndPosition;
				}
			}

			Camera.Transform.Position = camPos;
			Camera.Transform.Rotation = eyeAngles.ToRotation();
		}
	}

	private void GetPlayerReferences() {
		Player = PlayerMovement.Local;
		Head = null;
		Body = null;

		if (Player is null) {
			Log.Error("Failed to find Player instance to attach camera to");
			return;
		}

		Body = Player.Body;
		Head = Player.Head;
	}
}
