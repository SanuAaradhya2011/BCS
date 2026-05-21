using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace GPRSComService.Framework
{
    public abstract class WorkerBase
    {
        #region Nested Types
        #endregion

        #region Constants and Variables
        private bool isRunning;
        private Int32 threadTimeout;
        private Thread executingThread;
        private object startParameters;
        private EventWaitHandle canBeAborted;
        private EventWaitHandle signaledToStop;

        private List<Thread> workerThreads;
        #endregion

        #region Properties
        /// <summary>
        /// Set the Start Parameter that worker 
        /// may require to execute
        /// </summary>
        public Object StartParameters
        {
            set
            {
                startParameters = value;
            }
        }

        /// <summary>
        /// Get or set the time span (in sec) that worker should wait when asked to stop
        /// before aborting worker threads, if any.
        /// </summary>
        public Int32 ThreadTimeOut
        {
            get
            {
                return (threadTimeout == 0)? 1 : threadTimeout;
            }
            set
            {
                threadTimeout = value;
            }
        }

        /// <summary>
        /// Get or set the thread on which working is being executed
        /// </summary>
        /// <remarks>Set and used by the Process</remarks>
        internal Thread ExecutingThread
        {
            get
            {
                return executingThread;
            }
            set
            {
                executingThread = value;
            }
        }

        /// <summary>
        /// Set the wait handle (set by the executing process); 
        /// to be used by the worker to communicate back to the process that it has safely stoped
        /// </summary>
        /// <remarks>Process starting the worker will also set
        /// this EventWaitHandler</remarks>
        internal EventWaitHandle SetToSignalAbort
        {
            set
            {
                canBeAborted = value;
            }
        }

        /// <summary>
        /// Returns true if running, false otherwise
        /// </summary>
        public bool IsRunning
        {
            get
            {
                lock (this)
                {
                    return isRunning;
                }
            }
        }

        /// <summary>
        /// Get access to the worker threads list
        /// </summary>
        /// <remarks>Worker can create threads to carry out its tasks. Threads created are tracked 
        /// through this collection</remarks>
        protected List<Thread> WorkerThreads
        {
            get
            {
                return workerThreads;
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Create and initialize an instance of Worker
        /// </summary>
        public WorkerBase()
        {
            //Initialize worker with IsRunning = false
            isRunning = false;

            workerThreads = new List<Thread>();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Starts the worker to perform the job
        /// </summary>
        public void Start()
        {
            //When starting the worker
            lock (this)
            {
                //Set IsRunning = true
                //Do preprocessing i.e. Call to BeforeStart method) which will be different for different workers

                isRunning = true;
                BeforeStart(this.startParameters);
            }

            OnStart(this.startParameters);

            //thread now waits for stop signal
            signaledToStop.WaitOne();
            OnStop();
            canBeAborted.Set();
        }

        /// <summary>
        /// Provides mechanism to signal stop of the worker
        /// </summary>
        /// <remarks>When stopped, work at hand will not be stopped. 
        /// Only new tasks will not be picked. This method is called by the executing process thread,
        /// different from the thread on which the worker is being executed</remarks>
        public void Stop()
        {
            lock (this)
            {
                isRunning = false;
            }
            OnStop();
            signaledToStop.Set();
        }

        /// <summary>
        /// Called to do the threads clean-up
        /// </summary>
        internal void FinalStop()
        {
            //Do any clean-up or similar activity by calling the AfterStop method
            for (int index = 0; index < workerThreads.Count; index++)
            {
                try
                {
                    if (workerThreads[index].IsAlive)
                    {
                        workerThreads[index].Abort();
                    }
                }
                catch (ThreadAbortException exception)
                {
                    //Aborting still running thread
                }
            }

            workerThreads = null;

            AfterStop();
        }

        #endregion

        #region Protected Methods
        /// <summary>
        /// Method called just beforing starting the worker, 
        /// hook point for the Worker Start 
        /// </summary>
        /// <param name="parameters">Any parameters required</param>
        protected virtual void BeforeStart(Object parameters)
        {
            signaledToStop = new EventWaitHandle(false, EventResetMode.AutoReset);
        }

        /// <summary>
        /// Method called after stopping the worker, 
        /// hook point for any addition clean-up/release of resource.
        /// </summary>
        protected virtual void AfterStop()
        {
        }
        #endregion

        #region Event Handlers
        #endregion

        #region Private Methods
        /// <summary>
        /// Worker method to Start the Worker Instance
        /// </summary>
        /// <param name="parameters">Any parameters required</param>
        protected abstract void OnStart(Object parameters);

        /// <summary>
        /// Template method to stop the worker
        /// </summary>
        protected virtual void OnStop()
        {
        }
        #endregion
    }
}
