using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace AutoPogo
{
    [TestFixture]
    public class TestPogoDriver
    {
        [Test]
        public void DriveShouldSendFile()
        {
            string file = "test.jpg";

            var driver = new PogoDriver("6000");
            driver.ProcessFile(file);
        }
    }
}
