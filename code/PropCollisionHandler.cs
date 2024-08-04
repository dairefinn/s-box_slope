using Sandbox;

public sealed class PropCollisionHandler : Component, Component.ICollisionListener
{

    public void OnCollisionStart(Collision collision) {
        // If prop collides with the player, disable them
        var isPlayer = collision.Other.GameObject.Tags.Has("player");
		if (isPlayer) {
            collision.Other.GameObject.Enabled = false;
        }
    }

}
