namespace SmallShop.Entities
{
    public class Category
    {
        public Category()
        {
            Goodss = new HashSet<Goods>();
        }
        public int Id { get; set; }
        public string Title { get; set; }
        public HashSet<Goods> Goodss { get; set; }

    }
}
