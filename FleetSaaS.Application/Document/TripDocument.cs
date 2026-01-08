using FleetSaaS.Application.DTOs.Response;
using FleetSaaS.Domain.Enum;
using FleetSaaS.Infrastructure.Common;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace FleetSaaS.Domain.Document
{
    public class TripDocument : IDocument
    {
        private readonly TripPdfDTO _trip;
        public TripDocument(TripPdfDTO trip) 
        {
            _trip = trip;
        }

        public static readonly Color PrimaryRed = Colors.Red.Darken2;
        public static readonly Color RoseLight = Colors.Red.Lighten5;
        public static readonly Color BorderGrey = Colors.Grey.Lighten2;
        public static readonly Color TextDark = Colors.Grey.Darken4;

        public DocumentMetadata GetMetadata()
        => DocumentMetadata.Default;


        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(30);
                page.DefaultTextStyle(x => x.FontSize(10));
                page.Content().Element(ComposeOuterBox);
            });
        }

        void ComposeOuterBox(IContainer container)
        {
            container
                .Border(2)
                .BorderColor(PrimaryRed)
                .Padding(20)
                .Column(col =>
                {
                    col.Spacing(15);

                    col.Item().Element(ComposeHeader);

                    col.Item().AlignCenter()
                        .Text("Trip Detail Information")
                        .FontSize(13)
                        .Bold()
                        .FontColor(PrimaryRed);

                    col.Item().Element(ComposeTable);

                    col.Item().AlignRight()
                        .Text($"Generated On: {DateFormat.CURRENT_DATE}")
                        .FontSize(9)
                        .FontColor(Colors.Grey.Darken2);
                });
        }

        void ComposeHeader(IContainer container)
        {
            container.Column(col =>
            {
                col.Item().Text(SystemConstant.PROJECT_NAME)
                    .FontSize(20)
                    .Bold()
                    .FontColor(PrimaryRed);

                col.Item().LineHorizontal(1)
                    .LineColor(PrimaryRed);
            });
        }

        void ComposeTable(IContainer container)
        {
            container.Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.ConstantColumn(200);
                    columns.RelativeColumn();
                });

                void Row(string label, string value)
                {
                    table.Cell().Element(c => LabelCell(c, label));
                    table.Cell().Element(c => ValueCell(c, value));
                }

                Row("Trip Code", _trip.TripCode);
                Row("Route", $"{_trip.Origin} → {_trip.Destination}");
                Row("Description", _trip.Description);
                Row("Scheduled At", FormatDate(_trip.ScheduledAt));
                Row("Status", _trip.StatusName);
                Row("Driver", _trip.DriverName ?? "--");
                Row("Vehicle License Plate", _trip.VehicleNumber ?? "--");
                Row((_trip.Status==TripStatus.Completed?"Completed At":"Cancelled At"), FormatDate(_trip.Completed_At) ?? "--");
                Row("Vehicle Name", _trip.Make + '-' + _trip.Model  ?? "--");
                Row("Distance Covered", $"{_trip.DistanceCovered??0} km");
                if (_trip.Status == TripStatus.Cancelled)
                {
                    Row("Cancel Reason", _trip.CancelReason ?? "--");
                }
            });
        }

        void LabelCell(IContainer container, string text)
        {
            container
                .Border(1)
                .BorderColor(BorderGrey)
                .Background(RoseLight)
                .Padding(8)
                .Text(text)
                .SemiBold()
                .FontColor(PrimaryRed);
        }

        void ValueCell(IContainer container, string text)
        {
            container
                .Border(1)
                .BorderColor(BorderGrey)
                .Padding(8)
                .Text(text)
                .FontColor(TextDark);
        }

        string FormatDate(string? dateValue)
        {
            if (string.IsNullOrWhiteSpace(dateValue))
                return "--";

            if (DateTimeOffset.TryParse(dateValue, out var dto))
            {
                return dto.ToString(DateFormat.IST_DATE);
            }

            return dateValue;
        }

    }
}
