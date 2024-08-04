using Sandbox;


public sealed class GameLogic : Component
{

	[Property] public PropEmitter SpawnerProp { get; set; }
	[Property] public GameObject StateIndicator { get; set; }
	[Property] public GameObject Player { get; set; }

	protected override void OnStart()
	{
		StartGame();
	}

	private void StartGame() {
		TrySpawnPlayer();
	}
	
	private void TrySpawnPlayer() {
		if (Player is null) return;
		if (Player.Enabled) return;

		// Finds the first active spawn point and spawns the player there
		foreach (var spawnPoint in Scene.GetAllComponents<SpawnPoint>() ) {
			if (spawnPoint.Active) {
				Player.Enabled = true;
				Player.Transform.Position = spawnPoint.Transform.Position;
				Player.Transform.Rotation = spawnPoint.Transform.Rotation;
				return;
			}
		}
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
		Player.Enabled = false;
		StartGame();
	}
}
