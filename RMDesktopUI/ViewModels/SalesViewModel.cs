using Caliburn.Micro;
using RMDesktopUI.Library.Api;
using RMDesktopUI.Library.Models;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace RMDesktopUI.ViewModels
{
    public class SalesViewModel : Screen
    {
		private BindingList<ProductModel> _products;
		private int _itemQuantity = 1;
        private BindingList<CartItemModel> _cart = new BindingList<CartItemModel>();
        private IProductEndpoint _productEndpoint;
        private ProductModel _selectedProduct;

        public SalesViewModel(IProductEndpoint productEndpoint)
        {
            _productEndpoint = productEndpoint;
        }

        public ProductModel SelectedProduct
        {
            get
            { 
                return _selectedProduct;
            }
            set 
            {
                _selectedProduct = value;
                NotifyOfPropertyChange(() => SelectedProduct);
                NotifyOfPropertyChange(() => CanAddToCart);
            }
        }

        public BindingList<CartItemModel> Cart
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
                NotifyOfPropertyChange(() => CanAddToCart);
            }
        }

        public bool CanAddToCart
        {
            get
            {
                bool result = false;

                if(ItemQuantity > 0 && SelectedProduct?.QuantityInStock >= ItemQuantity)
                {
                    result = true;
                }

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
            CartItemModel existingItem = Cart.FirstOrDefault(x => x.Product == SelectedProduct);

            if (existingItem != null) 
            {
                existingItem.QuantityInCart += ItemQuantity;
                Cart.Remove(existingItem);
                Cart.Add(existingItem);
            }
            else
            {
                CartItemModel item = new CartItemModel
                {
                    Product = SelectedProduct,
                    QuantityInCart = ItemQuantity
                };
                Cart.Add(item);
            }

            SelectedProduct.QuantityInStock -= ItemQuantity;
            ItemQuantity = 1;
            NotifyOfPropertyChange(() => SubTotal);
        }

        public void RemoveFromCart()
        {
            NotifyOfPropertyChange(() => SubTotal);
        }

        public string SubTotal
        {
            get
            {
                decimal subTotal = 0;
                
                foreach(var item in Cart)
                {
                    subTotal += item.Product.RetailPrice * item.QuantityInCart;
                }

                return subTotal.ToString("C");
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
