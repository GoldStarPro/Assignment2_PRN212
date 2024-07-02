using BusinessObject;
using Service;
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
using System.Windows.Shapes;

namespace TranHuyHoangWPF
{
    /// <summary>
    /// Interaction logic for AddRoomInformationWindow.xaml
    /// </summary>
    public partial class AddRoomInformationWindow : Window
    {
        private RoomInformation roomInformation;
        private readonly IRoomInformationService roomInformationService = new RoomInformationService();
        private readonly IRoomTypeService roomTypeService = new RoomTypeService();


        public AddRoomInformationWindow()
        {
            InitializeComponent();
            roomInformation = new RoomInformation
            {
                RoomStatus = 1
            };
            DataContext = roomInformation;

            txtRoomType.ItemsSource = roomTypeService.GetRoomTypes();
            txtRoomType.DisplayMemberPath = "RoomTypeName";
            txtRoomType.SelectedValuePath = "RoomTypeId";
        }


        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {

            if (string.IsNullOrWhiteSpace(txtRoomNumber.Text) ||
                txtRoomType.SelectedValue == null ||
                string.IsNullOrWhiteSpace(txtRoomDescription.Text) ||
                string.IsNullOrWhiteSpace(txtRoomMaxCapacity.Text) ||
                string.IsNullOrWhiteSpace(txtPrice.Text))
            {
                MessageBox.Show("Please fill in all fields correctly.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!int.TryParse(txtRoomMaxCapacity.Text, out int roomMaxCapacity))
            {
                MessageBox.Show("Max Capacity must be a valid number.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!decimal.TryParse(txtPrice.Text, out decimal roomPrice))
            {
                MessageBox.Show("Price must be a valid decimal number.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            roomInformation.RoomTypeId = (int)txtRoomType.SelectedValue;

            // So sánh với AddCustomer, ko cần gán các trường, đã có trong DBContext, Binding

            roomInformationService.AddRoomInformation(roomInformation);

            DialogResult = true;
            Close();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;

            Close();
        }
    }
}
