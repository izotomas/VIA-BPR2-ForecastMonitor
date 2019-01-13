namespace ForecastMonitor.Service.AppSettings
{
    public class Installation
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public int Id { get; set; }
        public override string ToString()
        {
            return $"Name: {Name} | ID: {Id} | URL: {Url}";
        }
    }
}
