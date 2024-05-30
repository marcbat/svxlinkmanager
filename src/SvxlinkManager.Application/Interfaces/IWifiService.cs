using SvxlinkManager.Domain.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SvxlinkManager.Application.Interfaces
{
    public interface IWifiService
    {
        List<Device> Devices { get; }

        void LoadDevices();

        void Connect(Device device);

        void Disconnect(Connection connection);

        void Down(Connection connection);

        void Up(Connection connection);
    }
}
