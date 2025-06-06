#nullable enable

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Damage : MonoBehaviour, Triggerable
{
    [SerializeField] private List<string> tags = new();
    [SerializeField] private int damage;

    private HashSet<string> tagSet = new();

    void Start()
    {
        Init();
    }

    protected virtual void Init()
    {
        tagSet = tags.ToHashSet();
    }

    public virtual bool ShouldTrigger(GameObject gameObject)
    {
        return true;
    }

    public void Trigger(GameObject gameObject)
    {
        if (tagSet.Contains(gameObject.tag))
        {
            Health? playerHealth = gameObject.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.Damage(damage);
            }
        }
    }
}