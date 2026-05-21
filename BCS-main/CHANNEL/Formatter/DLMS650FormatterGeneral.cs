/* 
 * |---------------------------------------------------------------------------------------------------------------|
 * |All rights reserved to Cabcon Technologies                                                                             |
 * |                                                                                                               |
 * |Author : Piyush Singh.                                                                               |
 * |                                                                                                               |
 * |                                                                                                               |
 * |---------------------------------------------------------------------------------------------------------------| */

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using CAB.Entity;
using CAB.BLL;
using CAB.Framework.Utility;

namespace CAB.Channel.Formatter
{
    public class DLMS650FormatterGeneral:ReadBase
    {
        private DLMS650FormatterCommon common;
        private DLMS650StructureInfoBLL structureInfoBLL;
        private DLMS650StructureUnitInfoBLL structureUnitInfoBLL;
        private bool showModelNo = false;
        public DLMS650FormatterGeneral()
        {
            showModelNo = true;
            structureInfoBLL = new DLMS650StructureInfoBLL();
            structureUnitInfoBLL = new DLMS650StructureUnitInfoBLL();
            common = new DLMS650FormatterCommon(); 
        }
        public DLMS650FormatterGeneral(bool isAPI)
        {
            structureInfoBLL = new DLMS650StructureInfoBLL();
            structureUnitInfoBLL = new DLMS650StructureUnitInfoBLL();
            common = new DLMS650FormatterCommon();
            showModelNo = true;
        }
        public void LoadGeneralData(string[] data, BillingGeneralNFDLMSEntity master)
        {          
            int counter;
            bool flag = false;
            for (  counter = 0; counter < data.Length; counter++)
            {
                if (string.IsNullOrEmpty(data[counter]))
                {
                    flag=false;
                    break;
                }
                else
                    flag = true;
            }
            if (!flag)
            {
                this.StatusMessage = "General data not found.";
                Application.DoEvents();
                return;
            }
            this.StatusMessage = "Uploading general data......";
            Application.DoEvents();
            DLMS650NamePlateDetailsEntity entity = new DLMS650NamePlateDetailsEntity();
            int meterModelNo = new DLMS650GeneralBLL().GetMeterModelNoByMeterDataID(ConfigInfo.ActiveMeterDataId);
            if (meterModelNo == 10)
            {
                entity.MeterDataType = "HTCT";
            }
            else
            {
                entity.MeterDataType = "LTCT";
            }

            for (  counter = 0; counter < data.Length; counter++)
            {
                if (string.IsNullOrEmpty(data[counter]))
                    continue;
                if (counter == 0)
                    entity.MeterSerialNumber = common.ConvertHexToString(data[counter].Substring(0, (data[counter].Length - 1)));
                else if (counter == 1)
                {
                    if (data[counter].IndexOf('\r') > 0)
                        entity.Manufacturername = common.ConvertHexToString(data[counter].Substring(0, data[counter].IndexOf('\r')));
                    else
                        entity.Manufacturername = common.ConvertHexToString(data[counter]);
                }
                else if (counter == 2)
                {
                    if (data[counter].IndexOf('\r') > 0)
                        entity.FirmwareVersionformeter = common.ConvertHexToString(data[counter].Substring(0, data[counter].IndexOf('\r')));
                    else
                        entity.FirmwareVersionformeter = common.ConvertHexToString(data[counter]);
                }
                else if (counter == 3)
                {
                    if (data[counter].IndexOf('\r') > 0)
                        entity.Metertype = GetMeterType(common.ConvertHexToDecimal(data[counter].Substring(0, data[counter].IndexOf('\r'))));
                    else
                        entity.Metertype = GetMeterType(common.ConvertHexToDecimal(data[counter]));
                }
                else if (counter == 4)
                {
                    if (data[counter].IndexOf('\r') > 0)
                        entity.InternalCTratio = common.ConvertHexToDecimal(data[counter].Substring(0, data[counter].IndexOf('\r'))).ToString();
                    else
                        entity.InternalCTratio = common.ConvertHexToDecimal(data[counter]).ToString();
                }
                else if (counter == 5)
                {
                    if (data[counter].IndexOf('\r') > 0)
                        entity.Meteryearofmanufacture = common.ConvertHexToDecimal(data[counter].Substring(0, data[counter].IndexOf('\r'))).ToString();
                    else
                        entity.Meteryearofmanufacture = common.ConvertHexToDecimal(data[counter]).ToString();
                }
                else if (counter == 6)
                {
                    if (data[counter].IndexOf('\r') > 0)
                        entity.InternalPTratio= common.ConvertHexToDecimal(data[counter].Substring(0, data[counter].IndexOf('\r'))).ToString();
                    else
                        entity.InternalPTratio = common.ConvertHexToDecimal(data[counter]).ToString();
                    if(entity.InternalPTratio=="0")
                        entity.InternalPTratio = "---";
                }
                else if (counter == 7)
                {
                    //
                    //Console.WriteLine(showModelNo + " = Model NO");
                    if (showModelNo)
                    {
                        if (data[counter].IndexOf('\r') > 0)
                            entity.MeterModelNo = common.ConvertHexToDecimal(data[counter].Substring(0, data[counter].IndexOf('\r'))).ToString();
                        else
                            entity.MeterModelNo = common.ConvertHexToDecimal(data[counter]).ToString();
                        if (entity.MeterModelNo == "0")
                            entity.MeterModelNo = "---";
                    }
                }

            }
            entity.EnergyResolution = DLMS650FormatterCommon.EnergyResolution.ToString();
            master.General = entity;
        }
        private string GetMeterType(int typ)
        {
            if(typ==0)
                return "3P-3W";
            else
                return "3P-4W";
        }
      
    }
}
