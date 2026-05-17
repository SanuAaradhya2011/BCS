using System;
using System.Data;
using System.Threading;
using GPRSCommunication;
using Hunt.EPIC.Logging;
using GPRSComService.Framework;
using CAB.DALC.Data;
using System.Text;
using System.Runtime.Serialization;
using System.IO;
using System.Configuration;

namespace GPRSComService.Worker
{
    class EndpointOperationsWorker : WorkerBase 
    {
        private static readonly IGeneralLog logger = LogFactory.CreateGeneralLogger(typeof(EndpointOperationsWorker).ToString());
        private bool continueExecution = true;

        protected override void OnStart(object parameters)
        {
            while (continueExecution)
            {
                MigrateEndPoints(); 
           
            }
        }


        /// <summary>
        /// Worker method to migrate endpoints from BCS to GPRS Adapter
        /// </summary>
        private void MigrateEndPoints()
        {
            try
            {
               
                
                DataSet dsEndPoints = MeterMasterDAL.GetEndPointsForBulkMigrate();
                
                LandisGyr.AMI.Layers.Endpoint[] endpoints = null;
                StringBuilder syncedMeterIdXml = new StringBuilder();


                if (dsEndPoints != null && dsEndPoints.Tables != null && dsEndPoints.Tables[0].Rows.Count > 0)
                {
                    endpoints = new LandisGyr.AMI.Layers.Endpoint[dsEndPoints.Tables[0].Rows.Count];
                    int index = 0;
                    syncedMeterIdXml.Append("<xml>");

                    foreach (DataRow dr in dsEndPoints.Tables[0].Rows)
                    {
                        try
                        {
                            endpoints[index] = new LandisGyr.AMI.Layers.Endpoint();
                            endpoints[index].Model = new LandisGyr.AMI.Layers.EndpointModel();
                            endpoints[index].Model.Type = LandisGyr.AMI.Layers.EndpointType.GPRS;
                            endpoints[index].SerialNumber = dr["Meter_GPRSModem_IMEI"].ToString();
                            endpoints[index].IsUsable = true;
                            endpoints[index].AssociatedMeters = new LandisGyr.AMI.Layers.Meter[1];
                            endpoints[index].AssociatedMeters[0] = new LandisGyr.AMI.Layers.Meter();
                            endpoints[index].AssociatedMeters[0].MeterNumber = dr["Meter_ID"].ToString();
                            endpoints[index].AssociatedMeters[0].Model = new LandisGyr.AMI.Layers.MeterModel();
                            endpoints[index].AssociatedMeters[0].Model.Type = LandisGyr.AMI.Layers.MeterType.Electric;
                            syncedMeterIdXml.Append("<meterid>" + endpoints[index].AssociatedMeters[0].MeterNumber + "</meterid>");
                            index++;
                        }
                        catch (Exception ex)
                        {
                            logger.Log(LOGLEVELS.Error, ex);
                        }
                    }

                    syncedMeterIdXml.Append("</xml>");

                    try
                    {
                        EndPointOperations.AddEndpoints(endpoints);
                        MeterMasterDAL.BulkUpdateEndPointSyncStatus(syncedMeterIdXml, index);

                    }

                    catch (Exception ex)
                    {

                        logger.Log(LOGLEVELS.Error,"Fatal error occured while adding endpoints to GPRS Adapter.", ex);

                    }

                }
                else
                {
                   
                    // if there is no endpoints to sync,make the thread sleep
                    Int32 sleepDuration = Constants.EndPointSyncSleepDuration;
                    Int32.TryParse(ConfigurationSettings.AppSettings[Constants.EndPointSyncSleepDurationKey], out sleepDuration);
                    logger.Log(LOGLEVELS.Info,string.Format("No more endpoints to migrate.Thread is going to sleep for {0}.",sleepDuration));
                    Thread.Sleep(sleepDuration*1000);
                }

            }
            catch
            {
               
                // code will fall in the block when database is not available so making the thread sleep 
                Int32 sleepDuration = Constants.EndPointSyncSleepDuration;
                Int32.TryParse(ConfigurationSettings.AppSettings[Constants.EndPointSyncSleepDurationKey], out sleepDuration);
                logger.Log(LOGLEVELS.Error,String.Format("Exception occurs while migrating endpoints.Thread is going to sleep for {0}.",sleepDuration));
                Thread.Sleep(sleepDuration * 1000); // 10 minute sleep
            }
     }

        protected override void OnStop()
        {
            base.OnStop();
            continueExecution = false;
        }
    }
        
    
}

