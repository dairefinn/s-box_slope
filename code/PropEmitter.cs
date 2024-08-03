using System;
using Sandbox;

public sealed class PropEmitter : Component
{

	[Property] public Model[] PropList { get; set; }


	private  BoxCollider spawnerBox;

	protected override void OnStart() {
		spawnerBox = Components.Get<BoxCollider>();
	}

	protected override void OnUpdate()
	{

	}

	public void SpawnProp() {
		if (PropList is null) return;
		Log.Info(PropList.Length);
		if (PropList.Length == 0) return;
		if (spawnerBox is null) return;

		Random random = new Random();
		var randomIndex = random.Next(0, PropList.Length);

		var modelUsing = PropList[randomIndex];

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
		ModelCollider newPropModelCollider = newProp.Components.Create<ModelCollider>();
		newPropModelCollider.Model = modelUsing;
		SpawnedProp newPropSpawnerPropOutput = newProp.Components.Create<SpawnedProp>();
		newPropModelRenderer.Model = modelUsing;
		Rigidbody newPropRigidbody = newProp.Components.Create<Rigidbody>();

		

		newProp.Tags.Add("prop");

		// Scene.Children.Add(newProp);
		Log.Info("Spawned prop: " + modelUsing.Name);
		Log.Info(newProp);
	}

}
