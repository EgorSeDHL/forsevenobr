namespace FoodDelivery
{
    public class MyUser
    {
        int Id { get; set; }
        string Name { get; set; }
        public override string ToString()
        {
            return Name; // Для отображения только названия в ComboBox
        }
        public MyUser(int id, string name)
        {
            Id = id;
            Name = name;

        }
        public int getUserID() { return Id; }
    }

}
