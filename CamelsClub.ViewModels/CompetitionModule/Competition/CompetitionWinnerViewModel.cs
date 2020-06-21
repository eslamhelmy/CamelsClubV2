namespace CamelsClub.ViewModels
{
    public class CompetitionWinnerViewModel
    {
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string UserImage { get; set; }
        public double? Percentage { get; set; }
        public string GroupName { get; set; }
        public string GroupImage { get; set; }
        public Rank Rank { get; set; }
        public string RewardTextArabic { get; set; }
        public string RewardTextEnglish { get; set; }
    }
}