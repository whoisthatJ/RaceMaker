using UnityEngine;
using UnityEngine.UI;
using System.Collections;
namespace Playflock.Log.Widget
{
    public class DescriptionWidget : HUDWidget
    {
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
