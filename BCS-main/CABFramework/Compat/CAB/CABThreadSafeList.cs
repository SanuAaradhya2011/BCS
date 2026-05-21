using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LNG.Framework.Utility;

namespace LNG.Framework
{

    //public sealed class ThreadSafeListForMeterReadAPI
    //{
    //    private static volatile ThreadSafeListForMeterReadAPI threadSafeList = null;
    //    private List<READAPI> configurationList = new List<READAPI>();
    //    private object syncRoot = new object();
    //    private static object sync = new object();
    //    private bool duplicateInstanceID = false;
    //    private bool duplicatePort = false;

    //    public static ThreadSafeListForMeterReadAPI ListOfConfiguration
    //    {
    //        get
    //        {
    //            if (threadSafeList == null)
    //            {
    //                lock (sync)
    //                {
    //                    if (threadSafeList == null)
    //                        threadSafeList = new ThreadSafeListForMeterReadAPI();
    //                }
    //            }

    //            return threadSafeList;
    //        }

    //    }
    //    public void Add(READAPI value)
    //    {
    //        lock (syncRoot)
    //        {
    //            configurationList.Add(value);
    //        }
    //    }
    //    // try safe add
    //    public bool TrySafeAdd(READAPI readAPI)
    //    {
    //        lock (syncRoot)
    //        {
    //            duplicateInstanceID = false;
    //            duplicatePort = false;
    //            foreach (READAPI api in configurationList)
    //            {
    //                if (readAPI.INSTANCEID != api.INSTANCEID)
    //                {
    //                    // Try if the port name is same or if the port name is same and is busy
    //                    if (api.CONNECTIONDETAIL[0].PORT == readAPI.CONNECTIONDETAIL[0].PORT)
    //                    {
    //                        // duplicate port 
    //                        duplicatePort = true;
    //                        if (api.IsBusy)
    //                        {
    //                            duplicateInstanceID = true;
    //                        }
    //                        else
    //                        {
    //                            // should be up[dated with is busy = true and not be added in the list
    //                            duplicateInstanceID = false;
    //                            api.IsBusy = true;

    //                        }
    //                        break;
    //                    }
    //                }
    //                else
    //                {
    //                    duplicateInstanceID = true;
    //                    break;
    //                }
    //            }
    //            if (!duplicatePort && !duplicateInstanceID)
    //            {
    //                readAPI.IsBusy = true;
    //                Add(readAPI);
    //            }
    //        }
    //        return duplicateInstanceID;
    //    }

    //    public void SetAPIConfigurationToFree(READAPI readAPI)
    //    {
    //        lock (syncRoot)
    //        {
    //            foreach (READAPI api in configurationList)
    //            {
    //                if (readAPI.INSTANCEID == api.INSTANCEID)
    //                {
    //                    api.IsBusy = false;
    //                }
    //            }
    //        }
    //    }

    //}

}

