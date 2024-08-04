using Sandbox;

public sealed class SpawnedProp : Component, Component.ICollisionListener
{

    private float lifeTime = 5f;

	protected override void OnAwake()
	{
		base.OnAwake();

        lifeTime = GameLogic.PropLifetime;
	}

	protected override void OnUpdate()
	{
		base.OnUpdate();

        // If prop is out of bounds, destroy it
        if (Transform.Position.z < GameLogic.PropMinimumZ) {
            Log.Info("Prop is out of bounds, destroying it");
            GameObject.Destroy();
            return;
        }

        // If prop has been alive for too long, destroy it
        lifeTime -= Time.Delta;
        if (lifeTime <= 0) {
            Log.Info("Prop has been alive for too long, destroying it");
            GameObject.Destroy();
            return;
        }

	}

    public void OnCollision(Collision collision) {
        Log.Info("SpawnedProp.OnCollision");
        Log.Info(collision);
    }

}
