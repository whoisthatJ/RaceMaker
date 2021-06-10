using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Playflock.Log.Widget
{
    public class DisplayVariablesWidget : HUDWidget
    {
        public GameObject contentPanel;
        public GameObject variable;
        public GameObject vector;
        public override void Initialize()
        {
        }

        public override void Draw()
        {
        }

        public void DestroyWidget()
        {
            Destroy(gameObject);
        }
    }
}