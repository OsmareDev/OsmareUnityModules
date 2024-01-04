using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICollectible
{
    public void GetCollected(CollectibleCatcherController ccatcher = null);
}
