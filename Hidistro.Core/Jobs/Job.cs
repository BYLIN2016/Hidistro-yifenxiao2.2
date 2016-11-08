namespace Hidistro.Core.Jobs
{
    using System;
    using System.Globalization;
    using System.Threading;
    using System.Xml;
    using System.Xml.Serialization;

    [Serializable, XmlRoot("job")]
    public class Job : IDisposable
    {
        private bool _enabled = true;
        private bool _enableShutDown = false;
        private IJob _ijob;
        private bool _isRunning;
        private Type _jobType;
        private DateTime _lastEnd;
        private DateTime _lastStart;
        private DateTime _lastSucess;
        private int _minutes = 15;
        private string _name;
        [NonSerialized]
        private XmlNode _node = null;
        private int _seconds = -1;
        private bool _singleThread = true;
        [NonSerialized]
        private Timer _timer = null;
        private bool disposed = false;

        public Job(Type ijob, XmlNode node)
        {
            if (node != null)
            {
                this._node = node;
                this._jobType = ijob;
                XmlAttribute attribute = node.Attributes["enabled"];
                if (attribute != null)
                {
                    this._enabled = bool.Parse(attribute.Value);
                }
                attribute = node.Attributes["enableShutDown"];
                if (attribute != null)
                {
                    this._enableShutDown = bool.Parse(attribute.Value);
                }
                attribute = node.Attributes["name"];
                if (attribute != null)
                {
                    this._name = attribute.Value;
                }
                attribute = node.Attributes["seconds"];
                if (attribute != null)
                {
                    this._seconds = int.Parse(attribute.Value, CultureInfo.InvariantCulture);
                }
                attribute = node.Attributes["minutes"];
                if (attribute != null)
                {
                    try
                    {
                        this._minutes = int.Parse(attribute.Value, CultureInfo.InvariantCulture);
                    }
                    catch
                    {
                        this._minutes = 15;
                    }
                }
                attribute = node.Attributes["singleThread"];
                if (((attribute != null) && !string.IsNullOrEmpty(attribute.Value)) && (string.Compare(attribute.Value, "false", false, CultureInfo.InvariantCulture) == 0))
                {
                    this._singleThread = false;
                }
            }
        }

        public IJob CreateJobInstance()
        {
            if (this.Enabled && (this._ijob == null))
            {
                if (this._jobType != null)
                {
                    this._ijob = Activator.CreateInstance(this._jobType) as IJob;
                }
                this._enabled = this._ijob != null;
                if (!this._enabled)
                {
                    this.Dispose();
                }
            }
            return this._ijob;
        }

        public void Dispose()
        {
            if ((this._timer != null) && !this.disposed)
            {
                lock (this)
                {
                    this._timer.Dispose();
                    this._timer = null;
                    this.disposed = true;
                }
            }
        }

        public void ExecuteJob()
        {
            this._isRunning = true;
            IJob job = this.CreateJobInstance();
            if (job != null)
            {
                this._lastStart = DateTime.Now;
                try
                {
                    job.Execute(this._node);
                    this._lastEnd = this._lastSucess = DateTime.Now;
                }
                catch (Exception)
                {
                    this._enabled = !this.EnableShutDown;
                    this._lastEnd = DateTime.Now;
                }
            }
            this._isRunning = false;
        }

        public void InitializeTimer()
        {
            if ((this._timer == null) && this.Enabled)
            {
                this._timer = new Timer(new TimerCallback(this.timer_Callback), null, this.Interval, this.Interval);
            }
        }

        private void timer_Callback(object state)
        {
            if (this.Enabled)
            {
                this._timer.Change(-1, -1);
                this.ExecuteJob();
                if (this.Enabled)
                {
                    this._timer.Change(this.Interval, this.Interval);
                }
                else
                {
                    this.Dispose();
                }
            }
        }

        public bool Enabled
        {
            get
            {
                return this._enabled;
            }
        }

        public bool EnableShutDown
        {
            get
            {
                return this._enableShutDown;
            }
        }

        protected int Interval
        {
            get
            {
                if (this._seconds > 0)
                {
                    return (this._seconds * 0x3e8);
                }
                return (this.Minutes * 0xea60);
            }
        }

        public bool IsRunning
        {
            get
            {
                return this._isRunning;
            }
        }

        public Type JobType
        {
            get
            {
                return this._jobType;
            }
        }

        public DateTime LastEnd
        {
            get
            {
                return this._lastEnd;
            }
        }

        public DateTime LastStarted
        {
            get
            {
                return this._lastStart;
            }
        }

        public DateTime LastSuccess
        {
            get
            {
                return this._lastSucess;
            }
        }

        public int Minutes
        {
            get
            {
                return this._minutes;
            }
            set
            {
                this._minutes = value;
            }
        }

        public string Name
        {
            get
            {
                return this._name;
            }
        }

        public bool SingleThreaded
        {
            get
            {
                return this._singleThread;
            }
        }
    }
}

