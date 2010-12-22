using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.ServiceModel;

using Open.MOF.Messaging.Callback.WcfService;

namespace Open.MOF.Messaging.Callback
{
    public class WcfCallbackHost : ICallbackHost
    {
        private HandleCallbackDelegate _hadleCallbackDelegate;
        private bool _isServiceConfigured;
        private bool _isServiceRunning;
        private bool _isServiceStarting;
        private AutoResetEvent _startFlag;
        private AutoResetEvent _stopFlag;
        private Exception _startupException;
        private ServiceHost _serviceHost;
        private string _endpointUri;

        public WcfCallbackHost()
        {
            _hadleCallbackDelegate = null;
            _isServiceConfigured = false;
            _isServiceRunning = false;
            _endpointUri = null;
        }

        #region ICallbackHost Members

        public bool IsServiceRunning
        {
            get { return _isServiceRunning; }
        }

        public string EndpointUri
        {
            get { return _endpointUri; }
        }

        public void InitializeCallbackHost(HandleCallbackDelegate callbackDelegate)
        {
            _hadleCallbackDelegate = callbackDelegate;
            Start();
        }

        public void Dispose()
        {
            Stop();
        }

        #endregion

        private void MessageReceivedHandler(object sender, MessageReceivedEventArgs args)
        {
            if (_hadleCallbackDelegate != null)
                _hadleCallbackDelegate(sender, args.Message);
        }

        private void Start()
        {
            if (_isServiceRunning)
                return;

            if (_isServiceStarting)
            {
                while (_isServiceStarting)
                    Thread.Sleep(500);

                if (_isServiceRunning)
                    return;
            }

            _startFlag = new System.Threading.AutoResetEvent(false);
            _stopFlag = new System.Threading.AutoResetEvent(false);
            _startupException = null;

            System.Threading.Thread threadCallback = new System.Threading.Thread(RunServiceHost);
            threadCallback.IsBackground = true;
            threadCallback.Start();

            _startFlag.WaitOne();                    // wait for the service to start
            if (_startupException != null)
            {
                throw new ApplicationException("The Callback Service failed to start.  See InnerException for details.", _startupException);
            }
            System.Threading.Thread.Sleep(100);     // wait another moment
        }

        private void Stop()
        {
            _isServiceRunning = false;
            _isServiceStarting = false;

            _stopFlag.Set();
            while (_serviceHost.State != CommunicationState.Closed)
            {
                System.Threading.Thread.Sleep(100);
            }
        }

        private void RunServiceHost()
        {
            // This method should be running on a background thread all the while the application is running

            _isServiceStarting = true;
            try
            {
                WcfMessagingCallbackService serviceInstance = new WcfMessagingCallbackService();
                serviceInstance.MessageReceived += new EventHandler<MessageReceivedEventArgs>(MessageReceivedHandler);
                _serviceHost = new ServiceHost(serviceInstance);
                _serviceHost.Open();

                if (_serviceHost.ChannelDispatchers.Count > 0)
                    _endpointUri = _serviceHost.ChannelDispatchers[0].Listener.Uri.AbsoluteUri;
            }
            catch (Exception ex)
            {
                // If there was an exeption shutdown
                _isServiceRunning = false;
                _isServiceStarting = false;
                _startupException = ex;
                if (_serviceHost.State == CommunicationState.Opened)
                {
                    _serviceHost.Close();
                }
                _startFlag.Set();

                return;
            }

            // If we get here, the service is running
            _isServiceStarting = false;
            _isServiceRunning = true;
            _startFlag.Set();

            // The service is running, now wait for a signal to stop
            _stopFlag.WaitOne();

            if (_serviceHost.State == CommunicationState.Opened)
                _serviceHost.Close();
            _isServiceRunning = false;
        }
    }
}
