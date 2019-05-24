using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "Data/Character")]
public class Character : ScriptableObject 
{
	public float runSpeed;
	public float walkSpeed;
	public float crouchSpeed;
	public float sprintSpeed;
	public float jumpSpeed;
	public float gravity;
}
