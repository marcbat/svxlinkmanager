using NUnit.Framework;

using SvxlinkManager.Domain.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SvxlinkManager.Domain.Entities.Tests
{
    [TestFixture()]
    public class SvxlinkChannelTests
    {
        [Test()]
        public void CreateTest()
        {
            // Arrange
            var sut = SvxlinkChannel.Create(Guid.NewGuid(), "name", "", 0, "callSign", "authKey", "");
        }
    }
}