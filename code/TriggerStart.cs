using Sandbox;

public sealed class TriggerStart : Component, Component.ITriggerListener
{

	[Property] public GameLogic GameLogic { get; set; }

	public void OnTriggerEnter(Collider other) {
		var player = other.Components.Get<PlayerMovement>();
		if (player is null) return;
		GameLogic.StartSpawning();
	}

}
