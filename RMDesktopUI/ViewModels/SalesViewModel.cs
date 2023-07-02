using Caliburn.Micro;
using RMDesktopUI.Library.Api;
using RMDesktopUI.Library.Models;
using System.ComponentModel;
using System.Threading.Tasks;

namespace RMDesktopUI.ViewModels
{
    public class SalesViewModel : Screen
    {
		private BindingList<ProductModel> _products;
		private int _itemQuantity;
        private BindingList<ProductModel> _cart;
        private IProductEndpoint _productEndpoint;

        public SalesViewModel(IProductEndpoint productEndpoint)
        {
            _productEndpoint = productEndpoint;
        }

        public BindingList<ProductModel> Cart
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

        public BindingList<ProductModel> Products
        {
            get
            {
                return _products;
            }
            set
            {
                _products = value;
                NotifyOfPropertyChange(() => Products);
            }
        }

        public int ItemQuantity
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

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);

            await loadProducts();
        }

        private async Task loadProducts()
        {
            var prods = await _productEndpoint.GetAll();
            Products = new BindingList<ProductModel>(prods);
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
