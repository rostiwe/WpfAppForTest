using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using ClassesForTest;

namespace WpfAppForTest
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<Directory> DList = new List<Directory>();
        public List<DirectoryInTable> InDList = new List<DirectoryInTable>();
        public List<DirectoryInTable> OutDList = new List<DirectoryInTable>();


        public MainWindow()
        {
            try
            {
                XMLAllDirectoryClass.GetXMLInfo("XMLDirectory.xml", DList);
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Не смог загрузить справочник  " + ex.Message + "  " + ex.Source);
                try
                {
                    XmlTextWriter textWritter = new XmlTextWriter("XMLDirectory.xml", null);

                    textWritter.WriteStartDocument();

                    textWritter.WriteStartElement("root");

                    textWritter.WriteEndElement();

                    textWritter.Close();
                    List<Directory> DListMini = new List<Directory>() { new Directory("FFFFFFFFFFFFAAAAAAAAAAAA", "Товар 1"), new Directory("AAAAAAAAAAAAFFFFFFFFFFFF", "Товар 2") };
                    XMLAllDirectoryClass.DirectoryXMLRevrite(DListMini, "XMLDirectory.xml");
                    XMLAllDirectoryClass.GetXMLInfo("XMLDirectory.xml", DList);
                    
                }
                catch (Exception ex2)
                {
                    MessageBox.Show("Не пошло с самого начала" + ex.Message + "  " + ex.Source);
                    //MessageBox.Show("Создать нужный католог не получилось");
                    //this.Close();

                }
            }
            InitializeComponent();
            AllGrid.ItemsSource = DList;
            InGrid.ItemsSource = InDList;
            OutGrid.ItemsSource = OutDList;

        }

        private void CleanButton_Click(object sender, RoutedEventArgs e)
        {
            InDList.RemoveAll(x => true);
            OutDList.RemoveAll(x => true);
            OutGrid.Items.Refresh();
            InGrid.Items.Refresh();
            MessageBox.Show("Готово!");


        }

        private void InTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter)
                {
                    List<Directory> strArr;
                    try
                    {
                        strArr = PurseStr(InTextBox);
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message == "0") MessageBox.Show("Подходящих значений не обнаруженно!");
                        else MessageBox.Show(ex.Message);
                        return;
                    }


                    foreach (var s in strArr)
                    {
                        if (InDList.Count == 0)
                        {
                            InDList.Add(new DirectoryInTable(s.GetSetHexNamb, s.GetSetName));
                            DeliteFromTable(OutDList, s);
                            continue;
                        }
                        for (int i = 0; i< InDList.Count; i++)
                        {
                            if (InDList[i].GetSetHexNamb == s.GetSetHexNamb)
                            {
                                InDList[i].count++;
                                DeliteFromTable(OutDList, s);
                                break;
                            }
                            else if(i == InDList.Count - 1)
                            {
                                InDList.Add(new DirectoryInTable(s.GetSetHexNamb, s.GetSetName));
                                DeliteFromTable(OutDList, s);
                                break;
                            }
                        }
                    }
                    OutGrid.Items.Refresh();
                    InGrid.Items.Refresh();
                    MessageBox.Show("Всё!");
                    ///////////////////////
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        void DeliteFromTable (List<DirectoryInTable> l, Directory d)
        {
            for (int i = 0; i< l.Count; i++)
            {
                if (l[i].GetSetHexNamb == d.GetSetHexNamb)
                    if (l[i].count == 1)
                    {
                        l.RemoveAt(i);
                        return;
                    }
                    else
                    {
                        l[i].count--;
                        return;
                    }
            }

        }


        List <Directory> PurseStr (TextBox t)
        {
            if (t.Text == "") { t.Text = ""; throw new Exception("0"); }
            string str = t.Text.ToUpper();
            List< Directory> strArr = new List<Directory>();
            int i = 0;
            while (i < str.Length)
            {
                if (str[i] != ' ')
                {
                    int j = str.IndexOf(" ", i);                    
                    if (j == -1)
                    {
                        string SomeString = str.Substring(i, str.Length - i);
                        for(int Count = 0; Count< DList.Count;  Count++)
                        {
                            if (DList[Count].GetSetHexNamb.ToUpper() == SomeString)
                            {
                                strArr.Add(DList[Count]);
                                break;
                            }
                        }
                        break;
                    }
                    string SomeString2 = str.Substring(i, j - i);
                    for (int Count = 0; Count < DList.Count; Count++)
                    {
                        
                        if (DList[Count].GetSetHexNamb.ToUpper() == SomeString2)
                        {
                            strArr.Add(DList[Count]);
                            break;
                        }
                    }
                    i = j;
                }
                i++;
            }
            t.Text = "";
            if (strArr.Count == 0) throw new Exception("0");
            return strArr;
        }

        private void OutTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter)
                {
                    List<Directory> strArr;
                    try
                    {
                        strArr = PurseStr(OutTextBox);
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message == "0") MessageBox.Show("Подходящих значений не обнаруженно!");
                        else MessageBox.Show(ex.Message);
                        return;
                    }


                    foreach (var s in strArr)
                    {
                        if (OutDList.Count == 0)
                        {
                            OutDList.Add(new DirectoryInTable(s.GetSetHexNamb, s.GetSetName));
                            DeliteFromTable(InDList, s);
                            continue;
                        }
                        for (int i = 0; i < OutDList.Count; i++)
                        {
                            if (OutDList[i].GetSetHexNamb == s.GetSetHexNamb)
                            {
                                OutDList[i].count++;
                                DeliteFromTable(InDList, s);
                                break;
                            }
                            else if (i == OutDList.Count - 1)
                            {
                                OutDList.Add(new DirectoryInTable(s.GetSetHexNamb, s.GetSetName));
                                DeliteFromTable(InDList, s);
                                break;
                            }
                        }
                    }
                    OutGrid.Items.Refresh();
                    InGrid.Items.Refresh();
                    MessageBox.Show("Всё!");
                    ///////////////////////
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Check(DList);
                XMLAllDirectoryClass.DirectoryXMLRevrite(DList, "XMLDirectory.xml");
                try
                {
                    DList = new List<Directory>();
                    AllGrid.ItemsSource = DList;
                    XMLAllDirectoryClass.GetXMLInfo("XMLDirectory.xml", DList);
                    AllGrid.Items.Refresh();
                    MessageBox.Show("Готово");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CheckButton_Click(object sender, RoutedEventArgs e)
        {
            Check(DList);
            string str = "";
            for (int a = 0; a< DList.Count; a++)
            {
                if (DList[a].UniqueState && DList[a].HexNumbState) continue;
                str += "(" + DList[a].GetSetHexNamb + ", [" + a + "] )\n ";
            }
            if (str == "") MessageBox.Show("Проблем не обнаруженно");
            else
            {
                MessageBox.Show("В позициях с номерами: \n" + str + "\n обнаруженны пробдемы");
            }
        }
        void Check(List<Directory> List)
        {
            var count2 = List.Select((x, index) => (x.GetSetHexNamb, index))
                .GroupBy(x => x.GetSetHexNamb, x => x.index).Where(x => x.Count() > 1)
                .SelectMany(x => x)
                .ToList();
            foreach (var a in List)
            {
                a.UniqueState = true;
            }
            foreach (var a in count2)
            {
                List[a].UniqueState = false;
            }

        }
    }
}
