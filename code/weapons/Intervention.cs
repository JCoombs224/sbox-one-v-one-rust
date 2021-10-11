using Sandbox;
using System.Threading.Tasks;
using SWB_Base;

[Library( "intervention", Title = "Intervention" )]
partial class Intervention : WeaponBaseSniper
{
	public override string ViewModelPath => "weapons/intervention/models/weapons/v_snip_awp.vmdl";
	public override int Bucket => 5;
	public override HoldType HoldType => HoldType.Rifle;
	public override string WorldModelPath => "weapons/swb/css/awp/css_w_awp.vmdl";
	public override string Icon => "/swb_css/textures/ui/css_icon_awp.png";
	public override int FOV => 70;
	public override int ZoomFOV => 75;
	public override float WalkAnimationSpeedMod => 0.8f;
	public override bool DrawCrosshair => true;
	public override bool DrawCrosshairDot => false;
	public override float AimSensitivity => 0.3f;

	public override string LensTexture => "/materials/swb/scopes/swb_lens_hunter.png";
	public override string ScopeTexture => "/materials/swb/scopes/swb_scope_hunter.png";
	public override string ZoomInSound => "";
	public override float ZoomAmount => 20f;

	public Intervention()
	{
		Primary = new ClipInfo
		{
			Ammo = 5,
			AmmoType = AmmoType.Sniper,
			ClipSize = 5,
			ReloadTime = 3.67f,

			BulletSize = 5f,
			Damage = 100f,
			Force = 7f,
			Spread = 0.25f,
			Recoil = 2f,
			RPM = 50,
			FiringType = FiringType.semi,
			ScreenShake = new ScreenShake
			{
				Length = 0.5f,
				Speed = 4.0f,
				Size = 0.5f,
				Rotation = 0.5f
			},

			//DryFireSound = "",
			ShootSound = "intervention_fire1",

			BulletEjectParticle = "particles/pistol_ejectbrass.vpcf",
			MuzzleFlashParticle = "particles/pistol_muzzleflash.vpcf",
		};

		ZoomAnimData = new AngPos
		{
			Angle = new Angles( 0f, 0.5f, -2f ),
			Pos = new Vector3( -5.5f, 4f, -2f )
		};

		RunAnimData = new AngPos
		{
			Angle = new Angles( 10, 40, 0 ),
			Pos = new Vector3( 5, 0, 0 )
		};
	}

	public override void StartReloadEffects( bool isEmpty )
	{
		_ = ReloadSoundDelay();
		base.StartReloadEffects( isEmpty );
	}

	public override void AttackPrimary()
	{
		base.AttackPrimary();
		PlaySound( "intervention_fire1" );
	}


	// Using a task to delay the reload sounds since modeldoc sound events
	// do not work yet.
	private async Task ReloadSoundDelay()
	{
		await Task.DelaySeconds( 1.55f );
		PlaySound( "magout" );

		await Task.DelaySeconds( 1f );
		PlaySound( "magin" );
	}
}
