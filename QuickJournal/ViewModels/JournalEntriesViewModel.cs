using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using MvvmHelpers;
using Realms;
using Xamarin.Forms;

namespace QuickJournal
{
    public class JournalEntriesViewModel : BaseViewModel
    {
        // TODO: add UI for changing that.
        private const string AuthorName = "Me";

        private Realm _realm;

        public IQueryable<JournalEntry> Entries { get; private set; }

        public ICommand AddEntryCommand { get; private set; }

        public ICommand DeleteEntryCommand { get; private set; }

        public INavigation Navigation { get; set; }

        public JournalEntriesViewModel()
        {

            AddEntryCommand = new Command(AddEntry);
            DeleteEntryCommand = new Command<JournalEntry>(DeleteEntry);

            ConnectToRealm().ContinueWith(task => 
            {
                IsBusy = false;
                if (task.Exception != null) 
                {/* error */}
            });
        }

        async Task ConnectToRealm()
        {
            IsBusy = true;
            var ip = "13.64.233.2";

            var credentials = Realms.Sync.Credentials.UsernamePassword("james@xamarin.com", "123", false);
            var authUrl = new Uri($"http://{ip}:9080");
            var user = await Realms.Sync.User.LoginAsync(credentials, authUrl);



            var config = new Realms.Sync.SyncConfiguration(user, new Uri($"realm://{ip}:9080/~/journal"));


            _realm = Realm.GetInstance(config);

            Entries = _realm.All<JournalEntry>();
            OnPropertyChanged(nameof(Entries));
        } 

        private void AddEntry()
        {
            var entry = new JournalEntry
            {
                Title = "New Title",
                BodyText = "New description",
                Metadata = new EntryMetadata
                {
                    Date = DateTimeOffset.Now,
                    Author = AuthorName
                }
            };

            _realm.Write(() =>
            {
                _realm.Add<JournalEntry>(entry);
            });

            var page = new JournalEntryDetailsPage(new JournalEntryDetailsViewModel(entry));

            Navigation.PushAsync(page);
        }

        internal void EditEntry(JournalEntry entry)
        {

            var page = new JournalEntryDetailsPage(new JournalEntryDetailsViewModel(entry));

            Navigation.PushAsync(page);
        }

        private void DeleteEntry(JournalEntry entry)
        {
            _realm.Write(() => _realm.Remove(entry));
        }
    }
}

