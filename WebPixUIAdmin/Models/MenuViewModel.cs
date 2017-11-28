namespace WebPixUIAdmin.Models
{
    public class MenuViewModel : BaseModel
    {
        public string Url { get; set; }
        public int Pai { get; set; }
        public int Tipo { get; set; }
        public string Ativo { get; set; }
    }
}