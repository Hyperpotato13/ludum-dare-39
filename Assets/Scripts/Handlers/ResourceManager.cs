using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager {
	public Dictionary<string, Sprite> sprites {get; protected set;}
	//public Sprite character {get; protected set;}

	public Sprite character 
	{
		get {
			return sprites[WorldHandler.handler.character.getMeta()];
		}
	}

	public ResourceManager()
	{
		sprites = new Dictionary<string, Sprite>();
		foreach (Sprite sprite in Resources.LoadAll<Sprite>("World/Tiles/")) {
			sprites.Add(sprite.name, sprite);
		}

		foreach (Sprite sprite in Resources.LoadAll<Sprite>("Character/")) {
			sprites.Add(sprite.name, sprite);
		}
	}
}


