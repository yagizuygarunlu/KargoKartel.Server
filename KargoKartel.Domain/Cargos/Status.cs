using Ardalis.SmartEnum;

namespace KargoKartel.Server.Domain.Cargos
{
    public sealed class Status: SmartEnum<Status>
    {
        public static readonly Status Pending = new Status("Pending", 1);
        public static readonly Status InTransit = new Status("In Transit", 2);
        public static readonly Status Delivered = new Status("Delivered", 3);
        public static readonly Status Cancelled = new Status("Cancelled", 4);
        public static readonly Status Returned = new Status("Returned", 5);
        public static readonly Status Lost = new Status("Lost", 6);
        public static readonly Status Delayed = new Status("Delayed", 7);
        public static readonly Status Rescheduled = new Status("Rescheduled", 8);
        public static readonly Status OnHold = new Status("On Hold", 9);
        public static readonly Status AwaitingPickup = new Status("Awaiting Pickup", 10);
        public static readonly Status AwaitingDelivery = new Status("Awaiting Delivery", 11);
        public static readonly Status Exception = new Status("Exception", 12);
        public static readonly Status Unknown = new Status("Unknown", 13);
        public static readonly Status Expired = new Status("Expired", 14);
        public static readonly Status Rejected = new Status("Rejected", 15);
        public static readonly Status Scheduled = new Status("Scheduled", 16);
        public static readonly Status InReview = new Status("In Review", 17);
        public static readonly Status AwaitingConfirmation = new Status("Awaiting Confirmation", 18);
        public static readonly Status AwaitingPayment = new Status("Awaiting Payment", 19);
        public static readonly Status AwaitingApproval = new Status("Awaiting Approval", 20);
        public static readonly Status AwaitingProcessing = new Status("Awaiting Processing", 21);
        public static readonly Status AwaitingShipment = new Status("Awaiting Shipment", 22);
        public static readonly Status AwaitingReturn = new Status("Awaiting Return", 23);
        public static readonly Status AwaitingRefund = new Status("Awaiting Refund", 24);
        public static readonly Status AwaitingExchange = new Status("Awaiting Exchange", 25);

        private Status(string name, int value) : base(name, value)
        {
        }
    }
}

