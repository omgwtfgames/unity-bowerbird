using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AStar;

// Based on: http://www.codeproject.com/Articles/118015/Fast-A-Star-2D-Implementation-for-C

public class AStarExample : MonoBehaviour {
	public int Width = 64;
	public int  Height = 64;
	
	public class MyPathNode : IPathNode<Object> {
    	public int X { get; set; }
    	public int Y { get; set; }
    	public bool IsWall {get; set;}
	
		// the argument passed in here ('unused') can be setup as any type -
		// you can use it to provide custom 'walkable' behaviour, depending
		// on the state of the object passed
    	public bool IsWalkable(Object unused) {
        	return !IsWall;
    	}
	}
	
	void Awake() {
		// setup grid with walls
		MyPathNode[,] grid = new MyPathNode[Width, Height];
		for (int x = 0; x < Width; x++) {
    		for (int y = 0; y < Height; y++) {
        		bool isWall = ((y % 2) != 0) && (Random.Range(0, 10) != 8);
		        grid[x, y] = new MyPathNode() {
            		IsWall = isWall,
            		X = x,
            		Y = y,
        		};
    		}
		}
		
		// second generic parameter should be the argument type for IsWalkable (I'm using GameObject)
		SpatialAStar<MyPathNode, Object> aStar = new SpatialAStar<MyPathNode, Object>(grid);
		
		// start point, desired end point, 
		// the third parameter is the value passed to IsWalkable (eg, the GameObject of the NPC doing the walking)
		// if not path is found, null is returned
		LinkedList<MyPathNode> path = aStar.Search(new Point(0, 0), 
				new Point(Width - 2, Height - 2), null);
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
