using System;
using System.Collections.Generic;
using System.Text;

namespace LLM_Code_Reader.Models
{
    public class Message
    {
        private string _content;
        private string _sender;

        public string Content => _content;
        public string Sender => _sender;

        public Message(string content, string sender)
        {
            _content = content;
            _sender = sender;
        }
    }
}
