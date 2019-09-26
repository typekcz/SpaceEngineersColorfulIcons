# Space Engineers Mod Utils

Some libraries used by my other mods for Space Engineers.

* [Logging](#logging)
* [Localization](#localization)
* [Profiler](#profiler)
* [Net](#net)

## Logging

### Options

* bool LogOnEnterAndLeaveMethods
  * set if ILogger should create a log entry on `BeginMethod`, `EnterMethod` and `LeaveMethod` methods.

### ILogEventHandler

This library comes with three default `ILogEventHandler`.

* `GlobalStorageHandler` - log to a specified file in `%appdata%\SpaceEngineers\Storage`.
* `LocalStorageHandler` - log to a specified file in `%appdata%\SpaceEngineers\Storage\{CallingType}`.
* `WorldStorageHandler` - log to a specified file in `%appdata%\SpaceEngineers\Saves\{SteamUserId}\{World}\Storage\{CallingType}`.

### Usage

To create an `ILogger` you should call `Logger.ForScope<TScope>()` from your `MySessionComponentBase`.
You can then register a `ILogEventHandler` with `Log.Register(ILogEventHandler eventHandler)`

###### Example

```csharp
[MySessionComponentDescriptor(MyUpdateOrder.NoUpdate)]
public class Mod : MySessionComponentBase {
    private const string LOG_FILE_TEMPLATE = "{0}.log";
    public const string NAME = "Example Mod";
    private static readonly string LogFile = string.Format(LOG_FILE_TEMPLATE, NAME);

    public Mod() {
        Static = this;
        InitializeLogging();
    }

    /// <summary>
    ///     Logger used for logging.
    /// </summary>
    public ILogger Log { get; private set; }

    public static Mod Static { get; private set; }

    /// <summary>
    ///     Unloads all data.
    /// </summary>
    protected override void UnloadData() {
        Log?.EnterMethod(nameof(UnloadData));

        if (Log != null) {
            Log.Debug("Logging stopped");
            Log.Close();
            Log = null;
        }
    }

    /// <summary>
    ///     Initalize the logging system.
    /// </summary>
    private void InitializeLogging() {
        Log = Logger.ForScope<Mod>();
        Log.Register(new WorldStorageHandler(LogFile, LogFormatter, LogEventLevel.All));
        using (Log.BeginMethod(nameof(InitializeLogging))) {
            Log.Debug("Logging initialized");
        }
    }

    private static string LogFormatter(LogEventLevel level, string message, DateTime timestamp, Type scope, string method) {
        return $"[{timestamp:HH:mm:ss:fff}] [{new string(level.ToString().Take(1).ToArray())}] [{scope}->{method}()]: {message}";
    }
}
```

## Localization

Localization is removed with v1.6.3, because it's now possible to load resx files.

###### Example
This session component tries to load the localization resource files from `Data\Localization` folder on `LoadData` call.
The resx files should named `MyTexts.resx`, `MyTexts.{culture}.resx` like the vanilla ones.

```csharp
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Sandbox.ModAPI;
using VRage;
using VRage.Game.Components;

namespace Example {
    [MySessionComponentDescriptor(MyUpdateOrder.NoUpdate)]
    public class Mod : MySessionComponentBase {
        /// <summary>
        ///     Language used to localize this mod.
        /// </summary>
        public MyLanguagesEnum? Language { get; private set; }

        /// <summary>
        ///     Load mod data.
        /// </summary>
        public override void LoadData() {
            LoadLocalization();

            MyAPIGateway.Gui.GuiControlRemoved += OnGuiControlRemoved;
        }

        /// <summary>
        ///     Unloads mod data.
        /// </summary>
        protected override void UnloadData() {
            MyAPIGateway.Gui.GuiControlRemoved -= OnGuiControlRemoved;
        }

        /// <summary>
        ///     Load localizations for this mod.
        /// </summary>
        private void LoadLocalization() {
            var path = Path.Combine(ModContext.ModPathData, "Localization");
            var supportedLanguages = new HashSet<MyLanguagesEnum>();
            MyTexts.LoadSupportedLanguages(path, supportedLanguages);

            var currentLanguage = supportedLanguages.Contains(MyAPIGateway.Session.Config.Language) ? MyAPIGateway.Session.Config.Language : MyLanguagesEnum.English;
            if (Language != null && Language == currentLanguage) {
                return;
            }

            Language = currentLanguage;
            var languageDescription = MyTexts.Languages.Where(x => x.Key == currentLanguage).Select(x => x.Value).FirstOrDefault();
            if (languageDescription != null) {
                var cultureName = string.IsNullOrWhiteSpace(languageDescription.CultureName) ? null : languageDescription.CultureName;
                var subcultureName = string.IsNullOrWhiteSpace(languageDescription.SubcultureName) ? null : languageDescription.SubcultureName;

                MyTexts.LoadTexts(path, cultureName, subcultureName);
            }
        }

        /// <summary>
        ///     Event triggered on gui control removed.
        ///     Used to detect if Option screen is closed and then to reload localization.
        /// </summary>
        /// <param name="obj"></param>
        private void OnGuiControlRemoved(object obj) {
            if (obj.ToString().EndsWith("ScreenOptionsSpace")) {
                LoadLocalization();
            }
        }
    }
}
```

## Profiler

The profiler can be used to measure the execution time of your code.

### Usage

To use the profiler simply call the static class `Profiler`.

##### Measure

To start measure a code block you can youse `Profiler.Measure(string scope, string method)`.

###### Example

```csharp
public class DemoClass {
    public void SomeRandomMethod() {
        using (Profiler.Measure(nameof(DemoClass), nameof(SomeRandomMethod))) {
            // your code thats should be profiled.
        }
    }
}
```

##### Results

The property `Profiler.Result` is an `IEnumerable` with all profiler results.

###### Example

```csharp
private static void WriteProfileResults() {
    if (Profiler.Results.Any()) {
        using (var writer = MyAPIGateway.Utilities.WriteFileInLocalStorage("profiler.txt", typeof(Mod))) {
            foreach (var result in Profiler.Results) {
                writer.WriteLine(result);
                // Example line:
                // DemoClass.SomeRandomMethod():     600 executions, avg 0.099444ms, min 0.001500ms, max 29.107300ms, total 59.67ms
            }
        }
    }
}
```


##### SetLogger

If you want to log directly you can set an logger action with `Profiler.SetLogger(Action<string> logger)`.

###### Example

```csharp
public class DemoClass {
    public DemoClass() {
        Log = Logger.ForScope<DemoClass>();
        Profiler.SetLogger(Log.Debug);
    }
    public ILogger Log { get; }
    public void SomeRandomMethod() {
        using (Profiler.Measure(nameof(DemoClass), nameof(SomeRandomMethod))) {
            // your code thats should be profiled.
        }
    }
}
```

## Net

The Net library helps to send and receive Messages in multiplayer.

### Usage

First you have to create a new instance of `Network`.
It requires a unique network id. No other mod should use this id.

###### Example
```csharp
using ProtoBuf;
using Sandbox.ModAPI;
using Sisk.Utils.Net;
using Sisk.Utils.Net.Messages;
using VRage.Game;
using VRage.Game.Components;

namespace Demo {
    [MySessionComponentDescriptor(MyUpdateOrder.NoUpdate)]
    public class DemoSessionComponent : MySessionComponentBase {
        private const ushort NETWORK_ID = 45531;

        public Network Network { get; set; }

        public override void Init(MyObjectBuilder_SessionComponent sessionComponent) {
            base.Init(sessionComponent);

            Network = new Network(NETWORK_ID);
            if (Network.IsClient) {
                MyAPIGateway.Utilities.MessageEntered += OnMessageEntered;
                Network.Register<PongMessage>(OnPongMessageReceived);
            }

            if (Network.IsServer) {
                Network.Register<PingMessage>(OnPingMessageReceived);
            }
        }

        protected override void UnloadData() {
            if (Network != null) {
                if (Network.IsClient) {
                    MyAPIGateway.Utilities.MessageEntered -= OnMessageEntered;
                    Network.Unregister<PongMessage>(OnPongMessageReceived);
                }

                if (Network.IsServer) {
                    Network.Unregister<PingMessage>(OnPingMessageReceived);
                }
            }
        }

        private void OnMessageEntered(string messagetext, ref bool sendtoothers) {
            if (messagetext.ToLower().StartsWith("ping")) {
                sendtoothers = false;
                Network.SendToServer(new PingMessage());
            }
        }

        private void OnPingMessageReceived(ulong sender, PingMessage message) {
            Network.Send(new PongMessage(), sender);
        }

        private void OnPongMessageReceived(ulong sender, PongMessage message) {
            MyAPIGateway.Utilities.ShowMessage("server", "received pong response from server.");
        }
    }

    [ProtoContract]
    public class PingMessage : IMessage {
        public byte[] Serialze() {
            return MyAPIGateway.Utilities.SerializeToBinary(this);
        }
    }

    [ProtoContract]
    public class PongMessage : IMessage {
        public byte[] Serialze() {
            return MyAPIGateway.Utilities.SerializeToBinary(this);
        }
    }
}
```