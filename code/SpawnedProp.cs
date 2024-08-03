using Sandbox;

public sealed class SpawnedProp : Component, Component.ICollisionListener
{

    private float lifeTime = 10f;
    private float minimumY = -50f;

	protected override void OnUpdate()
	{
		base.OnUpdate();

        // If prop has been alive for too long, destroy it
        lifeTime -= Time.Delta;
        if (lifeTime <= 0) {
            Destroy();
        }

        // If prop is out of bounds, destroy it
        if (Transform.Position.y < minimumY) {
            Destroy();
        }
	}

    public void OnCollision(Collision collision) {
        Log.Info("SpawnedProp.OnCollision");
        Log.Info(collision);
    }

}
