using Sandbox;

public sealed class TriggerWin : Component, Component.ITriggerListener
{

	[Property] public GameLogic GameLogic { get; set; }

	public void OnTriggerEnter(Collider collider) {
		var player = collider.Components.Get<PlayerMovement>();
		if (player is null) return;
		GameLogic.SetPlayerAsWinner(collider.GameObject);
	}

}
