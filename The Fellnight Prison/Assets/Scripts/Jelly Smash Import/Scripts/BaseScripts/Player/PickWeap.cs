/*
Swapping this out for an SQL database later.  That will be easier to maintain.
*/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public class PickWeap : MonoBehaviour {
private static float attarc, attrng, damage, cooldown;
private static List<float> stats = new List<float>{0f, 0f, 0f, 0f};
	
	public static List<float> pickweap(string currweap){
		switch(currweap){
			case "bow":
				attarc = .15f;
				attrng = 10f;
				damage = 5;
				cooldown = 2;
				stats[0] = attarc;
				stats[1] = attrng;
				stats[2] = damage;
				stats[3] = cooldown;
				return(stats);
			default:
				attarc = 1;
				attrng = 2.25f;
				damage = 0;
				cooldown = 1;
				stats[0] = attarc;
				stats[1] = attrng;
				stats[2] = damage;
				stats[3] = cooldown;
				return(stats);
			}
	}
}