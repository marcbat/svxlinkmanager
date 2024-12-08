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
    public class RadioProfilTests
    {
        [TestCase("67", "67")]
        [TestCase("", "")]
        [TestCase(null, null)]
        public async Task RadioProfil_WhenCreate_ShouldCreateRadioProfil(string txCtcss, string rxCtcss)
        {
            // act
            var result = RadioProfil.Create(Guid.NewGuid(), "RadioProfil", "145.500", "145.500", "1", txCtcss, rxCtcss, "50", "1", "1", "1", "1");

            // assert
            await Verify(result);
        }
    }
}