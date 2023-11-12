using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using XFYun.SparkChat.Models;
using XFYun.SparkChat.SDK;

namespace XFYun.SparkChat.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private SparkWebSDK sdk;
        public MainWindowViewModel()
        {
            SendCmd = new DelegateCommand(Send, () => !string.IsNullOrEmpty(this.MsgToSend));
            sdk = new SparkWebSDK();
        }
        private async void Send()
        {
            sdk.Setup(this.AppId, this.ApiSecret, this.ApiKey, this.SelectedVersion);
            this.Messages.Add(new MessageModel(MessageType.Send, this.MsgToSend));
            var responseMessage = new MessageModel(MessageType.Receive);
            this.Messages.Add(responseMessage);
            var (ok, errMsg) = await sdk.Ask(new List<string>() { this.MsgToSend }, new System.Threading.CancellationToken(), strs =>
            {
                foreach (var str in strs)
                {
                    responseMessage.AddMsg(str);
                }
            });
            if (!ok)
            {
                MessageBox.Show(errMsg, "error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                MsgToSend = "";
            }
        }

        private ObservableCollection<MessageModel> messages = new ObservableCollection<MessageModel>();
        public ObservableCollection<MessageModel> Messages
        {
            get { return messages; }
            set { messages = value; }
        }
        private string apiKey = "";
        public string ApiKey
        {
            get { return apiKey; }
            set { apiKey = value; this.RaisePropertyChanged(nameof(ApiKey)); }
        }

        private string apiSecret = "";
        public string ApiSecret
        {
            get { return apiSecret; }
            set { apiSecret = value; this.RaisePropertyChanged(nameof(ApiSecret)); }
        }


        private string appId = "";
        public string AppId
        {
            get { return appId; }
            set { appId = value; this.RaisePropertyChanged(nameof(AppId)); }
        }

        private SparkVersions selectedVersion = SparkVersions.V3_0;
        public SparkVersions SelectedVersion
        {
            get { return selectedVersion; }
            set { selectedVersion = value; this.RaisePropertyChanged(nameof(SelectedVersion)); }
        }


        private string msgToSend;
        public string MsgToSend
        {
            get { return msgToSend; }
            set
            {
                msgToSend = value;
                this.RaisePropertyChanged(nameof(MsgToSend));
                this.SendCmd?.RaiseCanExecuteChanged();
            }
        }
        public DelegateCommand SendCmd { get; set; }
    }
}
