using System.Collections;
using UnityEngine;
using UnityEngine.UI;
namespace Playflock.Log.Node
{
    public class CustomButtonsNode : HUDNode
    {
        public static bool isDamage;
        public override void OnToggle(bool isOn)
        {
            isDamage = isOn;
        }
    }
}