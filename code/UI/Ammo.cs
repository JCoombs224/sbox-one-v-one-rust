using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;
using SWB_Base;

public partial class Ammo : Panel
{

	private Panel AmmoBack;
	private Label AmmoClip;
	private Label AmmoLeft;

	public Ammo()
	{
		StyleSheet.Load( "/ui/Ammo.scss" );

		Panel ammoBack = Add.Panel( "Ammo" );
		AmmoBack = ammoBack.Add.Panel( "ammoBack" );

		AmmoClip = AmmoBack.Add.Label( "10", "ammoLeft" );
		AmmoClip.Add.Label( "/" );
		AmmoLeft = AmmoBack.Add.Label( "100", "ammoClip" );

	}

	public override void Tick()
	{
		var player = Local.Pawn;
		if ( player == null ) return;

		if ( player.ActiveChild == null )
			return;

		var weapon = player.ActiveChild as WeaponBase;

		SetClass( "active", weapon != null );

		var inv = weapon.AvailableAmmo();

		if ( weapon == null ) return;

		AmmoClip.Text = $"{weapon.Primary.Ammo}";
		AmmoLeft.Text = $"{inv}";

		AmmoClip.SetClass( "danger", weapon.Primary.Ammo < (0.33 * weapon.Primary.ClipSize) );

		//HealthBar.Style.Dirty();
		//HealthBar.Style.Width = Length.Percent( player.Health );

		base.Tick();
	}
}
