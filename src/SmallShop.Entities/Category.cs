namespace SmallShop.Entities
{
    public class Category
    {
        public Category()
        {
            Goodss = new HashSet<Goods>();
            Title = string.Empty;
        }
        public int Id { get; set; }
        public string Title { get; set; }
        public HashSet<Goods> Goodss { get; set; }

    }
}
