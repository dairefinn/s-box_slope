using Sandbox;

public sealed class PropDespawnController : Component, Component.ICollisionListener
{

    [Property] public bool DestroyOnMinimumZ = true;
    [Property] public bool DestroyOnLifetime = true;

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
        if (DestroyOnMinimumZ && Transform.Position.z < GameLogic.PropMinimumZ) {
            Log.Info("Prop is out of bounds, destroying it");
            GameObject.Destroy();
            return;
        }

        // If prop has been alive for too long, destroy it
        if (DestroyOnLifetime) {
            lifeTime -= Time.Delta;
            if (lifeTime <= 0) {
                Log.Info("Prop has been alive for too long, destroying it");
                GameObject.Destroy();
                return;
            }
        }
	}

    public void OnCollision(Collision collision) {
        Log.Info("SpawnedProp.OnCollision");
        Log.Info(collision);
    }


    public void OnCollision(Collider other) {
        Log.Info("SpawnedProp.OnCollision");
        Log.Info(other);
    }

}
