 // define types for product elements
 interface ProductModel {
    product_name: string;
    unit_of_measure_size: number;
    unit_of_measure: string;
    price_sale: number;
    price: number;
    is_available: boolean;
    is_onsale: boolean;
    store_name: string;
    price_date: string;
  }

export default ProductModel;