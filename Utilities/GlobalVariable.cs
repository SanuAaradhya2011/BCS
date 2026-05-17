using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SerialCommunication;

namespace Utilities
{
    public sealed class GlobalVariable
    {

        public int seqno = 0;
        private static volatile GlobalVariable instance;
        private static object syncRoot = new Object();
        private static volatile SerialComm objSerialComm;
        //public SerialComm objSerialComm = new SerialComm();

       

        private GlobalVariable() { }

        public static GlobalVariable Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new GlobalVariable();
                    }
                }

                return instance;
            }
        }
       

    }
}