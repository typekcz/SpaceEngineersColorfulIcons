using System;
using Sandbox.ModAPI;
using VRage.Utils;
using ApiData = System.Collections.Generic.Dictionary<string, System.Delegate>;

namespace Sisk.ColorfulIcons.Api
{
    /// <summary>
    /// Publishes the Colorful Icons inter-mod API.
    /// </summary>
    internal sealed class ColorfulIconsApiServer
    {
        public const long WORKSHOP_ID = 801185519L;
        public const long REGISTRATION_CHANNEL = (WORKSHOP_ID << 8) | 1;

        public static Version ApiVersion => new Version(0, 1, 0);

        private readonly ApiData _apiData;
        private event Action ConfigChanged;
        private bool _registered;

        public ColorfulIconsApiServer(
            Func<string> getConfigXml,
            Func<byte[]> getConfigBinary)
        {
            if (getConfigXml == null)
                throw new ArgumentNullException(nameof(getConfigXml));

            if (getConfigBinary == null)
                throw new ArgumentNullException(nameof(getConfigBinary));

            _apiData = new ApiData
            {
                { "GetConfigVersion", new Func<Version>(GetConfigVersion) },
                { "GetConfig", getConfigXml },
                { "GetConfigBinary", getConfigBinary },
                { "SubscribeConfigChanged", new Action<Action>(SubscribeConfigChanged) },
                { "UnsubscribeConfigChanged", new Action<Action>(UnsubscribeConfigChanged) }
            };
        }

        public Version GetConfigVersion()
        {
            return ApiVersion;
        }

        public void Register()
        {
            if (_registered)
                return;

            MyAPIGateway.Utilities.RegisterMessageHandler(
                REGISTRATION_CHANNEL,
                OnApiRequest);

            _registered = true;
        }

        public void Close()
        {
            if (_registered)
            {
                MyAPIGateway.Utilities.UnregisterMessageHandler(
                    REGISTRATION_CHANNEL,
                    OnApiRequest);

                _registered = false;
            }

            ConfigChanged = null;
        }

        /// <summary>
        /// Notifies every subscribed client. One failing callback cannot prevent the
        /// other clients from receiving the notification.
        /// </summary>
        public void NotifyConfigChanged()
        {
            var handlers = ConfigChanged;
            if (handlers == null)
                return;

            foreach (Action handler in handlers.GetInvocationList())
            {
                try
                {
                    handler();
                }
                catch (Exception exception)
                {
                    MyLog.Default.Log(
                        MyLogSeverity.Warning,
                        $"[Colorful Icons API] A ConfigChanged callback failed: {exception}");
                }
            }
        }

        private void OnApiRequest(object payload)
        {
            if (!(payload is long))
                return;

            var replyChannel = (long)payload;
            if (replyChannel == REGISTRATION_CHANNEL)
                return;

            try
            {
                // Mod messages pass the object by reference. Give each requester its own
                // dictionary so a client cannot mutate the server's stored API table.
                MyAPIGateway.Utilities.SendModMessage(
                    replyChannel,
                    new ApiData(_apiData));
            }
            catch (Exception exception)
            {
                MyLog.Default.Log(
                    MyLogSeverity.Warning,
                    $"[Colorful Icons API] Failed to reply on channel {replyChannel}: {exception}");
            }
        }

        private void SubscribeConfigChanged(Action handler)
        {
            if (handler == null)
                return;

            ConfigChanged -= handler;
            ConfigChanged += handler;
        }

        private void UnsubscribeConfigChanged(Action handler)
        {
            if (handler != null)
                ConfigChanged -= handler;
        }
    }
}
