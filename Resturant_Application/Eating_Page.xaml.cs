using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Resturant_Application
{
    /// <summary>
    /// Interaction logic for Eating_Page.xaml
    /// </summary>
    public partial class Eating_Page : Page
    {
        int table_id = 0;
        public Eating_Page(int arg)
        {
            InitializeComponent();
            table_id = arg;
            
            int i = 0;
            int total = 0;
            
            using (var db = new Resturant_DatabaseEntities())
            {
               
                List<Dish> some_dish = new List<Dish>();
                var query = from billtable in db.BillTable where billtable.TableId == table_id && billtable.DishId != null select billtable;
                foreach (var row in query)
                {
                    var dish = from dishtable in db.Dish where dishtable.DishId == row.DishId select dishtable;

                    foreach (var column in dish)
                    {
                        some_dish.Add(column);
                        total +=(int)column.DishPrice;
                        
                    }
                    for (i = 0; i < some_dish.Count(); i++)
                    {
                        TextBlock textblock = new TextBlock();

                        textblock.Text = some_dish[i].DishName + "   $" + some_dish[i].DishPrice;
                        
                        textblock.Margin = new Thickness(200, 100 + 20 * i, 20, 30);
                        textblock.Foreground = Brushes.Brown;
                        
                        grid.Children.Add(textblock);
                    }  
                }

                var tables = from table in db.Table where table.TableId == table_id select table;
                foreach(var row in tables)
                 {
                     var waiters = from waiter in db.Waiter where waiter.WaiterId == row.WaiterId select waiter;
                     {
                         foreach(var column in waiters)
                         {                               
                             column.EarnMoney = total;
                             db.SaveChanges();

                         }
                     }
                 }
            }
            TextBlock textblock1 = new TextBlock();
            textblock1.Text = "The total earn money is $" + total;
            textblock1.Margin = new Thickness(200, 120 + 20 * i, 20, 30);
            textblock1.Foreground = Brushes.Red;
            grid.Children.Add(textblock1);
        }

    }
}
