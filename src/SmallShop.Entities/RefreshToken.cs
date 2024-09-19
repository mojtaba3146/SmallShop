namespace SmallShop.Entities
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public string Token { get; set; } = string.Empty;
        public string PRefreshToken { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool IsExpired => DateTime.Now >= ExpiryDate;
        public bool IsActive => !IsExpired;
        public string IpAddress { get; set; } = string.Empty;
        public int UserId { get; set; }
        public virtual User User { get; set; }
    }
}
