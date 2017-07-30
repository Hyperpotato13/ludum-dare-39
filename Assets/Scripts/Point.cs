using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point<T> {
	public T x, y;
	public Point(T x, T y)
	{
		this.x = x;
		this.y = y;
	}
}
