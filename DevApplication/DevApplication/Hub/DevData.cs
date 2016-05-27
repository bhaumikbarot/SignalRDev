using Dev.BO.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Web;

namespace DevApplication.Hub
{
    public class DevData
    {
        private static Lazy<DevData> _instance = new Lazy<DevData>(() => new DevData());
        private readonly int _updateInterval = 1000; //ms        
        private bool _updatingData = false;
        private Timer _timer;
        private readonly object _updatePointsLock = new object();
        private int _startPoint = 50, _minPoint = 25, _maxPoint = 99;
        public bool _change = false;
        public DevHub hubData;
        public IEnumerable<DevTest> lstDevTest;
        SqlDependency dependency;
        public static DevData Instance
        {
            get
            {
                if (_instance.Value == null)
                    _instance = new Lazy<DevData>(() => new DevData());
                return _instance.Value;
            }
        }
        public DevData()
        {
            _timer = new Timer(TimerCallBack, null, _updateInterval, _updateInterval);
        }
        public void initData()
        {
            if (lstDevTest == null)
                lstDevTest = GetDevTestData();
            SignalBroadcastDevData(lstDevTest, hubData.Context.ConnectionId);
        }
        /// <summary>
        /// TimerCallBack function will call after every 1000ms
        /// </summary>
        /// <param name="state">null</param>
        private void TimerCallBack(object state)
        {
            // This function must be re-entrant as it's running as a timer interval handler
            if (_updatingData)
            {
                return;
            }
            lock (_updatePointsLock)
            {
                if (!_updatingData)
                {
                    _updatingData = true;
                    if (_change)//Connected User's data will update if "_change" value is true
                    {
                        lstDevTest = GetDevTestData();
                        BroadcastDevData(lstDevTest);
                        _change = false;
                    }
                    _updatingData = false;
                }
            }
        }
        /// <summary>
        /// SQL dependency change event.
        /// It will fire when any change in Inventory table
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dependency_OnChange(object sender, SqlNotificationEventArgs e)
        {
            SqlDependency dep = sender as SqlDependency;
            if (e.Type == SqlNotificationType.Change)
            {
                _change = true;
            }
        }

        /// <summary>
        /// GetData function get all data from Inverntory table
        /// </summary>
        /// <returns>Inventory List</returns>
        public IEnumerable<DevTest> GetDevTestData()
        {
            if (dependency != null)
                dependency.OnChange -= new OnChangeEventHandler(dependency_OnChange);
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DevContext"].ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("dbo.spGetDevDataList", connection))
                {
                    // Make sure the command object does not already have
                    // a notification object associated with it.
                    command.Notification = null;
                    command.CommandType = CommandType.StoredProcedure;
                    dependency = new SqlDependency(command);
                    dependency.OnChange += new OnChangeEventHandler(dependency_OnChange);


                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        return reader.Cast<IDataRecord>()
                            .Select(x => new DevTest()
                            {
                                CampaignName = x.GetString(0),
                                Date = x.GetDateTime(1),
                                Clicks = x.GetInt32(2),
                                Conversions = x.GetInt32(3),
                                Impressions = x.GetInt32(4),
                                AffiliateName = x.GetString(5)
                            }).ToList();
                    }
                }
            }
        }
        private dynamic GetClients()
        {
            return hubData.Clients.All;
        }
        private void BroadcastDevData(IEnumerable<DevTest> point)
        {
            GetClients().updateData(point);
        }
        private dynamic GetSingelClients(string connectionId)
        {
            return hubData.Clients.Client(connectionId);
        }
        private void SignalBroadcastDevData(IEnumerable<DevTest> point, string connectionId)
        {
            GetSingelClients(connectionId).updateData(point);
        }
    }
}