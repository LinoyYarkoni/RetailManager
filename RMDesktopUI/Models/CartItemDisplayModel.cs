using System.ComponentModel;

namespace RMDesktopUI.Models
{
    public class CartItemDisplayModel : INotifyPropertyChanged
    {
        private int _quantityInCart;

        public ProductDisplayModel Product { get; set; }

        public int QuantityInCart 
        { 
            get
            {
                return _quantityInCart;
            }
            set
            {
                _quantityInCart = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(QuantityInCart)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DisplayText)));
            }
        }

        public string DisplayText
        {
            get
            {
                return $"{Product.ProductName} ({QuantityInCart})";
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
