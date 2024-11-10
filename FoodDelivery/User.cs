using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using System.Xml.Linq;

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
