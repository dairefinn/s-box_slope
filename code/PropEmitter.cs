using System;
using Sandbox;

public sealed class PropEmitter : Component
{
	[Property] public bool SpawningEnabled { get; set; } = true;
	[Property] public Model[] PropList { get; set; }
	[Property] public float PropSpawnInterval { get; set; } = 1f;
	[Property ]public bool spawningStarted { get; set; } = false;
	[Property] public float PropMinimumZ { get; set; } = -5f;
	[Property] public float PropLifetime { get; set; } = 10f;

	private float timeSinceLastSpawn = 0.0f;
	private BoxCollider spawnerBox;
	private Random random = new Random();

	protected override void OnStart() {
		spawnerBox = Components.Get<BoxCollider>();
	}

	protected override void OnUpdate()
	{
		TrySpawnProp();
	}

	public void TrySpawnProp() {
		if (spawningStarted == false) return;
		timeSinceLastSpawn += Time.Delta;
		if (timeSinceLastSpawn < PropSpawnInterval) return;
		timeSinceLastSpawn = 0.0f;
		if (PropList is null) return;
		if (PropList.Length == 0) return;
		if (spawnerBox is null) return;
		if (!SpawningEnabled) return;
		
		var modelUsing = GetRandomModelFromSelection();
		var newProp = CreateProp(modelUsing);
		newProp.Name = "SlopeProp";

		Log.Info("Spawned prop: " + modelUsing.Name);
	}

	private Model GetRandomModelFromSelection() {
		var randomIndex = random.Next(0, PropList.Length);
		return PropList[randomIndex];
	}

	private GameObject CreateProp(Model modelUsing) {
		var newProp = new GameObject();
		newProp.Transform.Position = Transform.Position;

		// Pick a random point along the Y axis of SpawnerProp
		var boxColliderSize = spawnerBox.Scale;
		var boxColliderSizeHalf = boxColliderSize / 2;
		var yStart =  - spawnerBox.Center.y - boxColliderSizeHalf.y;
		var yEnd =  + spawnerBox.Center.y + boxColliderSizeHalf.y;
		var randomY = random.Next((int)yStart, (int)yEnd);
		newProp.Transform.Position = newProp.Transform.Position.WithY(randomY);

		// Rotate a random amount
		var randomRotationX = random.Next(0, 360);
		var randomRotationY = random.Next(0, 360);
		var randomRotationZ = random.Next(0, 360);
		newProp.Transform.Rotation = new Angles(randomRotationX, randomRotationY, randomRotationZ).ToRotation();

		ModelRenderer newPropModelRenderer = newProp.Components.Create<ModelRenderer>();
		newPropModelRenderer.Model = modelUsing;
		ModelCollider newPropModelCollider = newProp.Components.Create<ModelCollider>();
		newPropModelCollider.Model = modelUsing;
		newPropModelCollider.Surface = Surface.FindByName("slippy_wheels");
		Rigidbody newPropRigidBody = newProp.Components.Create<Rigidbody>();
		newPropRigidBody.PhysicsBody.LinearDamping = 0f;
		newPropRigidBody.PhysicsBody.AngularDamping = 0f;
		newProp.Components.Create<PropDespawnHandler>();
		newProp.Components.Create<PropCollisionHandler>();

		newProp.Tags.Add("prop");

		return newProp;
	}

	public void StartSpawning() {
		timeSinceLastSpawn = 0.0f;
		spawningStarted = true;
	}

	public void StopSpawning() {
		spawningStarted = false;
	}

}
