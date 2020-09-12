using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace QUT
{
    class ViewModel: INotifyPropertyChanged
    {
        public ObservableCollection<Cards.Card> HumanCards { get; private set; }
        public ObservableCollection<Cards.Card> ComputerCards { get; private set; }
        public ObservableCollection<Cards.Card> Discards { get; private set; }
        public ObservableCollection<Cards.Card> RemainingDeck { get; private set; }

        public InteractionRequest<INotification> NotificationRequest { get; private set; }

        public ICommand ButtonCommand { get; set; }
        public ICommand DiscardCardFromHandCommand { get; set; }
        public ICommand TakeCardFromDiscardPileCommand { get; set; }
        public ICommand TakeCardFromDeckCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public bool pickUpStage = false;
        public bool discardStage = false;
        public bool computerStage = false;

        public ViewModel()
        {
            TakeCardFromDiscardPileCommand = new DelegateCommand<Cards.Card>(TakeCardFromDiscardPile);
            DiscardCardFromHandCommand = new DelegateCommand<Cards.Card>(DiscardCardFromHand);
            TakeCardFromDeckCommand = new DelegateCommand<Cards.Card>(TakeCardFromDeck);

            ButtonCommand = new DelegateCommand(ButtonClick);
            NotificationRequest = new InteractionRequest<INotification>();

            HumanCards = new ObservableCollection<Cards.Card>();
            ComputerCards = new ObservableCollection<Cards.Card>();
            Discards = new ObservableCollection<Cards.Card>();
            RemainingDeck = new ObservableCollection<Cards.Card>();

            HumanCards.CollectionChanged += HumanCards_CollectionChanged;

            Deal();
        }

        private async void Deal()
        {
            var deck = Cards.Shuffle(Cards.FullDeck);

            foreach (var card in deck)
            {
                RemainingDeck.Add(card);
                await Task.Delay(1);
            }

            for (int i = 0; i < 10; i++)
            {
                ComputerCards.Add(DrawTopCardFromDeck());
                await Task.Delay(30);
                HumanCards.Add(DrawTopCardFromDeck());
                await Task.Delay(30);
            }

            Discards.Add(DrawTopCardFromDeck());
            pickUpStage = true;
        }

        private Cards.Card DrawTopCardFromDeck()
        {
            var top = RemainingDeck[RemainingDeck.Count - 1];
            RemainingDeck.Remove(top);
            return top;
        }

        private void TakeCardFromDeck(Cards.Card card)
        {
            if (pickUpStage)
            {
                RemainingDeck.Remove(card);
                HumanCards.Add(card);
                pickUpStage = false;
                discardStage = true;
            }
            else
            {
                RaiseNotification("Not in Pick-Up Stage", "Title");
            }
        }

        private void TakeCardFromDiscardPile(Cards.Card p)
        {
            if (pickUpStage)
            {
                Discards.Remove(p);
                HumanCards.Add(p);
                pickUpStage = false;
                discardStage = true;
            }
            else
            {
                RaiseNotification("Not in Pick-Up Stage", "Title");
            }
        }

        private void UpdateButton() 
        {
            //check human's hand to see the highest value card that is not in a set
            //if you subtract that from the deadwood is it under 10?
            //if so then display "knock for x" on the button
            //set some var so that other parts of code know that this is the action required for the button

            //VERY similar to GIN implementation, except gin will be if deadwood is zero after one card is removed
        }

        private void DiscardCardFromHand(Cards.Card p)
        {
            if (discardStage)
            {
                HumanCards.Remove(p);
                Discards.Add(p);
                discardStage = false;
                computerStage = true;
                ComputerPlayerTurn();
            }
            else
            {
                RaiseNotification("Not in Discard Stage", "Title");
            }
        }

        private void ComputerPlayerTurn()
        {
            Cards.Card card = DrawTopCardFromDeck();
            ComputerCards.Add(card);
            ComputerCards.Remove(card);

            Discards.Add(card);
            computerStage = false;
            pickUpStage = true;
        }

        async private void HumanCards_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            HumanDeadwood = "Calculating ...";
            // this might take a while, so let's do it in the background
            int deadwood = await Task.Run(() => GinRummy.Deadwood(HumanCards));
            HumanDeadwood = "Deadwood: " + deadwood;
        }

        private string humanDeadwood;

        public string HumanDeadwood 
        { 
            get
            {
                return humanDeadwood;
            }
            private set
            {
                humanDeadwood = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("HumanDeadwood"));
            }
        }

        private void RaiseNotification(string msg, string title)
        {
            NotificationRequest.Raise(new Notification { Content = msg, Title = title });
        }

        private void ButtonClick()
        {
            RaiseNotification("You clicked the Button!", "Title");
        }

        private void shuffle()
        {

        }
    }
}