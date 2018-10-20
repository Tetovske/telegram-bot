using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Drawing;
using Newtonsoft.Json;
using Telegram.Bot;
using Telegram.Bot.Types;
using System.IO;
using SimpleJSON;

namespace TelegramBot
{
    static class Answers
    {
        public static AnswerType TextMessageRecieved(Telegram.Bot.Types.Message msg) // only text messages
        {
            AnswerBase.AnswerBase.Responce res = AnswerBase.AnswerBase.FindAnswer(msg);
            return new AnswerType(res.responce, res.type, msg);
        }
    }
    class AnswerType
    {
        public enum TypesMsg
        {
            onlyTxt,
            withName,
            photo
        }
        public object responce;
        public TypesMsg type;
        public Telegram.Bot.Types.Message msg;
        public AnswerType(object responce_, TypesMsg type_, Telegram.Bot.Types.Message msg_)
        {
            responce = responce_;
            type = type_;
            msg = msg_;
        }
    }
}
