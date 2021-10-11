using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;
using SWB_Base;

public partial class HUD : HudEntity<RootPanel>
{
	public HUD()
	{
		if ( !IsClient ) return;

		RootPanel.StyleSheet.Load( "/UI/Inventory.scss" );

		RootPanel.AddChild<NameTags>();
		RootPanel.AddChild<Scoreboard<ScoreboardEntry>>();
		RootPanel.AddChild<ChatBox>();
		RootPanel.AddChild<VoiceList>();
		RootPanel.AddChild<KillFeed>();
		//RootPanel.AddChild<Vitals>();
		RootPanel.AddChild<InventoryBar>();
		RootPanel.AddChild<Crosshair>();
		RootPanel.AddChild<Ammo>();
		RootPanel.AddChild<GameScoreHUD>();
	}
}
