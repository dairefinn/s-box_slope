using Sandbox;

public sealed class CameraMovement : Component
{
	[Property] public PlayerMovement Player { get; set; }
	[Property] public GameObject Body { get; set; }
	[Property] public GameObject Head { get; set; }
	[Property] public float Distance { get; set; } = 0f;

	public bool IsFirstPerson => Distance == 0f;
	private CameraComponent Camera;

	protected override void OnAwake()
	{
		Camera = Components.Get<CameraComponent>();
	}

	protected override void OnUpdate()
	{
		var eyeAngles = Head.Transform.Rotation.Angles();
		eyeAngles.pitch += Input.MouseDelta.y * 0.1f;
		eyeAngles.yaw -= Input.MouseDelta.x * 0.1f;
		eyeAngles.roll = 0f;
		eyeAngles.pitch = eyeAngles.pitch.Clamp( -89.9f, 89.9f );
		Head.Transform.Rotation = eyeAngles.ToRotation();

		if(Camera is not null) {
			var camPos = Head.Transform.Position;

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
}
