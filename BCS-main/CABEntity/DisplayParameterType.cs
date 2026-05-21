#region NameSpaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using CAB.Framework.Entity;
#endregion
namespace CAB.Entity 
{
    /// <summary>
    /// Describes type of display parameters .
    /// </summary>
    public enum DisplayParameterType
    {             
        [DescriptionAttribute("Push Mode")]
        PushMode=1,
        [DescriptionAttribute("Scroll Mode")]
        ScrollMode=2,
        [DescriptionAttribute("High Resolution Mode")]
        HighResolutionMode=3,
        [DescriptionAttribute("Display Timeouts")]
        DisplayTimeouts=4
   
    }
    /// <summary>
    /// Display parameter entity 
    /// </summary>
    public class DisplayParamatersDBEntity : EntityBase
    {
        public DisplayParameterType displayParamaterType;
        public string paramaterName;
        public int paramaterValue;

    }
}
