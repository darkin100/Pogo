using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace AutoPogo
{
    class Program
    {
        static void Main(string[] args)
        {
            string pin = ConfigurationManager.AppSettings["pin"];
            string watchFolder = ConfigurationManager.AppSettings["watchfolder"];

            HostFactory.Run(x =>                                 //1
            {
                x.Service<FileWatcherService>(s =>                        //2
                {
                    s.ConstructUsing(name => new FileWatcherService(watchFolder,new PogoDriver(pin)));     //3
                    s.WhenStarted(tc => tc.Start());              //4
                    s.WhenStopped(tc => tc.Stop());               //5
                });
                x.RunAsLocalSystem();                            //6

                x.SetDescription("Pogo Driver");         //7
                x.SetDisplayName("PogoDriver");                       //8
                x.SetServiceName("PogoDriver");                       //9
            });   
        }
    }
}
