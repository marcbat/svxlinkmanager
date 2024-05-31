namespace SvxlinkManager.Domain.Entities
{
    public class SvxlinkChannel : Channel
    {
        private string reportCallSign;

        public SvxlinkChannel(Guid id, string name, string host, int port, string callSign, string authKey, string reportCallSign) : base(id, name, host, callSign)
        {
            AuthKey = authKey;
            Port = port;
            ReportCallSign = reportCallSign;
        }

        public string? AuthKey { get; set; }

        public int Port { get; set; }

        public string ReportCallSign
        {
            get => reportCallSign; set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Le nom du reportCallSign ne peut pas être vide.", nameof(value));
                }

                reportCallSign = value;
            }
        }
    }
}
