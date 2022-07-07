import { createAsyncThunk, createSlice } from '@reduxjs/toolkit';
import ProductModel from '../components/ProductModel';


// get products from database
export const getProduct = createAsyncThunk(
  'databaseProductApi/getProduct',
  async() => {
    const response: Array<ProductModel> = await fetch('https://localhost:7135/items').then(
      (data) => data.json()
    );
    return response;
  }
);

// filter products by search
export const filterProduct = createAsyncThunk(
  'filterProducts',
 async (values: Array<Array<ProductModel>|string>) => {
   const products = values[0] as Array<ProductModel>;
   const searchTerm = values[1] as string;
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
	name: 'productBasket',
	initialState: {
    products: [] as Array<ProductModel>,
    filteredProducts: [] as Array<ProductModel>,
		status: ''
  },
  reducers: {
    reset(state) {
      state.products = [] as Array<ProductModel>
      state.filteredProducts = [] as Array<ProductModel>
      state.status = ''
    }
  },
  // extra reducers handle async requests
  extraReducers: (builder) => {
    builder
    .addCase(getProduct.fulfilled, (state, action) => {
			state.status = 'success';
      state.products = action.payload;
      state.filteredProducts = action.payload;
    })

		.addCase(getProduct.pending, (state) => {
			state.status = 'loading';
      state.products = [];
      state.filteredProducts = [];
		})

		.addCase(getProduct.rejected, (state) => {
			state.status = 'failed';
      state.products = [];
      state.filteredProducts = [];
		})

    .addCase(filterProduct.fulfilled, (state, action) => {
      state.filteredProducts = action.payload;
    })
  }
});

export const { reset } = productBasketSlice.actions;
export default productBasketSlice.reducer;
