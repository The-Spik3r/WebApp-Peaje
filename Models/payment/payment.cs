namespace Peajes.Models.payment
{
    public class payment
    {
        public int id { get; set; }

        public string matricula { get; set; }

        public bool pagado { get; set; }

        public int IdVehiculo { get; set; }

        public string CreateAt { get; set; }
    }
}
