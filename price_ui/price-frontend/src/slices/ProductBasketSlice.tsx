import { createAsyncThunk, createSlice } from '@reduxjs/toolkit'
import ProductModel from '../components/ProductModel';


// get products from database
export const getProduct: any = createAsyncThunk(
  'databaseProductApi/getProduct',
  async() => {
    const response: Array<ProductModel> = await fetch('http://127.0.0.1:5000/product').then(
      (data) => data.json()
    );
    return response;
  }
);

// filter products by search
export const filterProducts: any = createAsyncThunk(
  'filterProducts',
 async (values: Array<any>) => {
   const products: Array<ProductModel> = values[0]
   const searchTerm: string = values[1]
   let result: Array<ProductModel> = products.filter((product: ProductModel) => {
      // if searchTerm is blank, return products with no filter
      if (searchTerm === '') {
        return products;
      // else return products with filtered items 
      } else if (product.product_name.toLowerCase().includes(searchTerm.toLowerCase())) {
        return products;
      }
    });
  return result;
  }
)

export const productBasketSlice = createSlice({
	name: 'productApi',
	initialState: {
    products: [] as Array<ProductModel>,
    filteredProducts: [] as Array<ProductModel>,
		status: '' as string
  },
  reducers: {},
  // extra reducers handle async requests
  extraReducers: {
    [getProduct.fulfilled]: (state, action) => {
			state.status = 'success';
      state.products = action.payload;
      state.filteredProducts = action.payload;
    },

		[getProduct.pending]: (state) => {
			state.status = 'loading';
      state.products = [];
      state.filteredProducts = [];
		},

		[getProduct.rejected]: (state) => {
			state.status = 'failed';
      state.products = [];
      state.filteredProducts = [];
		},

    [filterProducts.fulfilled]: (state, action) => {
      state.filteredProducts = action.payload;
    }
  }
});

export default productBasketSlice.reducer;
