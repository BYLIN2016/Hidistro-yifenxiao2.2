namespace Hidistro.Core.Jobs
{
    using Hidistro.Core.Configuration;
    using System;
    using System.Collections;
    using System.Collections.Specialized;
    using System.Globalization;
    using System.Threading;
    using System.Xml;

    public class Jobs : IDisposable
    {
        private DateTime _completed;
        private DateTime _created = DateTime.Now;
        private static int _instancesOfParent = 0;
        private bool _isRunning;
        private static readonly Hidistro.Core.Jobs.Jobs _jobs = null;
        private DateTime _started;
        private bool disposed = false;
        private int Interval = 0xdbba0;
        private Hashtable jobList = new Hashtable();
        private Timer singleTimer = null;

        static Jobs()
        {
            _jobs = new Hidistro.Core.Jobs.Jobs();
        }

        private Jobs()
        {
        }

        private void call_back(object state)
        {
            this._isRunning = true;
            this._started = DateTime.Now;
            this.singleTimer.Change(-1, -1);
            foreach (Job job in this.jobList.Values)
            {
                if (job.Enabled)
                {
                    job.ExecuteJob();
                }
            }
            this.singleTimer.Change(this.Interval, this.Interval);
            this._isRunning = false;
            this._completed = DateTime.Now;
        }

        public void Dispose()
        {
            if ((this.singleTimer != null) && !this.disposed)
            {
                lock (this)
                {
                    this.singleTimer.Dispose();
                    this.singleTimer = null;
                    this.disposed = true;
                }
            }
        }

        public static Hidistro.Core.Jobs.Jobs Instance()
        {
            return _jobs;
        }

        public bool IsJobEnabled(string jobName)
        {
            if (!this.jobList.Contains(jobName))
            {
                return false;
            }
            return ((Job) this.jobList[jobName]).Enabled;
        }

        public void Start()
        {
            Interlocked.Increment(ref _instancesOfParent);
            lock (this.jobList.SyncRoot)
            {
                if (this.jobList.Count == 0)
                {
                    XmlNode configSection = HiConfiguration.GetConfig().GetConfigSection("Hishop/Jobs");
                    bool flag = true;
                    XmlAttribute attribute = configSection.Attributes["singleThread"];
                    if (((attribute != null) && !string.IsNullOrEmpty(attribute.Value)) && (string.Compare(attribute.Value, "false", true, CultureInfo.InvariantCulture) == 0))
                    {
                        flag = false;
                    }
                    XmlAttribute attribute2 = configSection.Attributes["minutes"];
                    if ((attribute2 != null) && !string.IsNullOrEmpty(attribute2.Value))
                    {
                        int result = 1;
                        if (int.TryParse(attribute2.Value, out result))
                        {
                            this.Interval = result * 0xea60;
                        }
                    }
                    foreach (XmlNode node2 in configSection.ChildNodes)
                    {
                        if ((configSection.NodeType != XmlNodeType.Comment) && (node2.NodeType != XmlNodeType.Comment))
                        {
                            XmlAttribute attribute3 = node2.Attributes["type"];
                            XmlAttribute attribute4 = node2.Attributes["name"];
                            Type ijob = Type.GetType(attribute3.Value);
                            if ((ijob != null) && !this.jobList.Contains(attribute4.Value))
                            {
                                Job job = new Job(ijob, node2);
                                if (flag && job.SingleThreaded)
                                {
                                    job.InitializeTimer();
                                }
                                else
                                {
                                    this.jobList[attribute4.Value] = job;
                                }
                            }
                        }
                    }
                    if (this.jobList.Count > 0)
                    {
                        this.singleTimer = new Timer(new TimerCallback(this.call_back), null, this.Interval, this.Interval);
                    }
                }
            }
        }

        public void Stop()
        {
            Interlocked.Decrement(ref _instancesOfParent);
            if ((_instancesOfParent <= 0) && (this.jobList != null))
            {
                lock (this.jobList.SyncRoot)
                {
                    foreach (Job job in this.jobList.Values)
                    {
                        job.Dispose();
                    }
                    this.jobList.Clear();
                    if (this.singleTimer != null)
                    {
                        this.singleTimer.Dispose();
                        this.singleTimer = null;
                    }
                }
            }
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "Created: {0}, LastStart: {1}, LastStop: {2}, IsRunning: {3}, Minutes: {4}", new object[] { this._created, this._started, this._completed, this._isRunning, this.Interval / 0xea60 });
        }

        public Hashtable CurrentJobs
        {
            get
            {
                return this.jobList;
            }
        }

        public ListDictionary CurrentStats
        {
            get
            {
                ListDictionary dictionary = new ListDictionary();
                dictionary.Add("Created", this._created);
                dictionary.Add("LastStart", this._started);
                dictionary.Add("LastStop", this._completed);
                dictionary.Add("IsRunning", this._isRunning);
                dictionary.Add("Minutes", this.Interval / 0xea60);
                return dictionary;
            }
        }
    }
}

