using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldHandler : MonoBehaviour {

	public World world {get; protected set;}
	Dictionary<WorldTile, GameObject> tileMap;
	GameObject characterGO;
	ResourceManager resourceManager;
	public static WorldHandler handler;
	public Character character {get; protected set;}
	public GameObject LightGO;
	bool win = false;

	// Use this for initialization
	void Start () {
		handler = this;
		resourceManager = new ResourceManager();
		tileMap = new Dictionary<WorldTile, GameObject>();
		world = new World(50,50);
		registerCharacter();
		Camera.main.transform.position = new Vector3(character.x, character.y, -10);
	}
	
	// Update is called once per frame
	void Update () {
		if(win)
		{
			if(LightGO.GetComponent<Light>().range<100f)
			LightGO.GetComponent<Light>().range += 1f;

			if(LightGO.GetComponent<Light>().range>20f)
			{
				characterGO.SetActive(false);
			}

			return;
		}

		character.update(Time.deltaTime);
	}

	public void registerTile(WorldTile tile)
	{
		GameObject GO = new GameObject();
		GO.name = "Tile at " +tile.x+ " and " +tile.y;
		GO.transform.position = new Vector3(tile.x, tile.y, 0f);
		GO.transform.SetParent(this.transform);
		tileMap.Add(tile, GO);
		GO.AddComponent<SpriteRenderer>().sprite = resourceManager.sprites[tile.getMeta()];
		tile.registerTypeChangeCallback(onTileTypeChanged);
	}

	public void registerCharacter()
	{
		character = new Character(world.width/2, world.height/2);
		characterGO = new GameObject();
		characterGO.name = "Character";
		characterGO.transform.position = new Vector3(character.x, character.y, 0f);
		characterGO.AddComponent<SpriteRenderer>().sprite = resourceManager.character;
		character.registerLocationUpdatedCallback(onCharacterPositionUpdated);
		character.registerDirectionUpdatedCallback(onCharacterDirectionUpdated);
		characterGO.GetComponent<SpriteRenderer>().sortingLayerName = "Character";
	}

	public void onTileTypeChanged(WorldTile tile)
	{
		tileMap[tile].GetComponent<SpriteRenderer>().sprite = resourceManager.sprites[tile.getMeta()];
	}

	public void onCharacterPositionUpdated()
	{
		characterGO.transform.position = new Vector3(character.x, character.y, 0f);
	}

	public void onCharacterDirectionUpdated()
	{
		characterGO.GetComponent<SpriteRenderer>().sprite = resourceManager.character;
	}

	public void winGame(){
		LightGO.transform.position = new Vector3(character.x+0.5f, character.y+0.5f, 15);
		win = true;
	}
}
