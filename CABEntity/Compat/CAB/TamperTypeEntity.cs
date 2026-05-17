/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 	 								|
 * | 																												|
 * |											Author : Piyush Singh. 					                |
 * | 																												|
 * |----------------------------------------------------------------------------------------------------------------| */

using System;
using LNG.Framework.Entity;
using System.ComponentModel;


namespace LNG.Entity
{
    public class TamperTypeEntity : EntityBase
    {
        private int tamperTypeID;
        private string tamperType;
        public int TamperTypeID
        {
            get
            {
                return tamperTypeID;
            }
            set
            {
                tamperTypeID = value;
            }
        }
        public string TamperType
        {
            get
            {
                return tamperType;
            }
            set
            {
                tamperType = value;
            }
        }

        // Added to get the tamper types.
        public enum GetTamperTypes
        {
            [DescriptionAttribute("A")]
            CTByPass,
            [DescriptionAttribute("B")]
            RPhaseMissingPotential,
            [DescriptionAttribute("C")]
            YPhaseMissingPotential,
            [DescriptionAttribute("D")]
            BPhaseMissingPotential,
            [DescriptionAttribute("E")]
            FrontCover,
            // Remove terminal cover for bug 75298.
            //[DescriptionAttribute("TMCO")]
            //TerminalCover,
            [DescriptionAttribute("F")]
            Magnet,
            [DescriptionAttribute("G")]
            VoltImbalance,
            [DescriptionAttribute("H")]
            CurrentImbalance,
            [DescriptionAttribute("I")]
            NeutralDisturbance,
            [DescriptionAttribute("J")]
            RPhaseCTReversal,
            [DescriptionAttribute("K")]
            YPhaseCTReversal,
            [DescriptionAttribute("L")]
            BPhaseCTReversal,
            [DescriptionAttribute("M")]
            RPhaseCTOpen,
            [DescriptionAttribute("N")]
            YPhaseCTOpen,
            [DescriptionAttribute("O")]
            BPhaseCTOpen,
            [DescriptionAttribute("P")]
            LowPF,
            [DescriptionAttribute("Q")]
            OverCurrent,
            [DescriptionAttribute("R")]
            UnderVoltage,
            [DescriptionAttribute("S")]
            OverVoltage,
            //New Tamper Added
            [DescriptionAttribute("T")]
            TwoPN,
            [DescriptionAttribute("U")]
            InvalidPhaseVolatge,
            [DescriptionAttribute("V")]
            InvalidVolatge,
            [DescriptionAttribute("W")]
            OverLoad

        }

    }
}




