using Sandbox;

public sealed class TriggerStart : Component, Component.ITriggerListener
{

	[Property] public GameLogic GameLogic { get; set; }

	public void OnTriggerEnter(Collider other) {
		var isPlayer = other.Tags.Has("player");
		if (isPlayer){
			GameLogic.StartSpawning();
		}
	}

}
