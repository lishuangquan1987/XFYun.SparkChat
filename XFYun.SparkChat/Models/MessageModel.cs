using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFYun.SparkChat.Models
{
    public class MessageModel : BindableBase
    {
        public MessageModel(MessageType type)
        {
            this.MessageType = type;
            this.Text = $"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}]{type}:";
        }
        public MessageModel(MessageType type, string msg)
        {
            this.MessageType= type ;
            this.Text = $"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}]{type}:{msg}";
        }
        private string text;
        public string Text
        {
            get { return text; }
            set { text = value; this.RaisePropertyChanged(nameof(Text)); }
        }

        private bool isLoading;
        public bool IsLoading
        {
            get { return isLoading; }
            set { isLoading = value; this.RaisePropertyChanged(nameof(IsLoading)); }
        }

        public void AddMsg(string msg)
        {
            this.text += msg;
            this.RaisePropertyChanged(nameof(Text));
        }

        private MessageType messageType;
        public MessageType MessageType
        {
            get { return messageType; }
            set { messageType = value; this.RaisePropertyChanged(nameof(MessageType)); }
        }

    }
    public enum MessageType { Send, Receive }
}
