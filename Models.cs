namespace Zadanie4
{
    public class Animal
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }  // e.g., dog, cat
        public double Weight { get; set; }
        public string FurColor { get; set; }
    }

    public class Visit
    {
        public int Id { get; set; }
        public DateTime DateOfVisit { get; set; }
        public int AnimalId { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
    }

}
