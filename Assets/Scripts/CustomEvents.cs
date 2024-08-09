using System;
using UnityEngine;
using UnityEngine.Events;


//Separamos os nossos eventos personalizados em um �nico arquivo para facilitar a manuten��o

[Serializable]
public class NPCVendorEvent : UnityEvent<SO_Inventory> { }
[Serializable]
public class FloatEvent : UnityEvent<float> { }

[Serializable]
public class IntEvent : UnityEvent<int> { }

[Serializable]
public class BoolEvent : UnityEvent<bool> { }

[Serializable]
public class StringEvent : UnityEvent<string> { }

[Serializable]
public class GameObjectEvent : UnityEvent<GameObject> { }

[Serializable]
public class InputEvent : UnityEvent<float, float> { }
