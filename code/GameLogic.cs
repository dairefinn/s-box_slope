using Sandbox;


public sealed class GameLogic : Component
{

	protected override void OnStart()
	{
		base.OnStart();
		Log.Info("GameLogic started");
		
		foreach (var spawnPoint in Scene.GetAllComponents<SpawnPoint>() ) {
			Log.Info("SpawnPoint found: " + spawnPoint.Id);
			if (spawnPoint.Active) {
				Log.Info("SpawnPoint is active");
				SlopePlayer newPlayer = new SlopePlayer(spawnPoint.Transform);
			}
		}
	}

}
