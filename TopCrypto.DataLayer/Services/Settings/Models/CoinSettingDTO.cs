namespace TopCrypto.DataLayer.Services.Settings.Models
{
    public class CoinSettingDTO
    {
        public string LastSavedCoinId { get; set; }
        public int? LastTimeType { get; set; }

        //For testing
        public override string ToString()
        {
            return LastSavedCoinId + "  " + LastTimeType;
        }
    }
}
