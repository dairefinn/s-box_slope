using System;
using Sandbox;


public sealed class GameLogic : Component
{

	[Property] public PropEmitter SpawnerProp { get; set; }
	[Property] public GameObject StateIndicator { get; set; }
	[Property] public float PropSpawnInterval { get; set; } = 2.0f;

	private int countPropsSpawned = 0;

	private float timeSinceLastSpawn = 0.0f;
	public Boolean spawningStarted = false;

	// protected override void OnStart()
	// {
	// 	foreach (var spawnPoint in Scene.GetAllComponents<SpawnPoint>() ) {
	// 		// Log.Info("SpawnPoint found: " + spawnPoint.Id);
	// 		if (spawnPoint.Active) {
	// 			// Log.Info("SpawnPoint is active");
	// 			// SlopePlayer newPlayer = new SlopePlayer(spawnPoint.Transform);
	// 		}
	// 	}
	// }

	protected override void OnUpdate()
	{
		TrySpawnProp();
	}

	private void TrySpawnProp() {
		if (!spawningStarted) return;
		timeSinceLastSpawn += Time.Delta;
		if (timeSinceLastSpawn < PropSpawnInterval) return;
		Log.Info("timeSinceLastSpawn >= PropSpawnInterval");
		SpawnerProp.SpawnProp();
		timeSinceLastSpawn = 0.0f;
		countPropsSpawned++;
	}

	public void StartSpawning() {
		Log.Info("StartSpawning called");
		if (spawningStarted) return;
		spawningStarted = true;
		StateIndicator.Components.Get<ModelRenderer>().Tint = "#00FF00";
		timeSinceLastSpawn = 0.0f;
		SpawnerProp.SpawnProp();
	}

	public void StopSpawning() {
		Log.Info("StopSpawning called");
		if (!spawningStarted) return;
		spawningStarted = false;
		StateIndicator.Components.Get<ModelRenderer>().Tint = "#FF0000";
	}

}
