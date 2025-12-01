namespace Pulpa.TrainerSafety.Data.Entities
{
    public class CampaignResult : BaseEntity, IBaseEntity
    {
        public int CampaignResultId { get; set; }      
        public virtual Campaign? Campaign { get; set; }      
        public virtual Usuario Usuario { get; set; }
        public DateTime? EmailOpenedAt { get; set; }
        public DateTime? LinkClickedAt { get; set; }
        public DateTime? DataSubmittedAt { get; set; }
        public string? ClickedLinkUrl { get; set; }
        public string? SubmittedData { get; set; }
        public ResultType Result { get; set; }
        public int Score { get; set; }
    }
     

    public enum ResultType
    {
        Unknown = 0,
        NotDelivered,
        DeliveredNotOpened,
        OpenedNoClick,
        ClickedNoSubmit,
        SubmittedData,
        ReportedAsPhishing
    }

}
