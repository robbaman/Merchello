﻿using Merchello.Core.Models;
using Merchello.Core.Models.Rdbms;

namespace Merchello.Core.Persistence.Factories
{
    internal class ProductFactory : IEntityFactory<IProduct, ProductDto>
    {

        private readonly ProductVariantFactory _productVariantFactory;
        private readonly ProductOptionCollection _productOptionCollection;
        private readonly ProductVariantCollection _productVariantCollection;

        public ProductFactory()
            : this(new ProductAttributeCollection(), new CatalogInventoryCollection(), new ProductOptionCollection(), new ProductVariantCollection())
        {}

        public ProductFactory(ProductAttributeCollection productAttributes,
            CatalogInventoryCollection catalogInventories, ProductOptionCollection productOptions, ProductVariantCollection productVariantCollection)
        {
            _productVariantFactory = new ProductVariantFactory(productAttributes, catalogInventories);
            _productOptionCollection = productOptions;
            _productVariantCollection = productVariantCollection;
        }

        public IProduct BuildEntity(ProductDto dto)
        {
            var variant = _productVariantFactory.BuildEntity(dto.ProductVariantDto);
            var product = new Product(variant)
            {
                Key = dto.Key,
                ProductOptions = _productOptionCollection,
                ProductVariants = _productVariantCollection,
                UpdateDate = dto.UpdateDate,
                CreateDate = dto.CreateDate
            };

            product.ResetDirtyProperties();

            return product;
        }

        public ProductDto BuildDto(IProduct entity)
        {
            
            var dto = new ProductDto()
            {
                Key = entity.Key,
                UpdateDate = entity.UpdateDate,
                CreateDate = entity.CreateDate,
                ProductVariantDto = _productVariantFactory.BuildDto(((Product)entity).MasterVariant)
            };

            return dto;
        }

    }
}
