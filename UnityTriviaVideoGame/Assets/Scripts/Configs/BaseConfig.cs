using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class BaseConfig : ScriptableObject // I used the BaseConfig from SpaceShmup as a starting point. If you think of anything that every config should have, pleast add it here.
{
   //[field: SerializeField] public GameObject prefab { private set; get; }

   //protected virtual void OnValidate()
   //{
      //Assert.IsNotNull(prefab, $"{nameof(prefab)} cannot be null in {name}");
   //}
}
