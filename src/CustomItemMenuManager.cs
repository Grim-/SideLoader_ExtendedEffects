using SideLoader_ExtendedEffects.Item_Context_Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SideLoader_ExtendedEffects
{
    public class CustomItemMenuManager
    {
        public Dictionary<int, CustomItemDisplayMenuOption> CustomItemOptions { get; private set; }

        public CustomItemMenuManager()
        {
            //ExtendedEffects.Instance?.Log($"CustomItemManager Initializing..");
            CustomItemOptions = new Dictionary<int, CustomItemDisplayMenuOption>();
        }
        /// <summary>
        /// Creates a new custom Item Menu Option that is added to right click menu of the inventory.
        /// </summary>
        /// <param name="NewActionID">YourNewID</param>
        /// <param name="ActionString">The string that is displayed for the option</param>
        /// <param name="OnCustomActionPressed">What should happen on action press?</param>
        /// <param name="ShouldAddAction">When should this action be added? If null it is always added.</param>
        public void RegisterCustomMenuOption(int NewActionID, string ActionString, Action<Character, Item, ItemDisplayOptionPanel, int> OnCustomActionPressed, Func<Character, Item, ItemDisplayOptionPanel, int, bool> ShouldAddAction)
        {
            ExtendedEffects.Instance?.DebugLogMessage($"Registering custom action with ID {NewActionID} String {ActionString}");
            if (!CustomItemOptions.ContainsKey(NewActionID))
            {
                CustomItemOptions.Add(NewActionID, new CustomItemDisplayMenuOption(NewActionID, ActionString, OnCustomActionPressed, ShouldAddAction));
            }
            else
            {
                ExtendedEffects.Instance?.DebugLogMessage($"Custom Action already exists with ID {NewActionID}");
            }
        }

        public void UnRegisterCustomMenuOption(int CustomActionID)
        {
            if (CustomItemOptions.ContainsKey(CustomActionID))
            {
                ExtendedEffects.Instance?.DebugLogMessage($"UnRegistering custom action ID {CustomActionID}");
                CustomItemOptions.Remove(CustomActionID);
            }
        }
    }
}
