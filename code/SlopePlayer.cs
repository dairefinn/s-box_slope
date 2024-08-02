using Sandbox;

public sealed class SlopePlayer : Component
{
    public new GameTransform Transform { get; set; }
    public float Health { get; set; } = 0.0f;

    public SlopePlayer(GameTransform transform)
    {
        Log.Info("SlopePlayer created at " + transform.Position);
        this.Transform = transform;
    }

    protected override void OnUpdate() {
        base.OnUpdate();

        Log.Info("SlopePlayer updated");
        if (this.Health <= 0.0f) {
            Log.Info("SlopePlayer is dead");
            this.Enabled = false;
        }
    }

}
