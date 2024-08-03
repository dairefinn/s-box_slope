using Sandbox;

public sealed class TriggerCleanup : Component, Component.ITriggerListener
{

	public void OnTriggerEnter(Collider other) {
		// FIXME: This don't work. Prop might need a box collider.
		var isProp = other.Tags.Has("prop");
		if (!isProp) return;
		other.Destroy();
	}

}
