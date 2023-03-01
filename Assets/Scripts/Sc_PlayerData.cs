using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Sc_PlayerData
{
	private float max_health = 100f;
	private float min_health = 0f;
	private float curr_health = 50f;

	private int score = 0;
	private int brain_count = 0;

	private float playerPosX, playerPosY;

	public int getScore(){ return this.score; }
	public void increaseScore(){ this.score++; }
	public int getBrainCount(){ return this.brain_count; }
	public void increaseBrainCount(){ this.brain_count++; }

	public float getMinHealth(){ return this.min_health; }
	public float getMaxHealth(){ return this.max_health; }
	public float getCurrHealth(){ return this.curr_health; }

	public void setHealth(float HealthAmount)
	{
		if(HealthAmount > this.max_health)
		{
			this.curr_health = this.max_health;
		}else
		{
			this.curr_health = HealthAmount;
		}
	}

	/*public void setHealth(float HealthAmount) {
		 // Make sure HealthAmount is non-negative
		if (HealthAmount < 0) 
		{
			HealthAmount = 0;
		}
		
		// Add HealthAmount to current health, but cap it at max_health
		this.curr_health = Mathf.Min(this.curr_health + HealthAmount, this.max_health);
	}*/
}
