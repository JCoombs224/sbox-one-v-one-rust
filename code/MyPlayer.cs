using Sandbox;
using System;
using System.Linq;
using System.Numerics;
using System.Threading;
using SWB_Base;

partial class MyPlayer : PlayerBase
{
	private TimeSince timeSinceDropped;
	private TimeSince timeSinceJumpReleased;

	private DamageInfo lastDamage;

	public int KillCount { get; set; }

	/// <summary>
	/// The clothing container is what dresses the citizen
	/// </summary>
	public Clothing.Container Clothing = new();

	public MyPlayer() : base()
	{
		Inventory = new GameInventory( this );
	}

	/// <summary>
	/// Initialize using this client
	/// </summary>
	public MyPlayer( Client cl ) : this()
	{
		// Load clothing from client data
		Clothing.LoadFromClient( cl );
	}


	public override void Respawn()
	{
		base.Respawn();

		SetModel( "models/citizen/citizen.vmdl" );

		// Use WalkController for movement (you can make your own PlayerController for 100% control)
		Controller = new WalkController();
		// Use StandardPlayerAnimator  (you can make your own PlayerAnimator for 100% control)
		Animator = new StandardPlayerAnimator();
		Camera = new FirstPersonCamera();
		//LastCamera = MainCamera;

		EnableAllCollisions = true;
		EnableDrawing = true;
		EnableHideInFirstPerson = true;
		EnableShadowInFirstPerson = true;

		Clothing.DressEntity( this );

		Inventory.Add( new Intervention(), true );

		GiveAmmo( AmmoType.Sniper, 45 );

	}

	public override void OnKilled()
	{
		base.OnKilled();

		Inventory.DeleteContents();

		Controller = null;
		Camera = new SpectateRagdollCamera();

		EnableAllCollisions = false;
		EnableDrawing = false;
	}

	[ServerCmd( "inventory_current" )]
	public static void SetInventoryCurrent( string entName )
	{
		var target = ConsoleSystem.Caller.Pawn;
		if ( target == null ) return;

		var inventory = target.Inventory;
		if ( inventory == null )
			return;

		for ( int i = 0; i < inventory.Count(); ++i )
		{
			var slot = inventory.GetSlot( i );
			if ( !slot.IsValid() )
				continue;

			if ( !slot.ClassInfo.IsNamed( entName ) )
				continue;

			inventory.SetActiveSlot( i, false );

			break;
		}
	}

	[ServerCmd("give_weapon")]
	public static void GiveWeapon(string entName)
	{
		var target = ConsoleSystem.Caller.Pawn;
		if ( target == null ) return;

		var inventory = target.Inventory;
		if ( inventory == null )
			return;

	}

	[ServerCmd( "give_ammo" )]
	public static void GiveAmmo(string ammoTypeString, int amount)
	{
		var target = ConsoleSystem.Caller.Pawn;
		if ( target == null ) return;

		var player = target as MyPlayer;

		AmmoType ammoType;

		if (Enum.TryParse(ammoTypeString, out ammoType) && Enum.IsDefined( typeof( AmmoType ), ammoType ))
			player.GiveAmmo( ammoType, amount );
	}

	[ServerCmd( "drop_inventory_current" )]
	public static void DropCurrentWeapon()
	{
		var target = ConsoleSystem.Caller.Pawn;
		var inventory = target.Inventory;
		var dropped = inventory.DropActive();


		if ( dropped != null )
		{
			target.TakeDamage(DamageInfo.Generic(50f));
		}
	}


	public override void Simulate( Client cl )
	{
		base.Simulate( cl );

		if ( Input.ActiveChild != null )
		{
			ActiveChild = Input.ActiveChild;
		}

		if ( LifeState != LifeState.Alive )
			return;

		// Get players client
		KillCount = this.Client.GetInt("kills");

		var controller = GetActiveController();
		if ( controller != null )
			EnableSolidCollisions = !controller.HasTag( "noclip" );

		TickPlayerUse();
		SimulateActiveChild( cl, ActiveChild );

		if ( Input.Pressed( InputButton.Drop ) )
		{
			var dropped = Inventory.DropActive();
			if ( dropped != null )
			{
				dropped.PhysicsGroup.ApplyImpulse( Velocity + EyeRot.Forward * 500.0f + Vector3.Up * 100.0f, true );
				dropped.PhysicsGroup.ApplyAngularImpulse( Vector3.Random * 100.0f, true );

				timeSinceDropped = 0;
			}
		}

		if ( Input.Released( InputButton.Jump ) )
		{
			if ( timeSinceJumpReleased < 0.8f )
			{
				Game.Current?.DoPlayerNoclip( cl );
			}

			timeSinceJumpReleased = 0;
		}

		if ( Input.Left != 0 || Input.Forward != 0 )
		{
			timeSinceJumpReleased = 1;
		}
	}


}
