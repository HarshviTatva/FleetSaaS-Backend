using CsvHelper;
using System.Globalization;
using System.Text;

namespace FleetSaaS.Domain.Helper
{
    public static class CsvExportHelper
    {
        public static byte[] GenerateCsv<T>(IEnumerable<T> data)
        {
            MemoryStream ms = new MemoryStream();
            StreamWriter writer = new StreamWriter(ms, new UTF8Encoding(true));
            CsvWriter csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
            csv.WriteHeader<T>();
            csv.NextRecord();      
            csv.NextRecord();      
            csv.WriteRecords(data);
            writer.Flush();
            return ms.ToArray();
        }
    }
}
