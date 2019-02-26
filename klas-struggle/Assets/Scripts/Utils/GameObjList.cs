using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Utils
{
    // Workaround for inability to serialize nested generics: https://answers.unity.com/questions/289692/serialize-nested-lists.html
    [Serializable]
    public class ListWrapper<T>
    {
        public List<T> List;

        public T this[int i]
        {
            get { return List[i]; }
            set { List[i] = value; }
        }

        public int Count => List.Count;
    }

    [Serializable]
    class GameObjList : ListWrapper<GameObject>{}
}
