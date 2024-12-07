using LanguageExt;
using LanguageExt.Common;

namespace SvxlinkManager.Domain.Entities
{
    public class SvxlinkChannel : Channel
    {
        private string reportCallSign;

        public SvxlinkChannel()
        {
            
        }

        internal SvxlinkChannel(Guid id, string name, string host, int port, string callSign, string authKey, string reportCallSign) : base(id, name, host, callSign)
        {
            AuthKey = authKey;
            Port = port;
            this.reportCallSign = reportCallSign;
        }

        public static Validation<Error, SvxlinkChannel> Create(Guid id, string name, string host, int port, string callSign, string authKey, string reportCallSign)
        {
            return (ValidateName(name), ValidateCallSign(callSign), ValidateReportCallSign(reportCallSign), ValidateHost(host))
                .Apply((vname, vcallSign, vreportCallSign, vhost) => new SvxlinkChannel(id, vname, vhost, port, vcallSign, authKey, vreportCallSign));
        }

        public string? AuthKey { get; set; }

        public int Port { get; set; }

        public string ReportCallSign => reportCallSign;

        public Validation<Error, string> SetReportCallSign(string reportCallSign)
        {
            return ValidateReportCallSign(reportCallSign).Map(v => this.reportCallSign = v);
        }

        public static Validation<Error, string> ValidateReportCallSign(string reportCallSign)
        {
            if (string.IsNullOrWhiteSpace(reportCallSign))
                return Error.New("Le nom du reportCallSign ne peut pas être vide.");

            return reportCallSign;
        }
    }
}
