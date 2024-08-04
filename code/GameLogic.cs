using Sandbox;


public sealed class GameLogic : Component
{

	[Property] public PropEmitter SpawnerProp { get; set; }
	[Property] public GameObject StateIndicator { get; set; }
	[Property] public NetworkHelper NetworkHelper { get; set; }

	protected override void OnStart()
	{
		
	}

	public void StartSpawning() {
		SpawnerProp.StartSpawning();
		StateIndicator.Components.Get<ModelRenderer>().Tint = "#00FF00";
	}

	public void StopSpawning() {
		SpawnerProp.StopSpawning();
		StateIndicator.Components.Get<ModelRenderer>().Tint = "#FF0000";
	}

	public void SetPlayerAsWinner(GameObject player) {
		if (player is null) return;
		StopSpawning();
		// TODO: Respawn players using NetworkHelper
		// Player.Enabled /= false;
		// StartGame();
	}
}
