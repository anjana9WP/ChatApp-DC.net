﻿using ServerInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ClientWPF
{
    public partial class MainWindow : Window
    {
        private DataServerInterface foob;
        private ChannelFactory<DataServerInterface> foobFactory;
        private string loggedUser;
        private int portNumForClient;
        private List<string> userLogged;

        public MainWindow()
        {
            InitializeComponent();

            // Set all the default state of all window elements where needed
            Warning_Label.Text = "";
            Username_TextBox.Text = "";
            LoggedIn_Label.Content = "Logged in as: ";
            Button_EnterChatRoom.Visibility = Visibility.Hidden;
            Button_EnterChatRoom.IsEnabled = false;
            portNumForClient = 8100;
            userLogged = new List<string>();

            connectToServer();
        }

        // Used to reconnect to the server
        private void connectToServer()
        {
            NetTcpBinding tcpB = new NetTcpBinding();

            tcpB.CloseTimeout = new TimeSpan(0, 0, 10);
            tcpB.ReceiveTimeout = new TimeSpan(0, 0, 10);
            tcpB.SendTimeout = new TimeSpan(0, 0, 30);
            tcpB.MaxBufferPoolSize = 100000;
            tcpB.MaxReceivedMessageSize = 700000;
            tcpB.MaxBufferSize = 700000;
            tcpB.ReaderQuotas.MaxArrayLength = 10000;
            tcpB.ReaderQuotas.MaxDepth = 10;
            tcpB.ReaderQuotas.MaxBytesPerRead = 10000;
            tcpB.ReaderQuotas.MaxStringContentLength = 10000;

            string URL = "net.tcp://localhost:8100/DataService";
            foobFactory = new ChannelFactory<DataServerInterface>(tcpB, URL);
            foob = foobFactory.CreateChannel();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            // Button Functionality
            Warning_Label.Text = "";
            string username = Username_TextBox.Text;
            if (string.IsNullOrEmpty(username))
            {
                // Case of Empty given field by user
                Warning_Label.Text = "Username field should not be empty!";
            }
            else
            {
                Warning_Label.Text = "Processing request..";
                bool logged = foob.UserAdd(username);
                if (logged)
                {
                    connectToServer();
                    // Enable the "Enter ChatRoom Button"
                    Warning_Label.Text = "Logged in successfully!";
                    loggedUser = username;
                    LoggedIn_Label.Content = "Logged in as: " + loggedUser;
                    Button_EnterChatRoom.Visibility = Visibility.Visible;
                    Button_EnterChatRoom.IsEnabled = true;
                }
                else
                {
                    Warning_Label.Text = "User exists!";
                }
            }
        }

        // This function is called once the user is logged in and they press the "Enter ChatRoom Button"
        private void enterChatRoom()
        {
            ChatRoomWindow chatRoomWindow = new ChatRoomWindow(loggedUser, portNumForClient, this);
            if (chatRoomWindow != null)
            {
                userLogged.Add(loggedUser);
                chatRoomWindow.Show();
            }
            else
            {
                Warning_Label.Text = "Failed to switch windows";
            }
        }

        public void exitChatRoom(string user)
        {
            if (checkIfAlreadyIn(user))
            {
                for (int i = 0; i < userLogged.Count; i++)
                {
                    if (userLogged[i].Equals(user))
                    {
                        userLogged.Remove(user);
                    }
                }
            }
        }

        private bool checkIfAlreadyIn(string user)
        {
            bool returnVal = false;
            foreach (string userInList in userLogged)
            {
                if (userInList.Equals(user))
                {
                    returnVal = true;
                }
            }
            return returnVal;
        }

        private void EnterChatRoom_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (checkIfAlreadyIn(loggedUser))
                {
                    Warning_Label.Text = "User is already logged in!";
                }
                else
                {
                    enterChatRoom();
                }
            }
            catch (Exception eR)
            {
                Console.WriteLine(eR.Message);
                Warning_Label.Text = eR.Message;
                connectToServer();
            }
        }
    }
}
