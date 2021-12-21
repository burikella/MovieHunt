using System.Collections;
using System.Windows.Input;
using Xamarin.Forms;

namespace MovieHunt.UserInterface.Controls
{
    public class InfiniteListView : ListView
    {
        public static readonly BindableProperty LoadMoreCommandProperty =
            BindableProperty.Create(nameof(LoadMoreCommand), typeof(ICommand), typeof(InfiniteListView));

        public static readonly BindableProperty ItemSelectedCommandProperty =
            BindableProperty.Create(nameof(ItemSelectedCommand), typeof(ICommand), typeof(InfiniteListView));

        public ICommand LoadMoreCommand
        {
            get => (ICommand)GetValue(LoadMoreCommandProperty);
            set => SetValue(LoadMoreCommandProperty, value);
        }

        public ICommand ItemSelectedCommand
        {
            get => (ICommand)GetValue(ItemSelectedCommandProperty);
            set => SetValue(ItemSelectedCommandProperty, value);
        }

        public InfiniteListView()
        {
            ItemAppearing += HandleItemAppearing;
            ItemSelected += HandleItemSelection;
            ItemTapped += OnItemTapped;
        }

        private void HandleItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            if (ItemsSource is IList items && e.Item == items[items.Count - 1])
            {
                if (LoadMoreCommand != null && LoadMoreCommand.CanExecute(null))
                {
                    LoadMoreCommand.Execute(null);
                }
            }
        }

        private void OnItemTapped(object sender, ItemTappedEventArgs itemTappedEventArgs)
        {
            ItemSelectedCommand?.Execute(itemTappedEventArgs.Item);
        }

        private void HandleItemSelection(object sender, SelectedItemChangedEventArgs selectedItemChangedEventArgs)
        {
            if (sender is ListView listView)
            {
                if (listView.SelectedItem != null || selectedItemChangedEventArgs.SelectedItem != null)
                {
                    listView.SelectedItem = null;
                }
            }
        }
    }
}