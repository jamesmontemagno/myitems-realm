using System.Windows.Input;
using Realms;
using Xamarin.Forms;

namespace QuickJournal
{
    public class JournalEntryDetailsViewModel
    {

        public JournalEntry Entry { get; private set; }

        internal INavigation Navigation { get; set; }

        public ICommand SaveCommand { get; private set; }

        public JournalEntryDetailsViewModel(JournalEntry entry)
        {
            
            Entry = entry;
            SaveCommand = new Command(Save);
        }

        private void Save()
        {
            Navigation.PopAsync(true);
        }

        internal void OnDisappearing()
        {
            //_transaction.Dispose();
        }
   }
}

