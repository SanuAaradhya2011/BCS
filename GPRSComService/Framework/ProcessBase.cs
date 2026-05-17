using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace GPRSComService.Framework
{
    public abstract class ProcessBase
    {
        #region Nested Types
        /// <summary>
        /// Represents the state of the Process
        /// </summary>
        private enum ProcessState
        {
            /// <summary>
            /// Represents the state of the process indicating
            /// process is not yet initialized, default state
            /// </summary>
            Uninitialized,

            /// <summary>
            /// Represents the state of the process indicating
            /// process is initialized and can be started/run
            /// </summary>
            Initialized,

            /// <summary>
            /// Represents the state of the process indicating
            /// process is started and running
            /// </summary>
            Running,

            /// <summary>
            /// Represents the state of the process indicating
            /// process is stopped.
            /// </summary>
            /// <remarks>When re-run/restarted, will re-initial itself automatically</remarks>
            Stopped
        }
        #endregion

        #region Constants and Variables
        private ProcessState state;
        private List<WorkerBase> workers;
        private List<EventWaitHandle> stopSignals;
        private const int DEFAULT_THREADPOOL_SIZE = 5;

        #endregion

        #region Properties
        /// <summary>
        /// Returns true the process is running, false otherwise
        /// </summary>
        public bool IsRunning
        {
            get
            {
                lock (this)
                {
                    bool isRunning = false;
                    isRunning = state == ProcessState.Running;
                    return isRunning;
                }
            }
        }

        /// <summary>
        /// Time span (in seconds) the process will wait before aborting the worker threads
        /// </summary>
        abstract protected Int16 WaitIntervalBeforeAbort
        {
            get;
        }

        /// <summary>
        /// Get or set the state of the process
        /// </summary>
        /// <remarks></remarks>
        private ProcessState State
        {
            get
            {
                lock (this)
                {
                    return state;
                }
            }
            set
            {
                lock (this)
                {
                    state = value;
                }
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Creates and initializes an instance of the type ProcessBase
        /// </summary>
        public ProcessBase()
        {
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Starts/Run the process
        /// </summary>
        public void Start()
        {
            if (this.State == ProcessState.Uninitialized)
            {
                Initialize();
            }

            OnStart();
        }

        /// <summary>
        /// Stops the Process
        /// </summary>
        public void Stop()
        {
            this.State = ProcessState.Stopped;
            OnStop();
        }
        #endregion

        #region Protected Methods
        /// <summary>
        /// Add a new worker to the process' workers collection
        /// </summary>
        /// <param name="worker">New worker to be added</param>
        protected void AddWorker(WorkerBase worker)
        {
            workers.Add(worker);
        }

        /// <summary>
        /// Create process workers.
        /// </summary>
        /// <remarks>All sub-classes are expected to 
        /// create their necessary workers by overriding this method.
        /// Sub-classes will use the protected method AddWorker to add 
        /// a new worker for the process</remarks>
        protected abstract void CreateWorkers();
        #endregion

        #region Event Handlers
        #endregion

        #region Private Methods
        /// <summary>
        /// Initialises the Process, 
        /// 1. Add workers to the workers collection
        /// 2. Set the process state to Initialized
        /// </summary>
        private void Initialize()
        {

            workers = new List<WorkerBase>();
            stopSignals = new List<EventWaitHandle>();

            CreateWorkers();
            this.State = ProcessState.Initialized;
        }

        /// <summary>
        /// Worker method for the Starting/Running the Process
        /// </summary>
        /// <remarks>
        /// 1. For each worker, create a new thread, sets the
        ///    worker's start method as ThreadStart delegate 
        /// 2. Set the worker's Executing Thread to newly created thread
        /// 3. Sets the wiathandle on the worker, used to signal stop
        /// 4. Starts the newly created thread</remarks>
        private void OnStart()
        {
            int workersCount = workers.Count;

            for (int workerIndexer = 0; workerIndexer < workersCount; workerIndexer++)
            {
                //1.
                WorkerBase worker = workers[workerIndexer];
                Thread workerThread = new Thread(worker.Start);
                workerThread.Name = worker.GetType().Name;

                //2.
                worker.ExecutingThread = workerThread;

                //3.
                EventWaitHandle stopSignal = new EventWaitHandle(false, EventResetMode.AutoReset);
                worker.SetToSignalAbort = stopSignal;
                stopSignals.Add(stopSignal);

                //4.
                workerThread.Start();
            }

            this.State = ProcessState.Running;
        }

        /// <summary>
        /// Performs the clean-up operations required when process is stopped
        /// </summary>
        private void OnStop()
        {
            if (workers != null)
            {
                int workersCount = workers.Count;

                //requests all the workers to stop
                if (workersCount > 0)
                {
                    for (int workerIndexer = 0; workerIndexer < workersCount; workerIndexer++)
                    {
                        WorkerBase worker = workers[workerIndexer];
                        worker.Stop();
                    }

                    //wait for stop signals from the workers 
                    EventWaitHandle[] threadStoppedSignals = new EventWaitHandle[stopSignals.Count];
                    stopSignals.CopyTo(threadStoppedSignals);
                    WaitHandle.WaitAll(threadStoppedSignals, this.WaitIntervalBeforeAbort * 1000, false);

                    //abort the worker thread if still running
                    for (int workerIndexer = 0; workerIndexer < workersCount; workerIndexer++)
                    {
                        WorkerBase worker = workers[workerIndexer];

                        try
                        {
                            worker.ExecutingThread.Abort();
                        }
                        catch (ThreadAbortException abortException)
                        {
                            //worker thread aborted
                        }

                        //finally, cleanup the worker
                        worker.FinalStop();
                        IDisposable disposableWorker = worker as IDisposable;
                        if (disposableWorker != null)
                        {
                            disposableWorker.Dispose();
                        }
                    }
                }

                stopSignals = null;
                workers = null;
            }

            //Finally, set the process state to 'Uninitialized' so that it can be made to run again
            this.State = ProcessState.Uninitialized;

        }
        #endregion
    }
}
