using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LandisGyr.AMI.Layers;

namespace GPRSComService
{
    class RequestCommandQueue
    {
        private static Queue<RequestOfbase64Binary> commandQueue = new Queue<RequestOfbase64Binary>();
        private static List<string> OldCommandList = new List<string>();
        private static List<string> NewCommandList = new List<string>();




        private static object lockObject = new object();
        private static object CommandLock = new object();

        /// <summary>
        /// Enqueue the command is Command out queue
        /// </summary>
        /// <param name="command"></param>
        public static void PushCommand(RequestOfbase64Binary command ) 
        {
            lock (lockObject)
            {
                commandQueue.Enqueue(command);
            }

            lock(CommandLock)
            {
                //Add Command Id to commandid Dictionary. This will be used to filter between inprocess and new command
                NewCommandList.Add(command.MessageID);
            }
        }


        /// <summary>
        /// Returns the number of commands in command queue
        /// </summary>
        public static int RequestCount
        {
            get
            {
                lock (lockObject)
                {
                    return commandQueue.Count();
                }
            }
        }

        /// <summary>
        /// Dequeue the command from command queue
        /// </summary>
        /// <returns></returns>
        public static RequestOfbase64Binary DequeueRequest()
        {
            if (commandQueue.Count > 0)
            {
                return commandQueue.Dequeue();
            }
            else
            {
                return null;
            }

        }

        /// <summary>
        /// Returns the commands to execute
        /// </summary>
        /// <returns></returns>

        public static List<RequestOfbase64Binary> GetCommandToPush()
        {
            int itemCount = 50;
            List<RequestOfbase64Binary> commands = new List<RequestOfbase64Binary>();

            lock (lockObject)
            {
                for (int i = 0; i < itemCount; i++)
                {
                    RequestOfbase64Binary command = RequestCommandQueue.DequeueRequest();
                    if (command == null)
                    {
                        break;
                    }
                    else
                    {
                        commands.Add(command);
                    }
                }
            }
            return commands;
        }

        /// <summary>
        /// Retuns the comma separate command ids
        /// </summary>
        /// <returns></returns>
        public static List<string> GetCommandId()
        {
            lock (CommandLock)
            {
                string[] tempList = null;

                if (NewCommandList.Count > 0)
                {
                    tempList = new string[NewCommandList.Count];

                    NewCommandList.CopyTo(tempList);

                    NewCommandList.RemoveRange(0, tempList.Length);

                 //   OldCommandList.AddRange(tempList);

                }
                return (tempList!=null)? tempList.ToList():null;
            }
        }

        public static void RemoveCommandIdFromOld(string commandId)
        {
            lock (CommandLock)
            {
                if (OldCommandList.Contains(commandId))
                {
                    OldCommandList.Remove(commandId);
                }
            }
        }

        /// <summary>
        /// Add the command to new command list. So that added command's response can be polled from GPRS.
        /// </summary>
        /// <param name="commandId"></param>
        public static void AddCommandIdToNewCommandList(string commandId)
        {
            lock (CommandLock)
            {
                NewCommandList.Add(commandId);
            }
        }

        /// <summary>
        /// Removes the command from newCommandList which is used to poll response.
        /// </summary>
        /// <param name="commandId"></param>
        public static void RemoveCommandFromNewCommandList(string commandId)
        {
            lock (CommandLock)
            {
                if (NewCommandList.Contains(commandId))
                {
                    NewCommandList.Remove(commandId);
                }
            }
        }
    }
}
