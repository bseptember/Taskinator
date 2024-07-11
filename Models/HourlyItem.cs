using MvvmHelpers;

namespace Taskinator.Models
{
    public class HourItem : BaseViewModel
    {
        private string _hour;

        public string Hour
        {
            get => _hour;
            set => SetProperty(ref _hour, value);
        }

        public HourItem(string hour)
        {
            _hour = hour;
        }
    }

}
