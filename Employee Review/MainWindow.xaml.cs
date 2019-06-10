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
using System.Data;
using System.Collections.ObjectModel;

namespace Employee_Review
{
    public partial class MainWindow : Window
    {
        DataTable dt = new DataTable();
        DataSet ds = new DataSet();
        public DataView dataView { get; set; }
        public bool doubleGridClick = false;
        //Zainicjowanie tabeli 
        public MainWindow()
        {
            InitializeComponent();
            //Utworzenie tabeli przechowującej dane pracownikow
            ds.DataSetName = "Employees";
            dt.TableName = "Employee";
            //Dodanie tabeli do DataSet
            ds.Tables.Add(dt);
            //Dodanie kolumn do tabeli           
            dt.Columns.Add("ID");
            dt.Columns.Add("Name");
            dt.Columns.Add("Surname");
            dt.Columns.Add("Age");
            dt.Columns.Add("Position");
            dt.Columns.Add("Salary");
            dt.Columns.Add("NIP");
            dt.Columns.Add("BirthDate");
            dt.Columns.Add("ImageSource");
            dt.Columns.Add("Sex");

            dataView = dt.DefaultView;
        }
        //Zasilenie DataGrid na starcie formularza
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                string path = System.IO.Directory.GetCurrentDirectory() + "\u005CXMLData.xml";
                ds.Clear();
                ds.ReadXml(path);
                EmployeeDetailsGrid.ItemsSource = ds.Tables[0].DefaultView;
            }
            catch (System.Xml.XmlException)
            {
                MessageBox.Show("Brak możliwości załadowania danych. Wskazany plik XML jest pusty.");
            }
            catch (Exception)
            {
                MessageBox.Show("Błąd załadowania danych");
            }
        }
        //Dodanie wiersza do tabeli z wartosciami z pol formularza
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DateTime currentDate = new DateTime();
                currentDate = DateTime.Now;

                DataView dv = EmployeeDetailsGrid.ItemsSource as DataView;
                dt = dv.Table;
                DataRow dr = dt.NewRow();
                if (NameTextBox.Text != "" && SurnameTextBox.Text != "" && BirthDatePicker.Text != "" && SexComboBox.Text != "")
                {
                    dr["ID"] = Guid.NewGuid().ToString();
                    dr["Name"] = NameTextBox.Text;
                    dr["Surname"] = SurnameTextBox.Text;
                    dr["Age"] = Convert.ToInt32(currentDate.Subtract(DateTime.Parse(BirthDatePicker.Text)).TotalDays / 365);
                    dr["Position"] = PositionTextBox.Text;
                    dr["Salary"] = SalaryTextBox.Text;
                    dr["NIP"] = NIPTextBox.Text;
                    dr["BirthDate"] = BirthDatePicker.Text;
                    Random random = new Random();
                    if (SexComboBox.Text == "M")
                    {
                        dr["ImageSource"] = System.IO.Directory.GetCurrentDirectory() + "\u005CIkony\u005CEmployeeM" + random.Next(1, 5) + ".png";
                    }
                    else
                    {
                        dr["ImageSource"] = System.IO.Directory.GetCurrentDirectory() + "\u005CIkony\u005CEmployeeF" + random.Next(1, 5) + ".png";
                    }
                    dr["Sex"] = SexComboBox.Text;

                    dt.Rows.Add(dr);
                    dt.DefaultView.RowFilter = null;
                }
                else
                {
                    MessageBox.Show("Wymagane dane pracownika to: imię, nazwisko, data urodzenia oraz płeć");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Bład dodawania pracownika");
            }
        }
        //Usuwanie wskazanego wiersza w DataGrid
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (EmployeeDetailsGrid.SelectedItem.ToString() != "{NewItemPlaceholder}")
            {
                var selectedItem = (DataRowView)EmployeeDetailsGrid.SelectedItem;
                int selectedRowIndex = EmployeeDetailsGrid.SelectedIndex;
                if (selectedItem != null)
                {
                    dt.Rows.Remove(selectedItem.Row);
                    EmployeeDetailsGrid.SelectedIndex = selectedRowIndex - 1;
                }
            }
        }
        //Edycja danych pracownika - wymagane przynajmniej imie, nazwisko oraz data urodzenia
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (EmployeeDetailsGrid.SelectedItem.ToString() != "{NewItemPlaceholder}")
                {
                    var selectedItem = (DataRowView)EmployeeDetailsGrid.SelectedItem;
                    DateTime currentDate = new DateTime();
                    currentDate = DateTime.Now;

                    if (selectedItem != null)
                    {
                        if (NameTextBox.Text != "" && SurnameTextBox.Text != "" && BirthDatePicker.Text != "" && SexComboBox.Text != "")
                        {
                            selectedItem["Name"] = NameTextBox.Text;
                            selectedItem["Surname"] = SurnameTextBox.Text;
                            selectedItem["Age"] = Convert.ToInt32(currentDate.Subtract(DateTime.Parse(BirthDatePicker.Text)).TotalDays / 365);
                            selectedItem["Position"] = PositionTextBox.Text;
                            selectedItem["Salary"] = SalaryTextBox.Text;
                            selectedItem["NIP"] = NIPTextBox.Text;
                            selectedItem["BirthDate"] = BirthDatePicker.Text;
                            selectedItem["Sex"] = SexComboBox.Text;
                        }
                        else
                        {
                            MessageBox.Show("Wymagane dane pracownika to: imię, nazwisko, płeć oraz data urodzenia.");
                        }
                    }
                }
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Wskaż wiersz do edycji");
            }
            catch (Exception)
            {
                MessageBox.Show("Błąd edycji");
            }
        }
        //Zapisanie danych do pliku XML (znajduje sie w biezacym katalogu)
        private void ExportButton_Click(object sender, RoutedEventArgs e)
        {
            string path = System.IO.Directory.GetCurrentDirectory() + "\u005CXMLData.xml";
            dt.WriteXml(path);
        }
        //Import danych do pliku XML (znajduje sie w biezacym katalogu)
        private void ImportButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string path = System.IO.Directory.GetCurrentDirectory() + "\u005CXMLData.xml";
                ds.Clear();
                ds.ReadXml(path);
                EmployeeDetailsGrid.ItemsSource = ds.Tables[0].DefaultView;
            }
            catch (System.Xml.XmlException)
            {
                MessageBox.Show("Brak możliwości załadowania danych. Wskazany plik XML jest pusty.");
            }
            catch (Exception)
            {
                MessageBox.Show("Błąda załadowania danych");
            }
        }
        //Obsluga zdarzenia wskazanie wiersza DataGrid
        private void EmployeeDetailsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var selectedItem = (DataRowView)EmployeeDetailsGrid.SelectedItem;
                DateTime currentDate = new DateTime();
                currentDate = DateTime.Now;
                if (selectedItem != null)
                {
                    IDTextBox.Text = selectedItem["ID"].ToString();
                    NameTextBox.Text = selectedItem["Name"].ToString();
                    SurnameTextBox.Text = selectedItem["Surname"].ToString();
                    PositionTextBox.Text = selectedItem["Position"].ToString();
                    SalaryTextBox.Text = selectedItem["Salary"].ToString();
                    NIPTextBox.Text = selectedItem["NIP"].ToString();
                    BirthDatePicker.Text = selectedItem["BirthDate"].ToString();
                    EmployeeImage.Source = new BitmapImage(new Uri(selectedItem["ImageSource"].ToString()));
                    SexComboBox.Text = selectedItem["Sex"].ToString();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Błąd danych. Proszę o ponowne wskazanie wiersza.");
            }
        }
        //Filtrowanie wynikow DataGrid
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string filterSurname = SurnameTextBox.Text;
            string filterNIP = NIPTextBox.Text;
            string filterPosition = PositionTextBox.Text;

            filterSurname = filterSurname ?? "1=1";
            filterNIP = filterNIP ?? "1=1";
            filterPosition = filterPosition ?? "1=1";

            if (string.IsNullOrEmpty(filterSurname) && string.IsNullOrEmpty(filterNIP) && string.IsNullOrEmpty(filterPosition))
            {
                dt.DefaultView.RowFilter = null;
            }
            else
            {
                dt.DefaultView.RowFilter = string.Format("Surname Like '%{0}%' AND NIP Like '%{1}%' AND Position Like '%{2}%'", filterSurname, filterNIP, filterPosition);
            }
        }
        //Czyszczenie filtrow
        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            dt.DefaultView.RowFilter = null;
        }
        //Czyszczenie pol formularza
        private void ClearFormButton_Click(object sender, RoutedEventArgs e)
        {
            IDTextBox.Text = "";
            NameTextBox.Text = "";
            SurnameTextBox.Text = "";
            PositionTextBox.Text = "";
            SalaryTextBox.Text = "";
            NIPTextBox.Text = "";
            BirthDatePicker.Text = "";
            SexComboBox.Text = "";
            EmployeeImage.Source = null;
        }
    }
}
