using System;
using Sandbox;

public sealed class SpawnerProp : Component
{

	[Property] public Model[] PropList { get; set; }


	private  BoxCollider boxCollider;

	protected override void OnStart() {
		boxCollider = Components.Get<BoxCollider>();
	}

	protected override void OnUpdate()
	{

	}

	public void SpawnProp() {
		if (PropList is null) return;
		if (PropList.Length == 0) return;
		if (boxCollider is null) return;

		Random random = new Random();
		var randomIndex = random.Next(0, PropList.Length);

		var modelUsing = PropList[randomIndex];

		var newProp = new GameObject();
		newProp.Transform.Position = Transform.Position;

		// Pick a random point along the Y axis of SpawnerProp
		var boxColliderSize = boxCollider.Scale;
		var boxColliderSizeHalf = boxColliderSize / 2;
		var yStart =  - boxCollider.Center.y - boxColliderSizeHalf.y;
		var yEnd =  + boxCollider.Center.y + boxColliderSizeHalf.y;
		var randomY = random.Next((int)yStart, (int)yEnd);
		newProp.Transform.Position = newProp.Transform.Position.WithY(randomY);

		ModelRenderer newPropModelRenderer = newProp.Components.Create<ModelRenderer>();
		newPropModelRenderer.Model = modelUsing;
		Rigidbody newPropRigidbody = newProp.Components.Create<Rigidbody>();

		newProp.Tags.Add("prop");

		// Scene.Children.Add(newProp);
		Log.Info("Spawned prop: " + modelUsing.Name);
		Log.Info(newProp);
	}

}
