import { createAsyncThunk, createSlice } from '@reduxjs/toolkit'


interface ProductType {
  product_name?: string;
}


export const getProduct: any = createAsyncThunk(
  'databaseProductApi/getProduct',
  async(thunkAPI) => {
    const response: Object[] = await fetch('http://127.0.0.1:5000/retrieve_product').then(
      (data) => data.json()
    );
    return response
  }
);


export const filterProducts: any = createAsyncThunk(
  'filterProducts',
 async (values: any[]) => {
   const productsList = values[0]
   const searchTerm = values[1]
   let result = productsList.filter((product: ProductType) => {
      // if searchTerm is blank, return products with not filter
      if (searchTerm === '') {
        return productsList;
      // else return products with filtered items 
      } else if ((product.product_name as string).toLowerCase().includes(searchTerm.toLowerCase())) {
        return productsList;
      }
    });
  return result;
  }
)

export const productBasketSlice = createSlice({
	name: 'productApi',
	initialState: {
    products: [],
    filteredProducts: [],
		status: ''
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
