using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Character")]
public class CharacterDescriptionSO : ScriptableObject
{
    public string Name;
    public string Description;
    public int CrouchSpeed;
    public int WalkSpeed;
    public int RunSpeed;
    public int WallrunSpeed;
}
