using Sandbox;


public sealed class GameLogic : Component
{

	[Property] public PropEmitter SpawnerProp { get; set; }
	[Property] public GameObject StateIndicator { get; set; }
	[Property] public float PropSpawnInterval { get; set; } = 2.0f;
	[Property] public GameObject Player { get; set; }

	private int countPropsSpawned = 0;

	private float timeSinceLastSpawn = 0.0f;
	public bool spawningStarted = false;

	protected override void OnStart()
	{
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

	protected override void OnUpdate()
	{
		TrySpawnProp();
	}

	private void TrySpawnProp() {
		if (!spawningStarted) return;
		timeSinceLastSpawn += Time.Delta;
		if (timeSinceLastSpawn < PropSpawnInterval) return;
		Log.Info("timeSinceLastSpawn >= PropSpawnInterval");
		SpawnerProp.TrySpawnProp();
		timeSinceLastSpawn = 0.0f;
		countPropsSpawned++;
	}

	public void StartSpawning() {
		Log.Info("StartSpawning called");
		if (spawningStarted) return;
		spawningStarted = true;
		StateIndicator.Components.Get<ModelRenderer>().Tint = "#00FF00";
		timeSinceLastSpawn = 0.0f;
		countPropsSpawned = 0;
		SpawnerProp.TrySpawnProp();
	}

	public void StopSpawning() {
		Log.Info("StopSpawning called");
		if (!spawningStarted) return;
		spawningStarted = false;
		StateIndicator.Components.Get<ModelRenderer>().Tint = "#FF0000";
	}

}
