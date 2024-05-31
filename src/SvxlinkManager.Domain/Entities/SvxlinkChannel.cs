namespace SvxlinkManager.Domain.Entities
{
    public class SvxlinkChannel(Guid id, string name, string host, int port, string callSign, string authKey, string reportCallSign) : Channel(id, name, host, callSign)
    {
        public string? AuthKey { get; } = authKey;

        public int Port { get; } = port;

        public string ReportCallSign { get; set; } = reportCallSign;
    }
}
