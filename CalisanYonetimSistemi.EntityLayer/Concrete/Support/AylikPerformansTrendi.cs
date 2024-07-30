namespace CalisanYonetimSistemi.EntityLayer.Concrete.Support
{
    public class AylikPerformansTrendi
    {

        public Dictionary<string, double?> Uretkenlik { get; set; } = new Dictionary<string, double?>();
        public Dictionary<string, double?> TakimCalismasi { get; set; } = new Dictionary<string, double?>();
        public Dictionary<string, double?> Analitiklik { get; set; } = new Dictionary<string, double?>();

    }
}
