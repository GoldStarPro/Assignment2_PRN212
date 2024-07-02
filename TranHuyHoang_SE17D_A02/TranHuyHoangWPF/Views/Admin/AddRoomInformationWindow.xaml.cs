﻿using BusinessObject;
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
            txtStatus.Text = "1";
            DataContext = roomInformation;

            txtRoomType.ItemsSource = roomTypeService.GetRoomTypes();
            txtRoomType.DisplayMemberPath = "RoomTypeName";
            txtRoomType.SelectedValuePath = "RoomTypeId";
        }


        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            roomInformation.RoomTypeId = (int)txtRoomType.SelectedValue;


            if (btnAdd.Content.Equals("Add"))
            {
                roomInformationService.AddRoomInformation(roomInformation);
            }
            else
            {
                roomInformationService.UpdateRoomInformation(roomInformation);
            }

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
