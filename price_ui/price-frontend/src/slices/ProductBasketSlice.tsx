import { createAsyncThunk, createSlice } from '@reduxjs/toolkit';
import Item from '../components/Models/Item';


// get products from database
export const getProduct = createAsyncThunk(
  'databaseProductApi/getProduct',
  async() => {
    const response: Array<Item> = await fetch('https://localhost:7135/items').then(
      (data) => data.json()
    );
    return response;
  }
);

// filter products by search
export const filterProduct = createAsyncThunk(
  'filterProducts',
  async (values: Array<Array<Item>|string>) => {
   const products = values[0] as Array<Item>;
   const searchTerm = values[1] as string;
   let result: Array<Item> = products.filter((product: Item) => {
      // if searchTerm is blank, return products with no filter
      if (searchTerm === '') {
        return products;
      // else return products with filtered items 
      } else if (product.productName.toLowerCase().includes(searchTerm.toLowerCase())) {
        return products;
      }
    });
  return result;
  }
)

export const productBasketSlice = createSlice({
	name: 'productBasket',
	initialState: {
    products: [] as Array<Item>,
    filteredProducts: [] as Array<Item>,
		status: ''
  },
  reducers: {
    reset(state) {
      state.products = [] as Array<Item>
      state.filteredProducts = [] as Array<Item>
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
