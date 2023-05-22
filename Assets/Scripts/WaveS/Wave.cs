using UnityEngine;

[System.Serializable]
public class Groups
{
    public Enemy enemy;
    public int count;
    public float rate;
    public float delay;
}
[CreateAssetMenu(fileName = "New Wave", menuName = "TowerDefence/Wave")]
public class Wave : ScriptableObject
{
    public Groups[] groups;
    // public float rate;
}

