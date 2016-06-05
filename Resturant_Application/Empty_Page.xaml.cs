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
    /// Interaction logic for Empty_Page.xaml
    /// </summary>
    public partial class Empty_Page : Page
    {
        int table_id = 0;
        int waiter_id = 0;

        
        public Empty_Page(int arg)
        {
            InitializeComponent();
            table_id = arg;
            
        }
        private void check_click(object sender, RoutedEventArgs e)
        {
            var checkbox = sender as CheckBox;
            var buttonName = checkbox.Name;
            int id = char.Parse(buttonName) - 'a';


            CheckBox chk = (CheckBox)sender;
            if (chk.IsChecked == true)
            {
                // MessageBox.Show("hello");
                using (var db = new Resturant_DatabaseEntities())
                {
                    BillTable billtable = new BillTable() { TableId = table_id, DishId = id };
                    db.BillTable.Add(billtable);
                    db.SaveChanges();
                }
            }
            else
            {
                using (var bill_db = new Resturant_DatabaseEntities())
                {
                    var check = from user in bill_db.BillTable where user.DishId == id && user.TableId != null select user;
                    foreach (var rowTable in check)
                    {
                        rowTable.DishId = null;
                        rowTable.TableId = null;
                        bill_db.SaveChanges();
                    }
                }

            }
        }
        private void Waiter_Click(object sender, RoutedEventArgs e)
        {
            var checkbox = sender as CheckBox;
            var buttonName = checkbox.Name;
            switch (buttonName)
            {
                case "box1": waiter_id = 1; break;
                case "box2": waiter_id = 2; break;
                case "box3": waiter_id = 3; break;
                case "box4": waiter_id = 4; break;
                case "box5": waiter_id = 5; break;
                case "box6": waiter_id = 6; break;
                case "box7": waiter_id = 7; break;
                case "box8": waiter_id = 8; break;
                case "box9": waiter_id = 9; break;
                case "box10": waiter_id = 10; break;
            }
            CheckBox chk = (CheckBox)sender;
            if (chk.IsChecked == true)
            {
                using (var db = new Resturant_DatabaseEntities())
                {
                    var query = from table in db.Table where table.TableId == table_id select table;
                    foreach (var row in query)
                    {
                        row.WaiterId = waiter_id;
                       
                        db.SaveChanges();
                    }
                }
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Do you want to submit this order?", "message", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                using(var db=new Resturant_DatabaseEntities())
                {
                    var count_dish = (from billtable in db.BillTable where billtable.TableId != null && billtable.DishId != null select billtable).Count();
                    var query = from table in db.Table where table.TableId == table_id select table;
                    
                    foreach (var row in query)
                    {
                        row.NumberOfDishes = count_dish;
                        var waiter = from waiters in db.Waiter where waiters.WaiterId == row.WaiterId select waiters;
                        foreach(var column in waiter)
                        {
                            column.NumberOfHandleTable += 1;
                            
                            db.SaveChanges();
                           
                        }
                        
                    }
                    
                }
                
            }
           
        }

    }
}
