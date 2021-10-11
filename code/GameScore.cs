using Sandbox;
using Sandbox.Hooks;
using Sandbox.UI;
using Sandbox.UI.Construct;
using System;
using System.Collections.Generic;
using System.Linq;

class GameScore
{
	Client FirstPlace = null, SecondPlace = null;
	int first, second;

	public GameScore()
	{
		updateTopPlayers();
	}

	public void updateTopPlayers()
	{
		int current;

		// Loop through all the players
		foreach ( var client in Client.All )
		{
			// Get current client kills
			current = client.GetInt( "kills" );

			if ( FirstPlace == null )
			{
				FirstPlace = client;
				first = client.GetInt( "kills" );
			}
			else if ( SecondPlace == null && FirstPlace != client )
			{
				SecondPlace = client;
				second = client.GetInt( "kills" );
			}
			else
			{
				current = client.GetInt( "kills" );
				if (current > first)
				{
					first = current;
					FirstPlace = client;
				}
				else if(current > second)
				{
					second = current;
					SecondPlace = client;
				}
			}
		}
	}

	// Getters
	public Client GetFirstPlacePlayer()
	{
		return FirstPlace;
	}

	public Client GetSecondPlacePlayer()
	{
		return SecondPlace;
	}

}
