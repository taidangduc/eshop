using EShop.Domain.Entities;
namespace EShop.Persistence.Seed;

using EShop.Domain.Enums;

public static class CatalogData
{
    public static List<Category> Categories { get; set; }
    public static List<Product> Products { get; set; }
    public static List<ProductOption> ProductOptions { get; set; }
    public static List<ProductOptionValue> ProductOptionValues { get; set; }
    public static List<ProductImage> ProductImages { get; set; }
    public static List<Variant> Variants { get; set; }
    public static List<VariantOption> VariantOptionValues { get; set; }

    static CatalogData()
    {
        var category_id = new Guid("3c5c0000-97c6-fc34-a0cb-08db322230c7");

        var product_non_optionvalue_id = new Guid("3c5c0000-97c6-fc34-a0cb-08db322230d8");
        var product_have_optionvalue_id = new Guid("3c5c0000-97c6-fc34-a0cb-08db322230d9");

        var option_color_id = new Guid("3c5c0000-97c6-fc34-a0cb-08db322230e1");
        var option_size_id = new Guid("3c5c0000-97c6-fc34-a0cb-08db322230e2");

        var option_value_white_id = new Guid("3c5c0000-97c6-fc34-a0cb-08db32223100");
        var option_value_black_id = new Guid("3c5c0000-97c6-fc34-a0cb-08db32223111");
        var option_value_s_id = new Guid("3c5c0000-97c6-fc34-a0cb-08db32223113");
        var option_value_m_id = new Guid("3c5c0000-97c6-fc34-a0cb-08db32223114");

        Categories = new List<Category>
        {
           new Category { Id = category_id, Name = "Shirt" },
        };

        Products = new List<Product>
        {
            new Product
            {
                Id = product_non_optionvalue_id,
                Title = "T-Shirt Blue",
                Description = "This is a blue t-shirt",
                CategoryId = category_id,
                Status = ProductStatus.Published
            },
            new Product
            {
                Id = product_have_optionvalue_id,
                Title = "T-Shirt Coolman",
                Description = "This is a coolman t-shirt",
                CategoryId = category_id,
                Status = ProductStatus.Published
            },
        };

        ProductOptions = new List<ProductOption>
        {
            new ProductOption
            {
               Id = option_color_id,
               ProductId = product_have_optionvalue_id,
               Name = "Color",
               HasImage = true
            },
            new ProductOption
            {

               Id = option_size_id,
               ProductId = product_have_optionvalue_id,
               Name = "Size",
               HasImage = false
            }
        };

        ProductOptionValues = new List<ProductOptionValue>
        {
            new ProductOptionValue(
               optionId: option_color_id,
               value: "Black",
               imageUrl: "images/3c5c0000-97c6-fc34-a0cb-08db32223099"
            ) { Id = option_value_black_id },
            new ProductOptionValue(
               optionId: option_color_id,
               value: "White",
               imageUrl: "images/3c5c0000-97c6-fc34-a0cb-08db32223100"
            ) { Id = option_value_white_id },
            new ProductOptionValue(
               optionId: option_size_id,
               value: "S"
            ) { Id = option_value_s_id },
            new ProductOptionValue(
               optionId: option_size_id,
               value: "M"
            ) { Id = option_value_m_id },
        };

        ProductImages = new List<ProductImage>
        {
            new ProductImage
            {
               Id = Guid.CreateVersion7(),
               ProductId = product_non_optionvalue_id,
               ImageUrl = "images/3c5c0000-97c6-fc34-a0cb-08db32223101",
               SortOrder = 0,
               IsMain = true
            },

            new ProductImage
            {
               Id = Guid.CreateVersion7(),
               ProductId = product_have_optionvalue_id,
               ImageUrl = "images/3c5c0000-97c6-fc34-a0cb-08db32223102",
               SortOrder = 0,
               IsMain = true
            },

            new ProductImage
            {
               Id = Guid.CreateVersion7(),
               ProductId = product_have_optionvalue_id,
               ImageUrl = "images/3c5c0000-97c6-fc34-a0cb-08db32223103",
               SortOrder = 1,
               IsMain = false
            },
            new ProductImage
            {
               Id = Guid.CreateVersion7(),
               ProductId = product_have_optionvalue_id,
               ImageUrl = "images/3c5c0000-97c6-fc34-a0cb-08db32223104",
               SortOrder = 2,
               IsMain = false
            },
        };

        Variants = new List<Variant>
        {
            new Variant
            {
                Id = new Guid("3c5c0000-97c6-fc34-a0cb-08db32223222"),
                ProductId = product_non_optionvalue_id,
                Title = "T-Shirt Blue",
                Price = 90000,
                Quantity = 10,
                Status = VariantStatus.Active
            },

            new Variant
            {
                Id = new Guid("3c5c0000-97c6-fc34-a0cb-08db32223223"),
                ProductId = product_have_optionvalue_id,
                Title = "T-Shirt Coolman / Black - S",
                Name = "Black-S",
                ImageUrl = "images/3c5c0000-97c6-fc34-a0cb-08db32223099",
                Price = 35000,
                Quantity = 10,
                Status = VariantStatus.Active,
            },

            new Variant
            {
                Id = new Guid("3c5c0000-97c6-fc34-a0cb-08db32223224"),
                ProductId = product_have_optionvalue_id,
                Title = "T-Shirt Coolman / Black - M",
                Name = "Black-M",
                ImageUrl = "images/3c5c0000-97c6-fc34-a0cb-08db32223099",
                Price = 40000,
                Quantity = 10,
                Status = VariantStatus.Active
            },

            new Variant
            {
                Id = new Guid("3c5c0000-97c6-fc34-a0cb-08db32223225"),
                ProductId = product_have_optionvalue_id,
                Title = "T-Shirt Coolman / White - S",
                Name = "White-S",
                ImageUrl = "images/3c5c0000-97c6-fc34-a0cb-08db32223100",
                Price = 35000,
                Quantity = 10,
                Status = VariantStatus.Active
            },

            new Variant
            {
                Id = new Guid("3c5c0000-97c6-fc34-a0cb-08db32223226"),
                ProductId = product_have_optionvalue_id,
                Title = "T-Shirt Coolman / White - M",
                Name = "White-M",
                ImageUrl = "images/3c5c0000-97c6-fc34-a0cb-08db32223100",
                Price = 40000,
                Quantity = 10,
                Status = VariantStatus.Active
            },
        };

        VariantOptionValues = new List<VariantOption>()
        {
            new VariantOption
            {
                VariantId = new Guid("3c5c0000-97c6-fc34-a0cb-08db32223223"),
                OptionId = option_color_id,
                OptionValueId = option_value_black_id,
                OptionName = "Color",
                OptionValueName = "Black"
            },

            new VariantOption
            {
                VariantId = new Guid("3c5c0000-97c6-fc34-a0cb-08db32223223"),
                OptionId = option_size_id,
                OptionValueId = option_value_s_id,
                OptionName = "Size",
                OptionValueName = "S"
            },

            new VariantOption
            {
                VariantId = new Guid("3c5c0000-97c6-fc34-a0cb-08db32223224"),
                OptionId = option_color_id,
                OptionValueId = option_value_black_id,
                OptionName = "Color",
                OptionValueName = "Black"
            },

            new VariantOption
            {
                VariantId = new Guid("3c5c0000-97c6-fc34-a0cb-08db32223224"),
                OptionId = option_size_id,
                OptionValueId = option_value_m_id,
                OptionName = "Size",
                OptionValueName = "M"
            },

            new VariantOption
            {
                VariantId = new Guid("3c5c0000-97c6-fc34-a0cb-08db32223225"),
                OptionId = option_color_id,
                OptionValueId = option_value_white_id,
                OptionName = "Color",
                OptionValueName = "White"
            },

            new VariantOption
            {
                VariantId = new Guid("3c5c0000-97c6-fc34-a0cb-08db32223225"),
                OptionId = option_size_id,
                OptionValueId = option_value_s_id,
                OptionName = "Size",
                OptionValueName = "S"
            },

            new VariantOption
            {
                VariantId = new Guid("3c5c0000-97c6-fc34-a0cb-08db32223226"),
                OptionId = option_color_id,
                OptionValueId = option_value_white_id,
                OptionName = "Color",
                OptionValueName = "White"
            },

            new VariantOption
            {
                VariantId = new Guid("3c5c0000-97c6-fc34-a0cb-08db32223226"),
                OptionId = option_size_id,
                OptionValueId = option_value_m_id,
                OptionName = "Size",
                OptionValueName = "M"
            },

        };
    }
}
