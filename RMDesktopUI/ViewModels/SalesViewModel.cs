using Caliburn.Micro;
using System.ComponentModel;

namespace RMDesktopUI.ViewModels
{
    public class SalesViewModel : Screen
    {
		private BindingList<string> _products;
		private string _itemQuantity;
        private BindingList<string> _cart;

        public BindingList<string> Cart
        {
            get 
            {
                return _cart; 
            }
            set 
            { 
                _cart = value;
                NotifyOfPropertyChange(() => Cart);
            }
        }


        public BindingList<string> Products
        {
            get
            {
                return
                    _products;
            }
            set
            {
                _products = value;
                NotifyOfPropertyChange(() => Products);
            }
        }

        public string ItemQuantity
        {
			get 
			{ 
				return _itemQuantity;
			}
			set 
			{
				_itemQuantity = value;
                NotifyOfPropertyChange(() => ItemQuantity);
            }
        }

        public bool CanAddToCart
        {
            get
            {
                bool result = false;

                // Make sure something is selected
                // Make sure there is item quantity

                return result;
            }
        }

        public bool CanRemoveFromCart
        {
            get
            {
                bool result = false;

                // Make sure something is selected

                return result;
            }
        }

        public bool CanCheckOut
        {
            get
            {
                bool result = false;

                // Make sure something is in the cart

                return result;
            }
        }

        public void CheckOut()
        {

        }

        public void AddToCart()
        {

        }

        public void RemoveFromCart()
        {

        }

        public string SubTotal
        {
            get
            {
                // Replace with calculation

                return "$0.00";
            }
        }

        public string Tax
        {
            get
            {
                // Replace with calculation

                return "$0.00";
            }
        }

        public string Total
        {
            get
            {
                // Replace with calculation

                return "$0.00";
            }
        }
    }
}
