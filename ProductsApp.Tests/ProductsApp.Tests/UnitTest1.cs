using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using Xunit;

namespace ProductsApp.Tests {

    public class ProductsAppShould
    {
        // Add your test here

        private readonly Products _products;

        public ProductsAppShould()
        {
            _products = new Products();
        }

        [Fact]
        public void ReturnAnExceptionWhenProductIsNotSpecified()
        {
            // Arrange
            Product product = null;

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => _products.AddNew(product));
        }

        [Fact]
        public void SuccessfullyAddProducts()
        {
            // Arrange
            const string myProductName = "MyProduct";

            var product = new Product
            {
                IsSold = false,
                Name = myProductName
            };

            // Act
            _products.AddNew(product);

            // Assert
            Assert.NotEmpty(_products.Items);
            Assert.Contains(product, _products.Items);
            Assert.Contains(myProductName, _products.Items.Select(x => x.Name));
        }

        [Fact]
        public void ReturnValidationErrorWhenProductNameIsNotSpecified()
        {
            // Arrange
            var product = new Product();

            // Act
            // Assert
            Assert.Throws<NameRequiredException>(() => _products.AddNew(product));
        }
    }

    internal class Products {
        private readonly List<Product> _products = new List<Product> ();

        public IEnumerable<Product> Items => _products.Where (t => !t.IsSold);

        public void AddNew (Product product) {
            product = product ??
                      throw new ArgumentNullException ();
            product.Validate ();
            _products.Add (product);
        }

        public void Sold (Product product) {
            product.IsSold = true;
        }

    }

    internal class Product {
        public bool IsSold { get; set; }
        public string Name { get; set; }

        internal void Validate () {
            Name = Name ??
                   throw new NameRequiredException ();
        }

    }

    [Serializable]
    internal class NameRequiredException : Exception {
        public NameRequiredException () { /* ... */ }

        public NameRequiredException (string message) : base (message) { /* ... */ }

        public NameRequiredException (string message, Exception innerException) : base (message, innerException) { /* ... */ }

        protected NameRequiredException (SerializationInfo info, StreamingContext context) : base (info, context) { /* ... */ }
    }
}