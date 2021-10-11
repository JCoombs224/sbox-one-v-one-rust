using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;


class GameScoreHUD : Panel
{
	private Label CurrentScore;
	private Panel CurrentScoreBar;
	private Label EnemyScore;
	private Panel EnemyScoreBar;
	private Label CurrentPosition;

	public GameScoreHUD()
	{
		StyleSheet.Load( "/ui/GameScoreHUD.scss" );

		Panel currScoreBack = Add.Panel( "scoreBack current" );

		CurrentScore = currScoreBack.Add.Label( "0", "score curr" );
		Panel currentScoreBarBack = currScoreBack.Add.Panel( "scoreBarBack" );
		CurrentScoreBar = currentScoreBarBack.Add.Panel( "scoreBar curr" );
		currentScoreBarBack.Add.Panel( "scoreBarEnd" );

		Panel enemyScoreBack = Add.Panel( "scoreBack enemy" );

		EnemyScore = enemyScoreBack.Add.Label("0", "score" );
		Panel enemyScoreBarBack = enemyScoreBack.Add.Panel( "scoreBarBack" );
		EnemyScoreBar = enemyScoreBarBack.Add.Panel( "scoreBar enemy" );
		enemyScoreBarBack.Add.Panel( "scoreBarEnd" );

		Panel currPositionBack = Add.Panel( "currPosition" );
		CurrentPosition = currPositionBack.Add.Label( "Draw", "currPosition label" );
	}

	public override void Tick()
	{
		var player = Local.Pawn as MyPlayer;
		if ( player == null ) return;

		var scores = new GameScore();
		Client FirstPlace = scores.GetFirstPlacePlayer();
		Client SecondPlace = scores.GetSecondPlacePlayer();

		CurrentScore.Text = $"{player.KillCount}";
		CurrentScoreBar.Style.Dirty();
		CurrentScoreBar.Style.Width = Length.Percent( player.KillCount * 5 );

		EnemyScoreBar.Style.Dirty();

		if (FirstPlace == player.Client && SecondPlace != null )
		{
			CurrentPosition.Text = "Winning";
			CurrentPosition.SetClass( "losing", false );
			CurrentPosition.SetClass( "winning", true );
			EnemyScore.Text = $"{SecondPlace.GetInt("kills")}";
			EnemyScoreBar.Style.Width = Length.Percent( SecondPlace.GetInt( "kills" ) * 5 );
		}
		else if (SecondPlace != null)
		{
			CurrentPosition.Text = "Losing";
			CurrentPosition.SetClass( "winning", false );
			CurrentPosition.SetClass( "losing", true );
			EnemyScore.Text = $"{FirstPlace.GetInt( "kills" )}";
			EnemyScoreBar.Style.Width = Length.Percent( FirstPlace.GetInt( "kills" ) * 5 );
		}
	}
}
