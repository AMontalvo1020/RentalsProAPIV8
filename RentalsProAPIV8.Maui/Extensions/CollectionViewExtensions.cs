using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalsProAPIV8.Maui.Extensions
{
    public static class CollectionViewExtensions
    {
        /// <summary>
        /// Binds an ObservableCollection to the ItemsSource of the CollectionView.
        /// </summary>
        public static void BindItems<T>(this CollectionView collectionView, ObservableCollection<T> items)
        {
            if (collectionView == null) throw new ArgumentNullException(nameof(collectionView));
            collectionView.ItemsSource = items;
        }

        /// <summary>
        /// Adds a new item to the ObservableCollection bound to the CollectionView.
        /// </summary>
        public static void AddItem<T>(this CollectionView collectionView, ObservableCollection<T> items, T newItem)
        {
            if (collectionView == null) throw new ArgumentNullException(nameof(collectionView));
            if (items == null) throw new ArgumentNullException(nameof(items));

            items.Add(newItem);
        }

        /// <summary>
        /// Removes an item from the ObservableCollection bound to the CollectionView.
        /// </summary>
        public static void RemoveItem<T>(this CollectionView collectionView, ObservableCollection<T> items, T itemToRemove)
        {
            if (collectionView == null) throw new ArgumentNullException(nameof(collectionView));
            if (items == null) throw new ArgumentNullException(nameof(items));

            items.Remove(itemToRemove);
        }

        /// <summary>
        /// Clears all items from the ObservableCollection bound to the CollectionView.
        /// </summary>
        public static void ClearItems<T>(this CollectionView collectionView, ObservableCollection<T> items)
        {
            if (collectionView == null) throw new ArgumentNullException(nameof(collectionView));
            if (items == null) throw new ArgumentNullException(nameof(items));

            items.Clear();
        }

        /// <summary>
        /// Scrolls to a specific item in the CollectionView.
        /// </summary>
        public static void ScrollToItem<T>(this CollectionView collectionView, T item, ScrollToPosition position = ScrollToPosition.MakeVisible, bool animate = true)
        {
            if (collectionView == null) throw new ArgumentNullException(nameof(collectionView));

            collectionView.ScrollTo(item, position: position, animate: animate);
        }

        /// <summary>
        /// Selects a specific item in the CollectionView.
        /// </summary>
        public static void SelectItem<T>(this CollectionView collectionView, T item)
        {
            if (collectionView == null) throw new ArgumentNullException(nameof(collectionView));
            collectionView.SelectedItem = item;
        }

        /// <summary>
        /// Deselects all items in the CollectionView.
        /// </summary>
        public static void DeselectAllItems(this CollectionView collectionView)
        {
            if (collectionView == null) throw new ArgumentNullException(nameof(collectionView));
            collectionView.SelectedItem = null;
        }

        /// <summary>
        /// Filters the items in the CollectionView based on a predicate.
        /// </summary>
        public static void FilterItems<T>(this CollectionView collectionView, ObservableCollection<T> items, Func<T, bool> predicate)
        {
            if (collectionView == null) throw new ArgumentNullException(nameof(collectionView));
            if (items == null) throw new ArgumentNullException(nameof(items));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            var filteredItems = new ObservableCollection<T>(items.Where(predicate));
            collectionView.ItemsSource = filteredItems;
        }

        /// <summary>
        /// Refreshes the CollectionView manually by reassigning the ItemsSource.
        /// </summary>
        public static void Refresh(this CollectionView collectionView)
        {
            if (collectionView == null) throw new ArgumentNullException(nameof(collectionView));

            var itemsSource = collectionView.ItemsSource;
            collectionView.ItemsSource = null;
            collectionView.ItemsSource = itemsSource;
        }

        /// <summary>
        /// Sorts the items in the CollectionView based on a comparer.
        /// </summary>
        public static void SortItems<T>(this CollectionView collectionView, ObservableCollection<T> items, IComparer<T> comparer)
        {
            if (collectionView == null) throw new ArgumentNullException(nameof(collectionView));
            if (items == null) throw new ArgumentNullException(nameof(items));
            if (comparer == null) throw new ArgumentNullException(nameof(comparer));

            var sortedItems = new ObservableCollection<T>(items.OrderBy(item => item, comparer));
            collectionView.ItemsSource = sortedItems;
        }

        /// <summary>
        /// Adds multiple items to the ObservableCollection bound to the CollectionView.
        /// </summary>
        public static void AddItems<T>(this CollectionView collectionView, ObservableCollection<T> items, IEnumerable<T> newItems)
        {
            if (collectionView == null) throw new ArgumentNullException(nameof(collectionView));
            if (items == null) throw new ArgumentNullException(nameof(items));
            if (newItems == null) throw new ArgumentNullException(nameof(newItems));

            foreach (var newItem in newItems)
            {
                items.Add(newItem);
            }
        }
    }
}
