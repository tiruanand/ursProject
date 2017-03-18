using System;
using System.Collections.Generic;
using System.IO;
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

namespace urShopper.Data.Loader.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class DataLoadUI : Window
    {
        public DataLoadUI()
        {
            InitializeComponent();
            cbxServices.SelectedIndex = 0;
        }

        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                btnLoad.IsEnabled = false;
                if (string.IsNullOrEmpty(txtInput.Text.Trim()))
                {
                    MessageBox.Show("Please enter valid input data to load.", "Invalid Data", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                switch (((ComboBoxItem)cbxServices.SelectedValue).Content.ToString())
                {
                    case "Consumers":
                        var consumerLoader = new ConsumerLoader();
                        txtOutput.Text = consumerLoader.LoadConsumers(txtInput.Text.Trim());
                        break;

                    case "Suppliers":
                        var supplierLoader = new SupplierLoader();
                        txtOutput.Text = supplierLoader.LoadSuppliers(txtInput.Text.Trim());
                        break;

                    case "Marketers":
                        var marketerLoader = new MarketerLoader();
                        txtOutput.Text = marketerLoader.LoadMarketers(txtInput.Text.Trim());
                        break;

                    case "Products":
                        var productLoader = new ProductLoader();
                        txtOutput.Text = productLoader.LoadProducts(txtInput.Text.Trim());
                        break;

                    case "Consumer Offers":
                        var consumerOfferLoader = new ConsumerOfferLoader();
                        txtOutput.Text = consumerOfferLoader.LoadConsumerOffers(txtInput.Text.Trim());
                        break;

                    case "Supplier Offers":
                        var supplierOfferLoader = new SupplierOfferLoader();
                        txtOutput.Text = supplierOfferLoader.LoadSupplierOffers(txtInput.Text.Trim());
                        break;

                    default:
                        throw new Exception("Unknown entity type. Please select a valid entity type to proceed.");
                }
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException entityEx)
            {
                var sbErr = new StringBuilder("Error(s) occurred:\n");
                foreach (var err in entityEx.EntityValidationErrors)
                {
                    foreach (var vErr in err.ValidationErrors)
                    {
                        sbErr.AppendLine(vErr.ErrorMessage);
                    }
                }
                MessageBox.Show(sbErr.ToString(), "Errors", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Errors", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                btnLoad.IsEnabled =true;
            }
        }

        private void cbxServices_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            txtInput.Text = string.Empty;
            txtOutput.Text = string.Empty;
        }

        private void btnSample_Click(object sender, RoutedEventArgs e)
        {
            txtInput.Text = string.Empty;
            txtOutput.Text = string.Empty;

            try
            {
                switch (((ComboBoxItem)cbxServices.SelectedValue).Content.ToString())
                {
                    case "Consumers":
                    case "Suppliers":
                    case "Marketers":
                    case "Products":
                    case "Consumer Offers":
                    case "Supplier Offers":
                        ShowSampleData(string.Format(@"SampleData\{0}.xml", ((ComboBoxItem)cbxServices.SelectedValue).Content.ToString()));
                        break;

                    default:
                        throw new Exception("Unknown entity type. Please select a valid entity type show sample data.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Sample Data Errors", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        void ShowSampleData(string SampleFileName)
        {
            txtInput.Text = File.ReadAllText(SampleFileName);
        }
    }
}
