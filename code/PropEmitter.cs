using System;
using Sandbox;

public sealed class PropEmitter : Component
{

	[Property] public Model[] PropList { get; set; }


	private BoxCollider spawnerBox;
	private Random random = new Random();

	protected override void OnStart() {
		spawnerBox = Components.Get<BoxCollider>();
	}

	protected override void OnUpdate()
	{

	}

	public void TrySpawnProp() {
		if (PropList is null) return;
		Log.Info(PropList.Length);
		if (PropList.Length == 0) return;
		if (spawnerBox is null) return;

		
		var modelUsing = GetRandomModelFromSelection();
		var newProp = CreateProp(modelUsing);

		// Scene.Children.Add(newProp);
		Log.Info("Spawned prop: " + modelUsing.Name);
		Log.Info(newProp);
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

		ModelRenderer newPropModelRenderer = newProp.Components.Create<ModelRenderer>();
		newPropModelRenderer.Model = modelUsing;
		ModelCollider newPropModelCollider = newProp.Components.Create<ModelCollider>();
		newPropModelCollider.Model = modelUsing;
		SpawnedProp newPropSpawnedProp = newProp.Components.Create<SpawnedProp>();
		// newPropSpawnedProp.Parent = Component.GameObject;
		newProp.Components.Create<Rigidbody>();

		newProp.Tags.Add("prop");

		return newProp;
	}
}
