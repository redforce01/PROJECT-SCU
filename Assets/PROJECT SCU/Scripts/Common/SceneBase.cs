using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SCU
{
    public abstract class SceneBase : MonoBehaviour
    {
        public virtual IEnumerator OnStartScene()
        {
            yield return null;
        }

        public virtual IEnumerator OnEndScene()
        {
            yield return null;
        }
    }
}
