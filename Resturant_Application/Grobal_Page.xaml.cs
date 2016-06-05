
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
using System.Text.RegularExpressions;


namespace Resturant_Application
{
    /// <summary>
    /// Interaction logic for Grobal_Page.xaml
    /// </summary>
    /// <summary>
    /// Interaction logic for Grobal_Page.xaml
    /// </summary>
    public partial class Grobal_Page : Page
    {
        Point targetPoint;
        bool edit_map = false;
        public Grobal_Page()
        {
            InitializeComponent();
        }
        private void Initial(object sender,RoutedEventArgs e)
        {
            string[] tableCsv = System.IO.File.ReadAllLines(@"initial_table.csv");
            string[] dishCsv = System.IO.File.ReadAllLines(@"initial_dish.csv");
            string[] waiterCsv = System.IO.File.ReadAllLines(@"initial_waiter.csv");
            string regexText = ",";
            string[] waiters;
            foreach (string line2 in waiterCsv)
            {
                waiters = Regex.Split(line2, regexText);
                using (var db = new Resturant_DatabaseEntities())
                {
                    Waiter waiter = new Waiter() { WaiterId = int.Parse(waiters[0]), WaiterName = waiters[1], WaiterStatus = waiters[2], NumberOfHandleTable = int.Parse(waiters[3]), EarnMoney = int.Parse(waiters[4]) };
                    db.Waiter.Add(waiter);

                    db.SaveChanges();
                }

            }

            string[] tables;
            foreach (string line in tableCsv)
            {
                tables = Regex.Split(line, regexText);
                using (var db = new Resturant_DatabaseEntities())
                {
                    Table table = new Table() { TableId = int.Parse(tables[0]), NumberOfPeople = int.Parse(tables[1]), NumberOfDishes = int.Parse(tables[2]), WaiterId = int.Parse(tables[3]), TableStatus = tables[4] };
                    db.Table.Add(table);

                    db.SaveChanges();
                }
            }

            string[] dishes;
            foreach (string line1 in dishCsv)
            {
                dishes = Regex.Split(line1, regexText);
                using (var db = new Resturant_DatabaseEntities())
                {
                    Dish dish = new Dish() { DishId = int.Parse(dishes[0]), DishName = dishes[1], DishPrice = int.Parse(dishes[2]) };
                    db.Dish.Add(dish);
                    db.SaveChanges();
                }
            }

            var button = sender as Button;
            
            grid.Children.Remove(button);
            
        }

        public void Judge_Status(object sender, RoutedEventArgs e)
        {

            var button = sender as Button;
            var buttonName = button.Name;
            int table_id = char.Parse(buttonName) - 'a';

            Empty_Page empty = new Empty_Page(table_id);
            Eating_Page eating = new Eating_Page(table_id);

            using (var db = new Resturant_DatabaseEntities())
            {
                var query = from table in db.Table where table.TableId == table_id select table;
                foreach (var rowTable in query)
                {
                    if (rowTable.TableStatus == "empty")
                    {
                        if (MessageBox.Show("This is an empty table, do you want to add dishes?", "message", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                        {
                            rowTable.TableStatus = "eating";
                            button.Content = "eating";
                            this.NavigationService.Navigate(empty);
                            db.SaveChanges();
                            
                        }

                    }
                    else if (rowTable.TableStatus == "eating")
                    {
                        this.NavigationService.Navigate(eating);
                        
                    }
                }
            }
        }

        private void Label_MouseEnter(object sender, MouseEventArgs e)
        {
            Label label = sender as Label;
            label.Opacity =100;

        }

        private void Label_MouseLeave(object sender, MouseEventArgs e)
        {
            Label label = sender as Label;
            label.Opacity = 0.01;
        }
        private void ShowModalDialog1(bool bShow)
        {
            this.ModalDialog1.IsOpen = bShow;
            this.MainPanel1.IsEnabled = !bShow;
        }
       
        private void BtnShowDlg_Click1(object sender, RoutedEventArgs e)
        {
            ShowModalDialog1(true);
        }
        private void Dlg_BtnClose_Click1(object sender, RoutedEventArgs e)
        {
            ShowModalDialog1(false);
        }
        private void Dlg_BtnOK_Click1(object sender, RoutedEventArgs e)
        {
            ShowModalDialog1(false);
            string regexText = ",";
            string[] dish_list;
            dish_list = Regex.Split(TxtBoxInput2.Text, regexText);
            using (var db = new Resturant_DatabaseEntities())
            {

                if(TxtBoxInput1.Text.Trim()=="Add")
                {
                    Dish dish = new Dish() {DishId=int.Parse(dish_list[0]),DishName=dish_list[1],DishPrice=int.Parse(dish_list[2])};
                    db.Dish.Add(dish);
                    db.SaveChanges();
                }
                else if(TxtBoxInput1.Text.Trim()=="Update")
                {
                    var query = from dishes in db.Dish where dishes.DishId == int.Parse(dish_list[0]) select dishes;
                    foreach(var row in query)
                    {
                        row.DishName = dish_list[1];
                        row.DishPrice = int.Parse(dish_list[2]);
                        db.SaveChanges();
                    }
                }
                else if(TxtBoxInput1.Text=="Delete")
                {
                    var query = from dishes in db.Dish where dishes.DishId == int.Parse(dish_list[0]) select dishes;
                    foreach (var row in query)
                    {
                        row.DishName =null;
                        row.DishPrice = null;
                        db.SaveChanges();
                    }
                }
            }
        }
        private void ShowModalDialog2(bool bShow)
        {
            this.ModalDialog2.IsOpen = bShow;
            this.MainPanel2.IsEnabled = !bShow;
        }

        private void BtnShowDlg_Click2(object sender, RoutedEventArgs e)
        {
            ShowModalDialog2(true);
        }
        private void Dlg_BtnClose_Click2(object sender, RoutedEventArgs e)
        {
            ShowModalDialog2(false);
            
        }
        private void Dlg_BtnOK_Click2(object sender, RoutedEventArgs e)
        {
            ShowModalDialog2(false);
            string regexText = ",";
            string[] Waiter_list;
            Waiter_list = Regex.Split(TxtBoxInput4.Text, regexText);
            using (var db = new Resturant_DatabaseEntities())
            {

                if (TxtBoxInput3.Text.Trim() == "Add")
                {
                    Waiter waiter = new Waiter() { WaiterId = int.Parse(Waiter_list[0]), WaiterName = Waiter_list[1] };
                    db.Waiter.Add(waiter);
                    db.SaveChanges();
                }
                else if (TxtBoxInput3.Text.Trim() == "Update")
                {
                    var query = from waiters in db.Waiter where waiters.WaiterId == int.Parse(Waiter_list[0]) select waiters;
                    foreach (var row in query)
                    {
                        row.WaiterName = Waiter_list[1];
                        db.SaveChanges();
                    }
                }
                else if (TxtBoxInput3.Text == "Delete")
                {
                    var query = from waiters in db.Waiter where waiters.WaiterId == int.Parse(Waiter_list[0]) select waiters;
                    foreach (var row in query)
                    {
                        row.WaiterName = null;
                        row.WaiterStatus = null;
                        row.NumberOfHandleTable = null;
                        row.EarnMoney = null;
                        db.SaveChanges();
                    }
                }
            }

        }
        private void ShowModalDialog3(bool bShow)
        {
            this.ModalDialog3.IsOpen = bShow;
            this.MainPanel3.IsEnabled = !bShow;
        }

        private void BtnShowDlg_Click3(object sender, RoutedEventArgs e)
        {
            ShowModalDialog3(true);
        }
        private void Dlg_BtnClose_Click3(object sender, RoutedEventArgs e)
        {
            ShowModalDialog3(false);
        }
        private void Dlg_BtnOK_Click3(object sender, RoutedEventArgs e)
        {
            //ShowModalDialog3(false);
            using(var db=new Resturant_DatabaseEntities())
            {
                if(TxtBoxInput5.Text.Length!=0&&TxtBoxInput6.Text.Length==0)
                {
                
                     int tableid = int.Parse(TxtBoxInput5.Text.Trim());
                     var query = from tables in db.Table where tables.TableId == tableid select tables.TableId;
                     foreach(var row in query)
                     {
                         var waiter =from waiters in db.Waiter where waiters.WaiterId==row select waiters;
                         if (waiter != null)
                         {
                             foreach (var column in waiter)
                             {
                                 MessageBox.Show("waiter id : " + column.WaiterId + " waiter name: " + column.WaiterName + " earn money :" + column.EarnMoney + " handled number of table: " + column.NumberOfHandleTable + " waiter status " + column.WaiterStatus);
                             }
                         }
                         else 
                         {
                             MessageBox.Show("No Found");
                         }
                     }
                }
                else if (TxtBoxInput5.Text.Length == 0 && TxtBoxInput6.Text.Length != 0)
                {

                    int money = int.Parse(TxtBoxInput6.Text.Trim());
                    var query = from waiters in db.Waiter where waiters.EarnMoney == money select waiters;
                    if (query != null)
                    {
                        foreach (var row in query)
                        {
                            MessageBox.Show("waiter id : " + row.WaiterId + " waiter name: " + row.WaiterName + " earn money :" + row.EarnMoney + " handled number of table: " + row.NumberOfHandleTable + " waiter status " + row.WaiterStatus);
                        }
                    }
                    else
                    {
                        MessageBox.Show("No Found");
                    }
                }
                else if(TxtBoxInput5.Text.Length!=0&&TxtBoxInput6.Text.Length!=0)
                {
                    int tableid = int.Parse(TxtBoxInput5.Text.Trim());
                    int money = int.Parse(TxtBoxInput6.Text.Trim());
                    var query = from tables in db.Table where tables.TableId == tableid select tables.TableId;

                    foreach (var row in query)
                    {
                        var waiter = from waiters in db.Waiter where waiters.WaiterId == row&&waiters.EarnMoney==money select waiters;
                        if (waiter != null)
                        {
                            foreach (var column in waiter)
                            {
                                MessageBox.Show("waiter id : " + column.WaiterId + " waiter name: " + column.WaiterName + " earn money :" + column.EarnMoney + " handled number of table: " + column.NumberOfHandleTable + " waiter status " + column.WaiterStatus);
                            }
                        }
                        else
                        {
                            MessageBox.Show("No Found");
                        }
                    }
                }

            }
            ShowModalDialog3(false);
        }
        private void Edit_Map(object sender ,RoutedEventArgs e)
        {
            edit_map = true;
        }
        private void Stop_Edit(object sender,RoutedEventArgs e)
        {
            edit_map = false;
        }
        public void Grid_Move(object sender, MouseEventArgs e)
        {
            var targetElement = Mouse.Captured as UIElement;
            if (e.LeftButton == MouseButtonState.Pressed && targetElement != null&&edit_map==true)
            {
                var pGrid = e.GetPosition(grid);
                Canvas.SetLeft(targetElement, pGrid.X - targetPoint.X);
                Canvas.SetTop(targetElement, pGrid.Y - targetPoint.Y);
            }
        }
        public void Grid_Up(object sender, MouseButtonEventArgs e)
        {
            if (edit_map == true)
            {
                Mouse.Capture(null);
            }
        }
        public void Grid_Down(object sender, MouseButtonEventArgs e)
        {
            var targetElement = e.Source as IInputElement;
            if (targetElement != null && edit_map==true)
            {
                targetPoint = e.GetPosition(targetElement);
                targetElement.CaptureMouse();
            }
        }
        
        
    }
}

