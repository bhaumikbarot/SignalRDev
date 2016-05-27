using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace DevApplication.Hub
{
    public class DevHub : Microsoft.AspNet.SignalR.Hub
    {
        private readonly DevData _pointer;
        public DevHub() : this(DevData.Instance) { }
        public DevHub(DevData pointer)
        {
            pointer.hubData = this;
            _pointer = pointer;

        }
        public override System.Threading.Tasks.Task OnConnected()
        {
            try
            {
                _pointer.initData();
            }
            catch (Exception ex)
            {
                base.Dispose();
                return null;
            }
            return base.OnConnected();
        }

        public override System.Threading.Tasks.Task OnReconnected()
        {
            try
            {
                _pointer.initData();
            }
            catch (Exception ex)
            {
                base.Dispose();
                return null;
            }
            return base.OnReconnected();
        }

        public override System.Threading.Tasks.Task OnDisconnected(bool stopCalled)
        {
            return base.OnDisconnected(false);
        }
    }
}