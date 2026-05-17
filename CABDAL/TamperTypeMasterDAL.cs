/* 
 * |----------------------------------------------------------------------------------------------------------------|
 * |											All rights reserved to Cabcon Technologies 	 								|
 * | 																												|
 * |											Author : Piyush Singh. 									        |
 * |											Date   : 06/05/2010 												|
 * | 																												|
 * |----------------------------------------------------------------------------------------------------------------| 
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using CAB.DALC.Data.DataServices;
using CAB.Entity;
using CAB.Framework;
using CAB.Framework.Entity;
using System.Data.Common;
using CAB.Framework.Utility;
using Hunt.EPIC.Logging;

namespace CAB.DALC.Data
{
    public class TamperTypeMasterDAL : DALBase
    {
        //1.LTCT IEC
        //2.LTCT DLMS
        //3.Ruby IEC
        //4.Ruby DLMS
        //Compartment 4 means transaction and other all are temper.
        //0 means All compartment
        private string TamperTypeID = "TamperTypeID";
        private string TamperType = "TamperType";
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(TamperTypeMasterDAL).ToString());
        private ApplicationType apptype = ConfigInfo.GetApplicationType();
        public void InsertDefaultTamper()
        {
            if (apptype.Equals(ApplicationType.DLMS_LTCT_650))
                InsertDefaultDLMS650Tamper();
            else
                InsertDefaultIEC650Tamper();
        }
        private void InsertDefaultIEC650Tamper()
        {
            string[] qry = new string[38]; //add pradipta_neu  38
            qry[0] = "Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment) values(161,'Voltage Imbalance R Phase Tamper',1,1)";
            qry[1] = "Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment) values(162,'Voltage Imbalance Y Phase Tamper',1,1)";
            qry[2] = "Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment) values(163,'Voltage Imbalance B Phase Tamper',1,1)";
            qry[3] = "Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment) values(164,'Missing Potential R Phase Tamper',1,1)";
            qry[4] = "Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment) values(165,'Missing Potential Y Phase Tamper',1,1)";
            qry[5] = "Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment) values(166,'Missing Potential B Phase Tamper',1,1)";
            qry[6] = "Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment) values(170,'Low/Under Voltage R Phase Tamper',1,1)";
            qry[7] = "Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment) values(171,'Low/Under Voltage Y Phase Tamper',1,1)";
            qry[8] = "Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment) values(172,'Low/Under Voltage B Phase Tamper',1,1)";
            qry[9] = "Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment) values(167,'High/Over Voltage R Phase Tamper',1,1)";
            qry[10] = "Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment) values(168,'High/Over Voltage Y Phase Tamper',1,1)";
            qry[11] = "Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment) values(169,'High/Over Voltage B Phase Tamper',1,1)";

            qry[12] = "Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment) values(177,'CT Short Tamper',1,2)";
            qry[13] = "Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment) values(178,'CT Open R Phase Tamper',1,2)";
            qry[14] = "Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment) values(179,'CT Open Y Phase Tamper',1,2)";
            qry[15] = "Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment) values(180,'CT Open B Phase Tamper',1,2)";
            qry[16] = "Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment) values(181,'Current Without Voltage R Phase Tamper',1,2)";
            qry[17] = "Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment) values(182,'Current Without Voltage Y Phase Tamper',1,2)";
            qry[18] = "Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment) values(183,'Current Without Voltage B Phase Tamper',1,2)";
            qry[19] = "Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment) values(184,'Low Power Factor R Phase Tamper',1,2)";
            qry[20] = "Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment) values(185,'Low Power Factor Y Phase Tamper',1,2)";
            qry[21] = "Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment) values(186,'Low Power Factor B Phase Tamper',1,2)";
            qry[22] = "Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment) values(187,'One Phase Neutral Absent Tamper',1,2)"; 
            qry[23] = "Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment) values(194,'Voltage Phase Reversal Tamper',1,3)";
            qry[24] = "Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment) values(195,'Current Imbalance R Phase Tamper',1,3)";
            qry[25] = "Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment) values(196,'Current Imbalance Y Phase Tamper',1,3)";
            qry[26] = "Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment) values(197,'Current Imbalance B Phase Tamper',1,3)";
            qry[27] = "Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment) values(198,'Current Reversal R Phase Tamper',1,3)";
            qry[28] = "Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment) values(199,'Current Reversal Y Phase Tamper',1,3)";
            qry[29] = "Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment) values(200,'Current Reversal B Phase Tamper',1,3)";
            qry[30] = "Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment) values(209,'Magnetic Influence Tamper',1,4)";
            qry[31] = "Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment) values(210,'Neutral Disturbance Tamper',1,4)";
            qry[32] = "Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment) values(211,'Front Cover Opening Tamper',1,4)";
            qry[33] = "Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment) values(212,'Terminal cover opening Tamper',1,4)";
            qry[34] = "Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment) values(226,'Total Tamper',1,6)";
            qry[35] = "Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment) values(225,'Power On/Off',1,5)";
            qry[36] = "Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment) values(227,'Low Load',1,6)";
            qry[37] = "Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment) values(228,'Over Load',1,6)";


           // qry[37] = "Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment) values(71,'High Neutral Current',1,1)";//add pradipta_neu


            IDataHelper helper = DatabaseFactory.GetHelper();
            for (int i = 0; i < 38; i++)
            {
                if (qry[i] != null)
                {
                    DataRequest request = new DataRequest(qry[i]);
                    helper.ExecuteNonQuery(request);
                }
            }
        }

        private void InsertDefaultDLMS650Tamper()
        {
            //GKG  Added Tamper for kvahselection
            //string[] qry = new string[46];
            List<string> query = new List<string>();
            //GKG  Added Tamper for kvahselection
            query.Add( "Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(1,'R-Phase - PT link Missing (Missing Potential) - Occurrence',2,1,1)");
            query.Add( "Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(2,'R-Phase - PT link Missing (Missing Potential) - Restoration',2,1,2)");
            query.Add( "Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(3,'Y-Phase - PT link Missing (Missing Potential) - Occurrence',2,1,1)");
            query.Add( "Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(4,'Y-Phase - PT link Missing (Missing Potential) - Restoration',2,1,2)");
            query.Add( "Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(5,'B-Phase - PT link Missing (Missing Potential) - Occurrence',2,1,1)");
            query.Add( "Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(6,'B-Phase - PT link Missing (Missing Potential) - Restoration',2,1,2)");
            query.Add( "Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(7,'Over Voltage in any Phase/Phase Split - Occurrence',2,1,1)");
            query.Add( "Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(8,'Over Voltage in any Phase/Phase Split - Restoration',2,1,2)");
            //query.Add( "Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(9,'Low Voltage in any Phase - Occurrence',2,1,1)");
            //query.Add( "Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(10,'Low Voltage in any Phase - Restoration',2,1,2)");
            query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(9,'Low Voltage in any Phase - Occurrence',0,1,1)");
            query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(10,'Low Voltage in any Phase - Restoration',0,1,2)");

            query.Add( "Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(11,'Voltage Unbalance - Occurrence',2,1,1)");
            query.Add( "Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(12,'Voltage Unbalance - Restoration',2,1,2)");

            query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(47,'Invalid Phase Association - Occurrence',0,1,1)");
            query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(48,'Invalid Phase Association - Restoration',0,1,2)"); 

            query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(49,'Invalid Voltage - Occurrence',2,1,1)");
            query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(50,'Invalid Voltage - Restoration',2,1,2)");
            //query.Add( "Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(51,'Phase - R CT Reverse - Occurrence',2,2,1)");
            //query.Add( "Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(52,'Phase - R CT Reverse - Restoration',2,2,2)");
            //*Insert query Reverse Tamper */
            query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(51,'Phase - R CT Reverse - Occurrence',0,2,1)");

            query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(52,'Phase - R CT Reverse - Restoration',0,2,2)");

            query.Add( "Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(53,'Phase - Y CT Reverse - Occurrence',2,2,1)");
            query.Add( "Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(54,'Phase - Y CT Reverse - Restoration',2,2,2)");
            query.Add( "Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(55,'Phase - B CT Reverse - Occurrence',2,2,1)");
            query.Add( "Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(56,'Phase - B CT Reverse - Restoration',2,2,2)");
            query.Add( "Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(57,'Phase - R CT Open - Occurrence',2,2,1)");
            query.Add( "Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(58,'Phase - R CT Open - Restoration',2,2,2)");
            query.Add( "Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(59,'Phase - Y CT Open - Occurrence',2,2,1)");
            query.Add( "Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(60,'Phase - Y CT Open - Restoration',2,2,2)");
            query.Add( "Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(61,'Phase - B CT Open - Occurrence',2,2,1)");
            query.Add( "Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(62,'Phase - B CT Open - Restoration',2,2,2)");
            query.Add( "Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(63,'Current Unbalance - Occurrence',2,2,1)");
            query.Add( "Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(64,'Current Unbalance - Restoration',2,2,2)");
            
            query.Add( "Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(65,'CT Bypass - Occurrence',2,2,1)");//remove By pradipta_tamper
            query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(66,'CT Bypass - Restoration',2,2,2)");

            //query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(65,'CT Bypass/ High Neutral Current - Occurrence',2,2,1)");
            //query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(66,'CT Bypass/ High Neutral Current - Restoration',2,2,2)");
           
            
            query.Add( "Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(67,'Over Current in any Phase - Occurrence',0,2,1)");
            query.Add( "Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(68,'Over Current in any Phase - Restoration',0,2,2)");
            /* Event Logging: Over Load Tamper  */ // Story - 428915 - new tamper added
            //query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(91,'Over Load - Occurrence',2,2,1)");
            //query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(92,'Over Load - Restoration',2,2,2)");
            query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(91,'Over Load - Occurrence',0,2,1)");
            query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(92,'Over Load - Restoration',0,2,2)");
            /* ******************* */
            query.Add( "Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(101,'Power Failure - Occurrence',0,3,1)");
            query.Add( "Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(102,'Power Failure - Restoration',0,3,2)");
            query.Add( "Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(151,'Real Time Clock - Date and Time',2,4,0)");
            query.Add( "Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(152,'Demand Integration Period',0,4,0)");
            query.Add( "Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(153,'Profile Capture Period',0,4,0)");
            query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(154,'Single-Action Schedule for Billing Dates/BILL Date Change',0,4,0)");
            // Billing Cycle Transcation
            query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(194,'Single-Action Schedule for Billing Periods',0,4,0)");

            query.Add( "Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(155,'Activity Calendar for Time Zones etc.',0,4,0)");
            //query.Add( "Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(156,'CT Ratio Changed',2,4,0)");
            query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(157,'PT Ratio Changed/New Firmware Activated',2,4,0)");
            query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(190,'CT Ratio Changed',2,4,0)");
            query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(191,'PT Ratio Changed',2,4,0)");
            //GKG  Added Tamper for kvahselection Tamper - 
            //qry[39] = "Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(201,'Permanent Magnet or AC/DC Electromagnet - Occurrence',2,5,1)";
            //qry[40] = "Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(202,'Permanent Magnet or AC/DC Electromagnet - Restoration',2,5,2)";
            //qry[41] = "Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(203,'Neutral Disturbance - HF & DC - Occurrence',2,5,1)";
            //qry[42] = "Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(204,'Neutral Disturbance - HF & DC - Restoration',2,5,2)";
            //qry[43] = "Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(205,'Very Low PF - Occurrence',2,5,1)";
            //qry[44] = "Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(206,'Very Low PF - Restoration',2,5,2)";
            //qry[45] = "Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(251,'Meter Cover Opening - Occurrence',2,5,1)";

            query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(158,'kVAh Selection Changed/Load Limit (Kw) set',2,4,0)");

            //  Sapphire LTCT Meter [ST] new DLMS code added. User Story 464096
            query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(188,'kVAh Selection Changed',2,4,0)");

            query.Add( "Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(201,'Permanent Magnet or AC/DC Electromagnet - Occurrence',0,5,1)");
            query.Add( "Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(202,'Permanent Magnet or AC/DC Electromagnet - Restoration',0,5,2)");
            query.Add( "Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(203,'Neutral Disturbance - HF & DC - Occurrence',0,5,1)");
            query.Add( "Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(204,'Neutral Disturbance - HF & DC - Restoration',0,5,2)");
            //query.Add( "Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(205,'Very Low PF - Occurrence',2,5,1)");
            //query.Add( "Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(206,'Very Low PF - Restoration',2,5,2)");
            query.Add( "Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(205,'Very Low PF - Occurrence',0,5,1)");
            query.Add( "Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(206,'Very Low PF - Restoration',0,5,2)");

            query.Add( "Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(251,'Meter Cover Opening - Occurrence',0,5,1)");
            //GKG  Added Tamper for kvahselection 
            //VBM - Added New Tamper's for KSEB.
            query.Add( "Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(13,'Voltage Phase Sequence Reversal - Occurrence',2,1,1)");
            query.Add( "Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(14,'Voltage Phase Sequence Reversal - Restoration',2,1,2)");

            query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(159,'MD Reset/Load Limit Function-Enabled',0,4,0)");
            query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(160,'Display - Push Mode Config/Load Limit Function-Disabled',2,4,0)");
            query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(161,'Display - Scroll Mode Config/LLS Secret (MR)Change',2,4,0)");
            query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(162,'Display - HR Mode Config/HLS key (US)Change',2,4,0)");

            // Sapphire LTCT Meter [ST] new DLMS code added. User Story 464096
            query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(192,'Display Parameter',2,4,0)");


            query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(163,'Display - Scroll Time Config/HLS key (FW)Change',2,4,0)");
            query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(164,'Auto Billing Lock/Global key change',2,4,0)");

            // Sapphire LTCT Meter [ST] new DLMS code added. User Story 464096
            query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(187,'Auto Billing Lock',2,4,0)");


            query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(165,'RS232 Lock/ESWF change',2,4,0)");

            // Sapphire LTCT Meter [ST] new DLMS code added. User Story 464096
            query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(186,'RS232 Lock',2,4,0)");

            query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(166,'Daily Log Parameters/MD Reset',2,4,0)");
            query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(167,'Software Billing Lock/Metering Mode',2,4,0)");

            query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(168,'Manual Billing Lock',2,4,0)");

            // Sapphire LTCT Meter [ST] new DLMS code added. User Story 464096
            query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(185,'Software Billing Lock',2,4,0)");
            // Sapphire LTCT Meter [ST] new DLMS code added. User Story 464096
            query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(184,'Manual Billing Lock',2,4,0)");

            query.Add( "Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(69, 'Earth Loading/Current Bypass - Occurrence',3,2,1)");
            query.Add( "Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(70, 'Earth Loading/Current Bypass - Restoration', 3, 2, 2)");
            query.Add( "Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(207, 'Single Wire Operation - Occurrence', 3, 5, 1)");
            query.Add( "Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(208, 'Single Wire Operation - Restoration', 3, 5, 2)");

            //Insert Over Voltage tamper for 1P DLMS meter supported in Compartment Other(5)
            query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(245,'Phase in Neutral/OverVoltage - Occurrence',3,5,1)");
            query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(246,'Phase in Neutral/OverVoltage - Restoration',3,5,2)");

            //User story 433253- Updated the name (2PN) 
            query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(247, '2 Phase Without Neutral (2PN) - Occurrence',2,5,1)");
            //User story 433253- Updated the name (2PN)
            query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(248, '2 Phase Without Neutral (2PN) - Restoration',2,5,2)");
            /*Insert query for ESD Tamper Occurance */
           //query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(249,'ESD - Occurrence',3,5,1)");
            /*Insert query for ESD Tamper Resturation */
           //query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(250,'ESD - Restoration',3,5,2)");

            query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(243,'Low Supply Voltage - Occurrence',3,5,1)");
            query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(244,'Low Supply Voltage - Restoration',3,5,2)");

            query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(189,'MD Reset',0,4,0)");

            //// User Story 456437, 455259            
            query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(256,'Tamper Reset',3,4,0)");

            // Task ID: 569567: Tamper Reset Option in 3P DLMS Sapphire Two TOU "sc" model
            query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(200,'Tamper Reset',2,4,0)");

            query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(301,'Relay - Disconnected',0,5,1)");
            query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(302,'Relay - Connected',0,5,2)");
            query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(297,'COMS Card Removal Occurence',0,5,1)");

            // Single phase smart meter

            query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(301,'Relay - Disconnected',0,5,1)");
            query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(302,'Relay - Connected',0,5,2)");

            query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(297,'COMS Card Removal Occurence',0,5,1)");
            //@Deep: RS 485 tamper event id: 156 created and updated over previous CT ratio Config id as per latest ammendment of DLMS
            query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(156,'RS 485',2,4,0)");

            // Low load Occurrence and restoration
            query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(93,'Low Load - Occurrence',0,2,1)");
            query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(94,'Low Load - Restoration',0,2,2)");

       
            query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(71,'High Neutral Current - Occurrence',2,2,1)");//add pradipta_neu
            query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(72,'High Neutral Current - Restoration',2,2,2)");

            //Insert plugin module in smart meter
            query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(209,'Plugin Module Removal - Occurrence',0,5,1)");
            query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(210,'Plugin Module Removal - Restoration',0,5,2)");
            

			//SarkarA code change start 20180308 // add Current Mismatch Tamper
            query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(703,'Current Mismatch - Occurrence',3,2,1)");
            query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(704,'Current Mismatch - Restoration',3,2,2)");
            //SarkarA code change end 20180308

            query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(1001,'Digital Input 2 Occurrence',2,5,1)");
            query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(1002,'Digital Input 2 Restoration',0,5,2)");

            query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(1003,'Digital Input 3 Occurrence',0,5,1)");
            query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(1004,'Digital Input 3 Restoration',0,5,2)");

            query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(1005,'Digital Input 4 Occurrence',0,5,1)");
            query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(1006,'Digital Input 4 Restoration'0,5,2)");

            query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(1007,'Digital Input 5 Occurrence',0,5,1)");
            query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(1008,'Digital Input 5 Restoration',0,5,2)");

            query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(1009,'Digital Input 6 Occurrence',0,5,1)");
            query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(1010,'Digital Input 6 Restoration',0,5,2)");

            query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(239,'Temperature Occurrence',0,5,1)");
            
            query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(951,'Temperature Occurrence',0,5,1)");
            query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(952,'Temperature Restoration',0,5,2)");
            query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(959,'%THDV R- Phase Tamper Occurance',0,5,1)");
            query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(960,'%THDV R- Phase Tamper Restoration',0,5,2)");
            query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(961,'%THDV Y- Phase Tamper Occurance',0,5,1)");
            query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(962,'%THDV Y- Phase Tamper Restoration',0,5,2)");
            query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(963,'%THDV B- Phase Tamper Occurance',0,5,1)");
            query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(964,'%THDV B- Phase Tamper Restoration',0,5,2)");
            query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(965,'%THDI R- Phase Tamper Occurance',0,5,1)");
            query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(966,'%THDI R- Phase Tamper Restoration',0,5,2)");
            query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(967,'%THDI Y- Phase Tamper Occurance',0,5,1)");
            query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(968,'%THDI Y- Phase Tamper Restoration',0,5,2)");
            query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(969,'%THDI B- Phase Tamper Occurance',0,5,1)");
            query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(970,'%THDI B- Phase Tamper Restoration',0,5,2)");

           query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(801,'ESD - Occurrence',0,5,1)");
            /*Insert query for ESD Tamper Resturation */
            query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(802,'ESD - Restoration',0,5,2)");


            //pradipta code change start  // add Abnormal Power Off
            query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(803,'Abnormal Power Off - Occurrence',3,5,1)");
            query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(804,'Abnormal Power Off - Restoration',3,5,2)");
            //For Falcon2 LTCT added 29-10-18
            query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(805,'Invalid Phase Association - Occurrence',0,5,1)");
            query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(806,'Invalid Phase Association - Restoration',0,5,2)");
           
            
            //pradipta code change end 
            //************* For smart meter start *************
            query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(701,'High Neutral Current - Occurence',0,2,1)");
            query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(702,'High Neutral Current - Restoration',0,2,2)");
            //Configuration changed to postpaid mode
            query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(211,'Configuration changed to postpaid mode',0,5,2)");
            //Configuration changed to forward only mode
            query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(213,'Configuration changed to forward only mode',0,5,2)");
            //Configuration changed to import and export mode
            query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(214,'Configuration changed to import and export mode',0,5,2)");
            query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(215,'Overload - Occurence',0,5,1)");
            query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(216,'Overload - Restoration',0,5,2)");
            query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(163,'HLS key (FW)Change',0,4,0)");
            query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(166,'MD Reset',0,4,0)");
            query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(167,'Metering Mode',0,4,0)");
            query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(177,'ConfigChanged Forward Mode Only',0,2,0)");
            query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(178,'Config Changed Import and Export Mode',0,2,0)");
            query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(751,'Last Token Recharge Amount prepaid',0,4,0)");
            query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(752,'Last Token Recharge Time prepaid',0,4,0)");
            query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(753,'Total Amount Last Recharge prepaid',0,4,0)");
            query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(754,'Current Balance Amount prepaid',0,4,0)");
            query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(755,'Current Balance Time prepaid',0,4,0)");
            query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(756,'Digital output Operation',0,4,0)");
            query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(757,'Sliding Demand Period Change',0,4,0)");
            query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(758,'Event Threshold Config Change',0,4,0)");
            query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(759,'Event Threshold Persistence time Change',0,4,0)");
            query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(760,'Event Display Parameters Change',0,4,0)");
            query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(761,'LS Parameter Store ID',0,4,0)");
            query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(762,'Optical port Lock',0,4,0)");
            query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(763,'Optical port Unlock',0,4,0)");
            query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(764,'RJ port Lock',0,4,0)");
            query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(765,'RJ port Unlock',0,4,0)");
            query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(766,'Special Day',0,4,0)");
            query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(767,'Event Enable/Disable Config',0,4,0)");
            query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(768,'Load Control Paramter',0,4,0)");
            query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(769,'ARM button Enable',0,4,0)");
            query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(770,'ARM button Disable',0,4,0)");
            query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(771,'FS Mode Lock',0,4,0)");
            query.Add("Insert Into TamperType_Master(TamperTypeID,TamperType,MeterType,Compartment,TamperCategory) values(772,'FS Mode Unlock',0,4,0)");
            //************* For smart meter end *************
            
            IDataHelper helper = DatabaseFactory.GetHelper();
            //GKG  Added Tamper for kvahselection
            //for (int i = 0; i < 46; i++)
            //GKG  Added Tamper for kvahselection
            foreach (string qry in query)
            {
                DataRequest request = new DataRequest(qry);
                helper.ExecuteNonQuery(request);
            }
        } 
        public override IEntity InsertData(IEntity entity)
        {
            TamperTypeEntity tamperTypeEntity = null;
            if (entity == null)
                return tamperTypeEntity;
              tamperTypeEntity = entity as TamperTypeEntity;
            bool Flag = false;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Insert Into TamperType_Master(TamperType) values(");
                builder.Append(string.Concat(ParameterName(TamperType), ")")); 
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(TamperType), tamperTypeEntity.TamperType, DbType.String, 50); 
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Tamper Type ", tamperTypeEntity.TamperType, " added"));
                Flag = true;
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "InsertData(IEntity entity)", ex);
                tamperTypeEntity = null;
            }
            return tamperTypeEntity;
        }

        public override bool UpdateData(IEntity entity)
        {
            bool Flag = false;
            try
            {
                TamperTypeEntity tamperTypeEntity = entity as TamperTypeEntity;
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Update TamperType_Master Set ");
                builder.Append(string.Concat( TamperType, "=", ParameterName(TamperType ))); 
                builder.Append(string.Concat(" Where ", TamperTypeID, "=", ParameterName(TamperTypeID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(TamperTypeID), tamperTypeEntity.TamperTypeID, DbType.Int32);
                request.AddParamter(ParameterName(TamperType), tamperTypeEntity.TamperType, DbType.String, 50); 
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Tamper Type ", tamperTypeEntity.TamperType, " modified"));
                Flag = true;
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "UpdateData(IEntity entity)", ex);
                Flag = false;
            }
            return Flag;
        }

        public override bool DeleteData(IEntity entity)
        {
            bool Flag = false;
            try
            {
                TamperTypeEntity tamperTypeEntity = entity as TamperTypeEntity;
                if (tamperTypeEntity == null)
                    return false;
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Delete From TamperType_Master ");
                builder.Append(string.Concat(" Where ", TamperTypeID, "=", ParameterName(TamperTypeID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(TamperTypeID), tamperTypeEntity.TamperTypeID, DbType.Int64);
                helper.ExecuteNonQuery(request);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Tamper Type ", tamperTypeEntity.TamperType, " deleted"));
                Flag = true;
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "DeleteData(IEntity entity)", ex);
                Flag = false;
            }
            return Flag;
        }

        public override IEntity GetDetailData(int id)
        {
            TamperTypeEntity TamperTypeEntity = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select TamperTypeID,TamperType,MeterType,Compartment from TamperType_Master where ");
                builder.Append(string.Concat(TamperTypeID, "=", ParameterName(TamperTypeID)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(TamperTypeID), id, DbType.Int64);
                DataSet ds = new DataSet();
                ds = helper.FillDataSet(request, ds);
                if (ds.Tables[0].Rows.Count > 0)
                    TamperTypeEntity = (TamperTypeEntity)RowToEntity(ds.Tables[0].Rows[0]);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Tamper type viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetDetailData(int id)", ex);
                TamperTypeEntity = null;
            }
            return TamperTypeEntity;
        }

        public IEntity GetDetailData(string tamperType)
        {
            TamperTypeEntity TamperTypeEntity = new TamperTypeEntity(); 
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select TamperTypeID,TamperType,MeterType,Compartment from TamperType_Master where ");
                builder.Append(string.Concat(TamperType, "=", ParameterName(TamperType)));
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName(TamperType), tamperType, DbType.String, 10);
                DataSet ds = new DataSet();
                ds = helper.FillDataSet(request, ds);
                if (ds.Tables[0].Rows.Count > 0)
                    TamperTypeEntity = (TamperTypeEntity)RowToEntity(ds.Tables[0].Rows[0]);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Tamper type viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetDetailData(string tamperType)", ex);
            }
            return TamperTypeEntity;
        }

        public DataSet GetAllTamperMasterData()
        {
            TamperTypeEntity TamperTypeEntity = new TamperTypeEntity();
            DataSet ds = new DataSet();
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("Select TamperTypeID,TamperType,Compartment from TamperType_Master  ");
                DataRequest request = new DataRequest(builder.ToString());
              
                ds = helper.FillDataSet(request, ds);
                //if (ds.Tables[0].Rows.Count > 0)
                //    TamperTypeEntity = (TamperTypeEntity)RowToEntity(ds.Tables[0].Rows[0]);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Tamper type viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetAllTamperMasterData()", ex);
            }
            return ds;
        }

        public override IList<IEntity> ListData()
        {
            throw new NotImplementedException();
        }
         public override DataSet ListDataSet()
        {
            return null;
        }
        public   DataSet ListDataSet(int compartment)
        { 
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                if (apptype.Equals(ApplicationType.DLMS_LTCT_650))
                {
                    if (compartment == 0) //All Tamper except transaction(Comp 4)
                    {
                        builder.Append("Select TamperTypeID,TamperType from TamperType_Master where (MeterType=0 or MeterType=" + GetMeterType() + ") and Compartment<>4 order by TamperTypeID");
                       
                    }
                    else
                    {
                        builder.Append("Select TamperTypeID,TamperType from TamperType_Master where (MeterType=0 or MeterType=" + GetMeterType() + ") and Compartment=" + compartment + " order by TamperTypeID");
                    }
                }
                else
                    builder.Append("Select TamperTypeID,TamperType from TamperType_Master order by TamperTypeID");

                DataRequest request = new DataRequest(builder.ToString());
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);

                //SarkarA code change start 20180327 // add Phase in neutral tamper for NetMeters
                string meterVariant = "";
                int meterModel = 0;
                if (ConfigInfo.ActiveMeterDataId != null)
                {
                    meterVariant = new DLMS650GeneralDAL().GetMeterVariantByMeterDataID(ConfigInfo.ActiveMeterDataId).Tables[0].Rows[0][0].ToString();
                    meterModel = new DLMS650GeneralDAL().GetMeterModelNoByMeterDataID(ConfigInfo.ActiveMeterDataId);

                    if ((meterVariant.Equals(CAB.Framework.MeterVariant.THREE) || meterVariant.Equals(CAB.Framework.MeterVariant.FOUR)) && meterModel == CAB.Framework.NamePlateConstants.SapphireValue || meterModel == CAB.Framework.NamePlateConstants.Sapphire_Netmeter_WCM)
                    {
                        DataSet ds = new DataSet();
                        request = new DataRequest("Select TamperTypeID,TamperType,Compartment from TamperType_Master where TamperType = 'Phase in Neutral/OverVoltage - Occurrence' or TamperType = 'Phase in Neutral/OverVoltage - Restoration'");
                        ds = helper.FillDataSet(request, ds);
                        if (compartment == 0 || compartment == (int)ds.Tables[0].Rows[0]["Compartment"])
                        {
                            foreach (DataRow row in ds.Tables[0].Rows)
                            {
                                DataRow temprow = dataSet.Tables[0].NewRow();
                                temprow[0] = row[0];
                                temprow[1] = row[1];
                                dataSet.Tables[0].Rows.Add(temprow);
                            }
                            dataSet.Tables[0].DefaultView.Sort = "TamperTypeID ASC";
                            DataTable temptable = dataSet.Tables[0].DefaultView.ToTable();
                            dataSet.Tables.RemoveAt(0);
                            dataSet.Tables.Add(temptable);
                        }
                    }
                }
                //SarkarA code change end 20180327

                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Tamper type viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "ListDataSet(int compartment)", ex);
                dataSet = null;
            }
            return dataSet;
        }
        /// <summary>
        /// Overload for ListDataset, get the tamper,transaction data according to the utility 
        /// (on the basis of whether ct/pt transactions are disabled or enabled)
        /// </summary>
        /// <param name="compartment"></param>
        /// <param name="ctPTDisabled"></param>
        /// <returns></returns>
        public DataSet ListDataSet(int compartment,bool ctPTDisabled)
        {
            DataSet dataSet = null;
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                if (apptype.Equals(ApplicationType.DLMS_LTCT_650))
                {
                    if (compartment == 4)
                    {
                        if (ctPTDisabled)
                        {
                            builder.Append("Select TamperTypeID,TamperType from TamperType_Master where (MeterType=0 or MeterType=" + GetMeterType() + ") and Compartment=4 and TamperTypeID not in (156,157) order by TamperTypeID");
                        }
                        else
                        {
                            builder.Append("Select TamperTypeID,TamperType from TamperType_Master where (MeterType=0 or MeterType=" + GetMeterType() + ") and Compartment=4 order by TamperTypeID");
                        }
                    }
                    else
                    {
                        builder.Append("Select TamperTypeID,TamperType from TamperType_Master where (MeterType=0 or MeterType=" + GetMeterType() + ") and Compartment<>4 order by TamperTypeID");
                    }
                }
                else
                {
                    builder.Append("Select TamperTypeID,TamperType from TamperType_Master order by TamperTypeID");
                }

                DataRequest request = new DataRequest(builder.ToString());
                dataSet = new DataSet();
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Tamper type viewed"));
            }
            catch (CABException ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "ListDataSet(int compartment,bool ctPTDisabled)", ex);
                dataSet = null;
            }
            return dataSet;
        }
        /// <summary>
        /// gives meter type id , 3 is for single phase & 2 is for 3 Phase , dafualt is 2 for utility.cs
        /// </summary>
        /// <returns>meter type</returns>
        private byte GetMeterType()
        {
            byte meterType = 2;
            switch (ConfigInfo.ActiveMeterType)
            {    
                case "1P-2W":
                    meterType = 3;
                    break;
                case "3P-4W":
                    meterType = 2;
                    break;
                case "3P-3W":
                    meterType = 2;
                    break;
            }
            return meterType;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public DataSet GetMeterModelandFirmware(int meterdataID)
        {
            DataSet dataSet = new DataSet();
            try
            {
                IDataHelper helper = DatabaseFactory.GetHelper();
                StringBuilder builder = new StringBuilder();
                builder.Append("SELECT MeterModelNo, InternalFirmwareVersion FROM `dlms_ltct_650`.`meterdata_general` where MeterData_ID= @meterdataID");
                DataRequest request = new DataRequest(builder.ToString());
                request.AddParamter(ParameterName("@meterdataID"), meterdataID, DbType.Int32);
                dataSet = helper.FillDataSet(request, dataSet);
                UserLogActivityDAL.GetInstance().GenerateAndSaveLog(string.Concat("Visibility Tab Viewed"));
            }
            catch (Exception ex)    //Exception log for catch block
            {
                logger.Log(LOGLEVELS.Error, "GetMeterModelandFirmware(int meterdataID)", ex);
            }
            return dataSet;
        }
        

        public override IEntity RowToEntity(DataRow row)
        {
            if (row == null) return null;
            TamperTypeEntity tamperTypeEntity = new TamperTypeEntity();
            if (NotNullAndNotDBNull(row, TamperTypeID)) tamperTypeEntity.TamperTypeID = Convert.ToInt32(row[TamperTypeID]);
            if (NotNullAndNotDBNull(row, TamperType)) tamperTypeEntity.TamperType = Convert.ToString(row[TamperType]);
            return tamperTypeEntity;
        }

        public override IEntity InsertData(IList<IEntity> entities)
        {
            throw new NotImplementedException();
        }
    }
}



