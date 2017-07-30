using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum GenericTileType {None, Solid, Floor, CentralComputer};

public class WorldTile {
	public World world;
	public int x {get; protected set;} 
	public int y {get; protected set;}
	public float oxygen;

	GenericTileType type;
	public GenericTileType Type {
		get {
			return type;
		}
		set {
			type = value;
			onTypeChanged(this);
		}
	}

	Action<WorldTile> onTypeChanged;

	public WorldTile(World world, int x, int y, GenericTileType type){
		this.world = world;
		this.x = x;
		this.y = y;
		this.type = type;
		oxygen = UnityEngine.Random.Range(0.1f, 0.21f) + UnityEngine.Random.Range(0f,1f)*UnityEngine.Random.Range(0f, 0.1f);
	}

	public void registerTypeChangeCallback(Action<WorldTile> callback)
	{
		onTypeChanged += callback;
	}

	public void unregisterTypeChangeCallback(Action<WorldTile> callback)
	{
		onTypeChanged -= callback;
	}

	public string getMeta()
	{
		return type.ToString();
	}

	public List<WorldTile> getNeighbors(bool horisontal = true, bool diagonal=false)
	{
		if(x<0 || y<0 || x>world.width || y>world.height)
		{
			Debug.LogError("[Neighbors]"+x+"&"+y+"out of bounds!");
			return new List<WorldTile>();
		}

		List<WorldTile> list = new List<WorldTile>();
		if(horisontal)
		{
			if(x>0) list.Add(world.getTileAt(x-1,y));
			if(x<world.width-1) list.Add(world.getTileAt(x+1,y));
			if(y>0)list.Add(world.getTileAt(x,y-1));
			if(y<world.height-1)list.Add(world.getTileAt(x,y+1));
		}

		if(diagonal == false) return list;
		if(x>0 && y>0)list.Add(world.getTileAt(x-1,y-1));
		if(x<world.width-1 && y<world.height-1)list.Add(world.getTileAt(x+1,y+1));
		if(y>0 && x<world.width-1) list.Add(world.getTileAt(x+1,y-1));
		if(x>0 && y<world.height-1) list.Add(world.getTileAt(x-1,y+1));

		return list;
	}

	public void claim()
	{
		world.claimArea(this);
	}

	public void depeteOxygen(float amount)
	{
		oxygen -= amount;
		if(oxygen<0f) oxygen = 0f;
	}

	public void insertOxygen(float amount)
	{
		oxygen += amount;
		if(oxygen>1f) oxygen = 1f;
	}
}
