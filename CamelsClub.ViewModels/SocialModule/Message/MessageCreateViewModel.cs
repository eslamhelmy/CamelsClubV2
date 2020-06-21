using CamelsClub.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CamelsClub.ViewModels
{
    public class MessageCreateViewModel
    {
        [IgnoreDataMember]
        public int ID { get; set; }
        [IgnoreDataMember]
        public int FromUserID { get; set; }
        public int ToUserID { get; set; }
        [Required]
        public string Text { get; set; }
    }

    public static partial class MessageExtensions
    {
        public static Message ToModel(this MessageCreateViewModel viewModel)
        {
            return new Message
            {
                ID = viewModel.ID,
                ToUserID = viewModel.ToUserID,
                FromUserID = viewModel.FromUserID,
                Text = viewModel.Text

            };
        }
    }
}
