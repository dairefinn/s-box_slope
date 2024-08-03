using Sandbox;

public sealed class TriggerCleanup : Component, Component.ITriggerListener
{

	public void OnTriggerEnter(Collider other) {
		Log.Info("TriggerCleanup.OnTriggerEnter");
		var prop = other.Components.Get<SpawnedProp>();
		if (prop is null) return;
		Log.Info("Destroying prop: " + prop);
		other.Destroy();
	}

}
