using Sandbox;

public sealed class PropDespawnHandler : Component
{

    [Property] public bool DestroyOnMinimumZ = true;
    [Property] public bool DestroyOnLifetime = true;
    [Property] public float lifeTime = 5f;
    [Property] public float minimumZ = -5f;

	protected override void OnAwake()
	{
		base.OnAwake();

	}

	protected override void OnUpdate()
	{
		base.OnUpdate();

        // If prop is out of bounds, destroy it
        if (DestroyOnMinimumZ && Transform.Position.z < minimumZ) {
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

}
