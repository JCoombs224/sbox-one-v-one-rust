using Sandbox;
using System;
using System.Linq;
using SWB_Base;

partial class GameInventory : BaseInventory
{
	public GameInventory( Player player ) : base( player )
	{

	}

	public virtual void Clear()
	{
		for(int i = 0; i < this.Count(); i++)
		{
			this.GetSlot( i ).Delete();
		}
	}

	public override bool CanAdd( Entity entity )
	{
		if ( !entity.IsValid() )
			return false;

		if ( !base.CanAdd( entity ) )
			return false;

		return !IsCarryingType( entity.GetType() );
	}

	public override bool Add( Entity entity, bool makeActive = true )
	{
		var player = Owner as MyPlayer;

		if ( !entity.IsValid() )
			return false;

		if ( IsCarryingType( entity.GetType() ) )
			return false;

		var weapon = entity as WeaponBase;

		if(weapon != null)
		{
			weapon.Primary.Ammo = weapon.Primary.ClipSize;
		}

		return base.Add( entity, makeActive );
	}

	public bool IsCarryingType( Type t )
	{
		return List.Any( x => x?.GetType() == t );
	}

	public override bool Drop( Entity ent )
	{
		if ( !Host.IsServer )
			return false;

		if ( !Contains( ent ) )
			return false;

		ent.OnCarryDrop( Owner );

		return ent.Parent == null;
	}
}
