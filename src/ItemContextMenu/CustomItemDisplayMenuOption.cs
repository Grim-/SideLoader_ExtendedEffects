using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SideLoader_ExtendedEffects.Item_Context_Menu
{
    public class CustomItemDisplayMenuOption
    {
        public int CustomEquipActionID = 10000;
        public string ActionString = "Activate ";
        //This Action delegate will be called when your custom action is pressed
        public Action<Character, Item, ItemDisplayOptionPanel, int> OnCustomActionPressed;
        //This Func delegate returns true or false depending on wether the action should be added to the list for currently right clicked item.
        public Func<Character, Item, ItemDisplayOptionPanel, int, bool> ShouldAddActionDelegate;

        public CustomItemDisplayMenuOption(int customEquipActionID, string equip_String, Action<Character, Item, ItemDisplayOptionPanel, int> onCustomActionPressed, Func<Character, Item, ItemDisplayOptionPanel, int, bool> shouldAddActionDelegate)
        {
            CustomEquipActionID = customEquipActionID;
            ActionString = equip_String;
            OnCustomActionPressed = onCustomActionPressed;
            ShouldAddActionDelegate = shouldAddActionDelegate;
        }
    }
}
