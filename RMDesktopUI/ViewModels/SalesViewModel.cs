﻿using AutoMapper;
using Caliburn.Micro;
using RMDesktopUI.Library.Api;
using RMDesktopUI.Library.Helpers;
using RMDesktopUI.Library.Models;
using RMDesktopUI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace RMDesktopUI.ViewModels
{
    public class SalesViewModel : Screen
    {
        private IConfigHelper _configHelper;
        private IProductEndpoint _productEndpoint;
        private ISaleEndpoint _saleEndpoint;
        private IMapper _mapper;
        private IWindowManager _window;
        private StatusInfoViewModel _status;
        private BindingList<ProductDisplayModel> _products;
        private BindingList<CartItemDisplayModel> _cart = new BindingList<CartItemDisplayModel>();
        private ProductDisplayModel _selectedProduct;
        private CartItemDisplayModel _selectedCartItem;
        private int _itemQuantity = 1;

        public SalesViewModel(
            IProductEndpoint productEndpoint,
            IConfigHelper configHelper,
            ISaleEndpoint saleEndpoint,
            IMapper mapper,
            StatusInfoViewModel status,
            IWindowManager window)
        {
            _productEndpoint = productEndpoint;
            _configHelper = configHelper;
            _saleEndpoint = saleEndpoint;
            _mapper = mapper;
            _status = status;
            _window = window;
        }

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);

            try
            {
                await loadProducts();
            }
            catch(Exception ex)
            {
                dynamic settings = new ExpandoObject();
                settings.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                settings.ResizeMode = ResizeMode.NoResize;
                settings.Title = "System Error";

                if(ex.Message == "Unauthorized")
                {
                    _status.UpdateMessage("Unauthorize Access",
                        "You do not have a permission to interact with the sale view");
                    await _window.ShowDialogAsync(_status, null, settings);
                }
                else
                {
                    _status.UpdateMessage("Fatal Exception", ex.Message);
                    await _window.ShowDialogAsync(_status, null, settings);
                }


                await TryCloseAsync();
            }  
        }

        public BindingList<ProductDisplayModel> Products
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

        public BindingList<CartItemDisplayModel> Cart
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

        public ProductDisplayModel SelectedProduct
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

        public CartItemDisplayModel SelectedCartItem
        {
            get
            {
                return _selectedCartItem;
            }
            set
            {
                _selectedCartItem = value;
                NotifyOfPropertyChange(() => SelectedCartItem);
                NotifyOfPropertyChange(() => CanRemoveFromCart);
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

                if (SelectedCartItem != null && SelectedCartItem?.QuantityInCart > 0)
                {
                    result = true;
                }

                return result;
            }
        }

        public bool CanCheckOut
        {
            get
            {
                bool result = false;

                if(Cart.Count > 0)
                {
                    result = true;
                }

                return result;
            }
        }

        public void AddToCart()
        {
            CartItemDisplayModel existingItem = Cart.FirstOrDefault(x => x.Product == SelectedProduct);

            if (existingItem != null)
            {
                existingItem.QuantityInCart += ItemQuantity;
            }
            else
            {
                CartItemDisplayModel item = new CartItemDisplayModel
                {
                    Product = SelectedProduct,
                    QuantityInCart = ItemQuantity
                };
                Cart.Add(item);
            }

            SelectedProduct.QuantityInStock -= ItemQuantity;
            ItemQuantity = 1;
            NotifyOfPropertyChange(() => SubTotal);
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => Total);
            NotifyOfPropertyChange(() => CanCheckOut);
        }

        public void RemoveFromCart()
        {
            SelectedCartItem.Product.QuantityInStock += 1;

            if (SelectedCartItem.QuantityInCart > 1)
            {
                SelectedCartItem.QuantityInCart -= 1;
            }
            else
            {
                Cart.Remove(SelectedCartItem);
            }

            NotifyOfPropertyChange(() => SubTotal);
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => Total);
            NotifyOfPropertyChange(() => CanCheckOut);
            NotifyOfPropertyChange(() => CanAddToCart);
        }

        public async Task CheckOut()
        {
            SaleModel sale = new SaleModel();

            foreach (var item in Cart)
            {
                sale.SaleDetail.Add(new SaleDetailModel
                {
                    ProductId = item.Product.Id,
                    Quantity = item.QuantityInCart
                });
            }

            await _saleEndpoint.PostSale(sale);
            await resetSalesViewModel();
        }

        public string SubTotal
        {
            get
            {
                return calculateSubTotal().ToString("C");
            }
        }

        public string Tax
        {
            get
            {
                return calculateTax().ToString("C");
            }
        }

        public string Total
        {
            get
            {
                decimal total = calculateSubTotal() + calculateTax();

                return total.ToString("C");
            }
        }

        private async Task loadProducts()
        {
            var productList = await _productEndpoint.GetAll();
            var products = _mapper.Map<List<ProductDisplayModel>>(productList);

            Products = new BindingList<ProductDisplayModel>(products);
        }

        private decimal calculateSubTotal()
        {
            decimal subTotal = 0;

            foreach (var item in Cart)
            {
                subTotal += item.Product.RetailPrice * item.QuantityInCart;
            }

            return subTotal;
        }

        private decimal calculateTax()
        {
            decimal taxAmount = 0;
            decimal taxRate = _configHelper.GetTaxRate()/100;

            taxAmount = Cart
                .Where(x => x.Product.IsTaxable)
                .Sum(x => x.Product.RetailPrice * x.QuantityInCart * taxRate);

            return taxAmount;
        }

        private async Task resetSalesViewModel()
        {
            Cart = new BindingList<CartItemDisplayModel>();

            await loadProducts();

            NotifyOfPropertyChange(() => SubTotal);
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => Total);
            NotifyOfPropertyChange(() => CanCheckOut);
        }
    }
}
