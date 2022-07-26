using SideLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SideLoader_ExtendedEffects.Experimental
{
    //intended to be c# class for deserialization from XML
    //eg <SL_Item>
    /// <SLEX_OnEquipEffects> 
    /// <SL_Effect >
    /// 
    /// </SL_Effect>
    /// </SLEX_OnEquipEffects>
    public class SLEx_OnEquipEffects
    {
        public List<SL_Effect> OnEquipEffects;
        public List<SL_Effect> OnUnEquipEffects;
    }
}
