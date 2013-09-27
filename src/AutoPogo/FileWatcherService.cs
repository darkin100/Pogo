using System;
using System.IO;
using InTheHand.Net;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Sockets;

namespace AutoPogo
{
    public class FileWatcherService
    {
        private readonly string _watchFolder;
        private readonly PogoDriver _driver;

        public FileWatcherService(string watchFolder,PogoDriver driver )
        {
            _watchFolder = watchFolder;
            _driver = driver;
        }

        public void Start()
        {
            var watcher = new FileSystemWatcher();
            watcher.Path = _watchFolder;

            /* Watch for changes in LastAccess and LastWrite times, and the renaming of files or directories. */
            watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            // Only watch text files.
            watcher.Filter = "*.jpg";

            // Add event handlers.
            watcher.Changed += new FileSystemEventHandler(OnChanged);
            watcher.Created += new FileSystemEventHandler(OnChanged);

            // Begin watching.
            watcher.EnableRaisingEvents = true;
        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            _driver.ProcessFile(e.FullPath);
        }

        

        public void Stop()
        {
        }
    }

    public class PogoDriver
    {
        private string _pin;

        public PogoDriver(string pin)
        {
            _pin = pin;
        }

        public void ProcessFile(string filePath)
        {
             var client = new BluetoothClient();
            var devices = client.DiscoverDevices();

            foreach (var device in devices)
            {
                Console.WriteLine(device.DeviceName);

                if (device.DeviceName == "Polaroid 25 25 53")
                {                    
                    device.SetServiceState(BluetoothService.ObexObjectPush, true);

                    if (!device.Authenticated)
                    {
                        // Use pin "0000" for authentication
                        if (!BluetoothSecurity.PairRequest(device.DeviceAddress, _pin))
                        {
                            Console.WriteLine("Pairing failed");
                            return;
                        }
                    }

                    var uri = new Uri("obex://" + device.DeviceAddress + "/" + filePath);
                    var request = new ObexWebRequest(uri);
                    request.ReadFile(filePath);
                    var response = (ObexWebResponse) request.GetResponse();
                    Console.WriteLine(response.StatusCode.ToString());
                    // check response.StatusCode
                    response.Close();
                }
            }
        }
    }




}