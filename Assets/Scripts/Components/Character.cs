using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum GenericDirection {None=0, Left, Right, Up, Down};

public class Character
{
	WorldTile location;
	WorldTile destination;
	GenericDirection direction;
	GenericDirection facing;
	float progress;
	float inertia;

	Action onLocationUpdated;
	Action onDirectionUpdated;

	public float x 
	{
		get {
			if(destination == null || location == destination)
			return location.x;

			else {
				return Mathf.Lerp(location.x, destination.x, progress);
			}
		}
	}

	public float y
	{
		get {
			if(destination == null || location == destination)
			return location.y;

			else {
				return Mathf.Lerp(location.y, destination.y, progress);
			}
		}
	}

	public Character(int x, int y)
	{
		location = WorldHandler.handler.world.getTileAt(x,y);
		location.Type = GenericTileType.None;
		destination = location;
		progress = 0f;
		inertia = 0f;
		direction = GenericDirection.None;
		facing = GenericDirection.Up;
		location.world.getTileAt((int)x+3,(int)y).Type = GenericTileType.CentralComputer;
	}

	public void registerLocationUpdatedCallback(Action callback)
	{
		onLocationUpdated += callback;
	}

	public void unregisterLocationUpdatedCallback(Action callback)
	{
		onLocationUpdated -= callback;
	}

	public void registerDirectionUpdatedCallback(Action callback)
	{
		onDirectionUpdated += callback;
	}

	public void unregisterDirectionUpdatedCallback(Action callback)
	{
		onDirectionUpdated -= callback;
	}

	float generic_inertia = 1f;
	public void update(float deltaTime)
	{
		if(inertia<=0f)
		{
			if(Input.GetKeyDown(KeyCode.A)) {
				direction = GenericDirection.Left;
				inertia = generic_inertia;
			}

			else if(Input.GetKeyDown(KeyCode.D)) {
				direction = GenericDirection.Right;
				inertia = generic_inertia;
			}

			else if(Input.GetKeyDown(KeyCode.W)) {
				direction = GenericDirection.Up;
				inertia = generic_inertia;
			}

			else if(Input.GetKeyDown(KeyCode.S)) {
				direction = GenericDirection.Down;
				inertia = generic_inertia;
			}
		}

		if(inertia>0f)
		if(location == destination && location.Type != GenericTileType.CentralComputer) {
			int x = Mathf.RoundToInt(this.x);
			int y = Mathf.RoundToInt(this.y);

			switch(direction)
			{
				case GenericDirection.Down:
				{
					if(y>0) destination = location.world.getTileAt(x,y-1);
				}break;

				case GenericDirection.Up:
				{
					if(y+1<location.world.height) destination = location.world.getTileAt(x,y+1);
				}break;

				case GenericDirection.Left:
				{
					if(x>0) destination = location.world.getTileAt(x-1,y);
				}break;

				case GenericDirection.Right:
				{
					if(x+1<location.world.width) destination = location.world.getTileAt(x+1,y);
				}break;
			}

			if(destination.Type == GenericTileType.Solid)
			{
				destination = location;
				inertia = 0f;
			}

			else {
				if(direction != GenericDirection.None)
				facing = direction;
				onDirectionUpdated();
			}
		}

		if(location != destination && location.Type != GenericTileType.CentralComputer)
		{
			Camera.main.transform.position = new Vector3(x, y, -10);
			progress += Mathf.Lerp(1f, 3.5f, inertia ) * deltaTime * Mathf.Lerp(1f, 3f, Camera.main.orthographicSize/15f)/1.75f;

			if(inertia-deltaTime/2f >= 0f)
				inertia -= deltaTime/5f;
			
			onLocationUpdated();
		}

		else {
			if(location.Type == GenericTileType.CentralComputer) {
				//win;
				inertia = 0f;
				direction = GenericDirection.None;
				facing = GenericDirection.Up;
				onDirectionUpdated();
				WorldHandler.handler.winGame();
			}
		}

		if(progress>=1f)
		{
			location = destination; progress = 0f;
		}
	}

	public string getMeta()
	{
		return "Character_" +facing.ToString();
	}
}
