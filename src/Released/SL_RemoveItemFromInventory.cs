﻿using SideLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SideLoader_ExtendedEffects.Released
{
    public class SL_RemoveItemFromInventory : SL_Effect, ICustomModel
    {
        public Type SLTemplateModel => typeof(SL_RemoveItemFromInventory);

        public Type GameModel => typeof(RemoveItemFromInventory);

        public int ItemID;
        public int ItemQuantity;

        public override void ApplyToComponent<T>(T component)
        {
            RemoveItemFromInventory comp = component as RemoveItemFromInventory;
            comp.ItemID = ItemID;
            comp.ItemQuantity = ItemQuantity;
        }

        public override void SerializeEffect<T>(T effect)
        {
            RemoveItemFromInventory comp = effect as RemoveItemFromInventory;
            this.ItemID = comp.ItemID;
            this.ItemQuantity = comp.ItemQuantity;
        }
    }

    public class RemoveItemFromInventory : Effect
    {
        public int ItemID;
        public int ItemQuantity;

        public override void ActivateLocally(Character _affectedCharacter, object[] _infos)
        {
            if (_affectedCharacter.IsAI || _affectedCharacter.Inventory == null)
            {
                return;
            }

            List<Item> ItemsFound = _affectedCharacter.Inventory.GetOwnedItems(ItemID);

            if (ItemsFound.Count > 0)
            {
                //if its a stackable item

                Item Item = ItemsFound[0];
                if (Item.IsStackable)
                {
                    if (Item.RemainingAmount >= ItemQuantity)
                    {
                        Item.RemoveQuantity(ItemQuantity);
                    }
                    else
                    {
                        //not enough in stack
                        ExtendedEffects.Instance.DebugLogMessage($"RemoveItemFromInventory :: Not enough items in stack");
                    }
              
                }
                //not a stackable item
                else
                {
                    if (ItemsFound.Count >= ItemQuantity)
                    {
                        _affectedCharacter.Inventory.RemoveItem(ItemID, ItemQuantity);
                    }
                }
               
            }
            else
            {
                ExtendedEffects.Instance.DebugLogMessage($"RemoveItemFromInventory :: No Items found with ID {ItemID}");
            }
        }
    }
}
