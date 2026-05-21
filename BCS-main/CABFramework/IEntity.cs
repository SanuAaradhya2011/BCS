/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 	 								|
 * | 																												|
 * |											Author : Piyush Singh. 	 												|
 * | 																												|
 * |----------------------------------------------------------------------------------------------------------------| 
 */

using System;
using System.Collections.Generic; 
using System.Text;

namespace CAB.Framework.Entity
{
    /// <summary>
    /// Base entity class.
    /// </summary>
    public interface IEntity
    {
        /// <summary>
        /// For Insert Only.
        /// </summary>
         bool New { get; set; }
        /// <summary>
        /// For Insert & Update Only.
        /// </summary>
         bool Dirty { get; set; }
        /// <summary>
        /// For Delete only.
        /// </summary>
         bool Deleted { get; set; }
    }
}
