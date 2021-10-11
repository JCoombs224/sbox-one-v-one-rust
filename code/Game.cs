using Sandbox;

public partial class MyGame : Sandbox.Game
{
	public HUD MyHUD;

	public MyGame()
	{
		if ( IsClient ) MyHUD = new HUD();
	}

	// On game hotload run this method
	[Event.Hotload]
	public void HotLoadUpdate()
	{
		if ( !IsClient || MyHUD == null ) return;
		MyHUD?.Delete();
		MyHUD = new HUD();
	}

	public override void ClientJoined( Client client )
	{
		base.ClientJoined( client );

		// Create a pawn and assign it to the client.
		var player = new MyPlayer(client);
		client.Pawn = player;
		player.Respawn();
	}
}
